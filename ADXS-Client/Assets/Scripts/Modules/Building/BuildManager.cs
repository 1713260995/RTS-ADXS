using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            terrain = GetComponent<Terrain>();
            LoadPreBuildings();
        }

        private Terrain terrain;
        private MapCellNode[,] mapCells;
        private Vector2Int gridSize;

        private Dictionary<GameUnitName, GameBuildingCtrl> preBuildings;

        private void LoadPreBuildings()
        {
            preBuildings = new Dictionary<GameUnitName, GameBuildingCtrl>();
            foreach (var pb in buildingsPrefab)
            {
                var obj = Instantiate(pb);
                preBuildings[pb.unitName] = obj;
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

                    var center = GetCellLocalPosition(i, j);
                    mapCells[i, j].height = center.y;
                    var steepness = terrainData.GetSteepness(center.x / terrainData.size.x, center.z / terrainData.size.z);
                    mapCells[i, j].steepness = steepness;
                }
            }
        }


        /// <summary>
        /// 根据索引获取cell中心点的本地坐标
        /// </summary>
        public Vector3 GetCellLocalPosition(int x, int y)
        {
            Vector3 cellLocalPos = new(cellSize.x * (x + 0.5f), 0, cellSize.y * (y + 0.5f));//+0.5是因为cell的中心点需要偏移
            cellLocalPos.y = terrain.SampleHeight(cellLocalPos);
            return cellLocalPos;
        }


    }


    public struct MapCellNode
    {
        public float height;
        public float steepness;
        public GameBuildingCtrl current;
    }
}