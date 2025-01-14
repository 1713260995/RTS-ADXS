using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Role
{
    /// <summary>
    /// 1. 初始化网格数据，计算每个网格点的高度,坡度
    /// 2. 添加测试按钮，按下按钮生成预览建筑，该建筑随
    /// </summary>
    public class BuildManager : MonoBehaviour
    {
        [SerializeField] private Vector2 cellSize = new(1, 1);
        [SerializeField] private List<GameBuildingCtrl> buildingsPrefab;
        private Terrain terrain;
        private Camera mainCamera;

        private void Awake()
        {
            terrain = GetComponent<Terrain>();
            mainCamera = mainCamera == null ? Camera.main : mainCamera;
            LoadPreBuildings();
            InitMapCells();
        }

        private void Update()
        {
            UpdatePreview();
        }

        #region Cell

        private MapCellNode[,] mapCells;
        /// <summary>
        /// 整个的网格大小
        /// gridSize.x在X轴方向cell的数量
        /// gridSize.y在Z轴方向cell的数量
        /// Tips: cell指单元格，grid是由多个cell组成的结构或区域
        /// </summary>
        private Vector2Int gridSize;

        private Dictionary<GameUnitName, GameBuildingCtrl> preBuildings;

        private void LoadPreBuildings()
        {
            preBuildings = new Dictionary<GameUnitName, GameBuildingCtrl>();
            foreach (var prefab in buildingsPrefab)
            {
                preBuildings[prefab.unitName] = prefab;
            }
        }

        // 初始化格子，计算每个格子的高度和坡度
        private void InitMapCells()
        {
            var terrainData = terrain.terrainData;
            gridSize.x = (int)(terrainData.bounds.size.x / cellSize.x);
            gridSize.y = (int)(terrainData.bounds.size.z / cellSize.y);

            mapCells = new MapCellNode[gridSize.x, gridSize.y];
            for (int i = 0; i < gridSize.x; ++i)
            {
                for (int j = 0; j < gridSize.y; ++j)
                {
                    mapCells[i, j].current = null;

                    var center = GetCellLocalPosByIndex(i, j);
                    mapCells[i, j].height = center.y;
                    var steepness = terrainData.GetSteepness(center.x / terrainData.size.x, center.z / terrainData.size.z);
                    mapCells[i, j].steepness = steepness;
                }
            }
        }


        /// <summary>
        /// 根据索引获取cell中心点的本地坐标
        /// </summary>
        public Vector3 GetCellLocalPosByIndex(int indexX, int indexY)
        {
            Vector3 cellLocalPos = new(cellSize.x * (indexX + 0.5f), 0, cellSize.y * (indexY + 0.5f));//+0.5是因为cell的中心点需要偏移
            cellLocalPos.y = terrain.SampleHeight(cellLocalPos);
            return cellLocalPos;
        }
        #endregion

        #region Preview


        private GameBuildingCtrl previewBuilding;
        public GameUnitName previewUnitName;
        public Transform buildGridLine;
        public float maxheightDifference;//地形最大高度差

        [ShowButton]
        public void TestBuild()
        {
            EnterPreview(previewUnitName);
        }

        private void EnterPreview(GameUnitName name)
        {
            var ctrl = Instantiate(preBuildings[name]);
            previewBuilding = ctrl;
        }

        private void CancelPreview()
        {
            Destroy(previewBuilding.gameObject);
            previewBuilding = null;
            buildGridLine.gameObject.SetActive(false);
        }



        private void UpdatePreview()
        {
            if (previewBuilding == null)
            {
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                CancelPreview();
                return;
            }

            Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, 1 << terrain.gameObject.layer);
            previewBuilding.transform.position = hit.point;
            buildGridLine.gameObject.SetActive(true);
            buildGridLine.position = new Vector3(hit.point.x, hit.point.y + 10, hit.point.z);
            GetGridCellIndex(hit.point, out int index1, out int index2);
            print($"index1={index1},index2={index2},{GetCellLocalPosByIndex(index1, index2)}");
            CheckBuildingIndexPosOnGrid(index1, index2);
        }

        /// <summary>
        /// 通过世界坐标获取地形网格的指定单元格索引
        /// </summary>
        public void GetGridCellIndex(Vector3 worldPos, out int index1, out int index2)
        {
            Vector3 localPosTerrain = terrain.transform.InverseTransformPoint(worldPos);//将世界坐标转换为地形的本地坐标
            index1 = (int)(localPosTerrain.x / cellSize.x);
            index2 = (int)(localPosTerrain.z / cellSize.y);
        }


        public bool CheckBuildingIndexPosOnGrid(int index1, int index2)
        {
            bool canBuild = true;
            previewBuilding.GetBuildingSize(out float lenX, out float _, out float lenZ);

            float aheight = GetTerrieAverageHeight(index1, index2, (int)lenX, (int)lenZ);
            Debug.Log("aheight:" + aheight);


            return canBuild;
        }

        /// <summary>
        /// 计算指定区域地形的平均高度，根据cell索引以及模型长宽，
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="lenX">指定区域x轴方向的长度</param>
        /// <param name="lenZ">指定区域z轴方向的长度</param>
        /// <returns></returns>
        public float GetTerrieAverageHeight(int index1, int index2, int lenX, int lenZ)
        {
            int cellXNum = Mathf.CeilToInt(lenX / cellSize.x);//获取该模型在地形的x轴占多少个格子
            int cellZNum = Mathf.CeilToInt(lenZ / cellSize.y);//获取该模型在地形的z轴占多少个格子
            int cellXIndex = index1 - cellXNum / 2;
            int cellZIndex = index2 - cellZNum / 2;
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            for (int i = cellXIndex; i < cellXIndex + cellXNum; i++)
            {
                if (i < 0 || i >= gridSize.x)
                    throw new System.Exception("Model at the edge of terrain");
                for (int j = cellZIndex; j < cellZIndex + cellZNum; j++)
                {
                    if (j < 0 || j > gridSize.y)
                        throw new System.Exception("Model at the edge of terrain");
                    float height = mapCells[i, j].height;
                    minHeight = Mathf.Min(minHeight, height);
                    maxHeight = Mathf.Max(maxHeight, height);
                }
            }

            return maxHeight - minHeight;
        }



        #endregion

    }


    public struct MapCellNode
    {
        public float height;
        public float steepness;
        public GameBuildingCtrl current;
    }
}