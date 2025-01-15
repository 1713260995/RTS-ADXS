using Assets.Scripts.Common.Enum;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Role
{
    /// <summary>
	///	1. 准备建造时，建造物变成预览样式。
	///	2. 预览时，建筑随鼠标移动
	///	3. 若地形不平坦或者地形上有其他障碍物，则提示无法建筑。
	///	4. 按鼠标右键取消建造。
    /// 5. 按下鼠标左键，开始建造。
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
        }

        private void Update()
        {
            UpdatePreview();
        }

        #region Prefab


        private Dictionary<GameUnitName, GameBuildingCtrl> preBuildings;

        private void LoadPreBuildings()
        {
            preBuildings = new Dictionary<GameUnitName, GameBuildingCtrl>();
            foreach (var prefab in buildingsPrefab)
            {
                preBuildings[prefab.unitName] = prefab;
            }
        }

        #endregion

        #region Preview


        private GameBuildingCtrl currentBuilding;
        public GameUnitName previewUnitName;
        public float maxheightDifference;//地形最大高度差

        [ShowButton]
        public void TestBuild()
        {
            EnterPreview(previewUnitName);
        }

        private void EnterPreview(GameUnitName name)
        {
            //if (Application.isEditor)
            //{
            //    UniTask.Create(async () =>
            //    {
            //        await UniTask.Delay(1000);
            //        currentBuilding = Instantiate(preBuildings[name]);
            //    });
            //    return;
            //}
            currentBuilding = Instantiate(preBuildings[name]);
        }

        private void CancelPreview()
        {
            Destroy(currentBuilding.gameObject);
            currentBuilding = null;
        }

        private void StartBuild()
        {
            currentBuilding.StartBuild();
            currentBuilding = null;

        }


        private void UpdatePreview()
        {
            if (currentBuilding == null)
            {
                return;
            }
            if (Input.GetMouseButtonDown(1))
            {
                CancelPreview();
                return;
            }

            Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, 1 << terrain.gameObject.layer);
            currentBuilding.transform.position = hit.point;
            Vector3 buildingSize = currentBuilding.GetBuildingSize();
            bool canBuild = IsTerrainFlat(hit.point, buildingSize.x, buildingSize.z) && !IsCollision();

            if (canBuild)
            {
                currentBuilding.SetNormalPreview();
            }
            else
            {
                currentBuilding.SetFailedPreview();
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!canBuild)
                {
                    print("无法建造");
                }
                else
                {
                    StartBuild();
                }
            }
        }


        public float maxHeightDifference = 0.5f; // 允许的最大高度差

        /// <summary>
        /// 判断指定区域的地形是否平坦
        /// </summary>
        /// <param name="worldPos">该区域的世界坐标</param>
        /// <param name="lenX">该区域的长度</param>
        /// <param name="lenZ">该区域的宽度</param>
        /// <returns></returns>
        public bool IsTerrainFlat(Vector3 worldPos, float lenX, float lenZ)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 localPos = terrain.transform.InverseTransformPoint(worldPos);
            float startX = localPos.x - lenX / 2;
            float endX = localPos.x + lenX / 2;
            float startZ = localPos.z - lenZ / 2;
            float endZ = localPos.z + lenZ / 2;
            if (startX < 0 || endX > terrainData.size.x || startZ < 0 || endZ > terrainData.size.z)
            {
                Debug.Log("该区域超出边界");
                return false;
            }
            float minHeight = float.MaxValue;
            float maxHeight = float.MinValue;
            // 以一定间隔采样区域内的高度
            int sampleXNum = Mathf.CeilToInt(lenX);
            int sampleZNum = Mathf.CeilToInt(lenZ);
            for (int i = 0; i <= sampleXNum; i++)
            {
                for (int j = 0; j <= sampleZNum; j++)
                {
                    float sampleX = Mathf.Lerp(startX, endX, i / (float)sampleXNum);
                    float sampleZ = Mathf.Lerp(startZ, endZ, j / (float)sampleZNum);
                    float normalizedX = sampleX / terrainData.size.x;
                    float normalizedZ = sampleZ / terrainData.size.z;
                    float height = terrainData.GetInterpolatedHeight(normalizedX, normalizedZ);
                    // 更新最小和最大高度
                    minHeight = Mathf.Min(minHeight, height);
                    maxHeight = Mathf.Max(maxHeight, height);
                    // 如果高度差超过限制，直接返回
                    if (maxHeight - minHeight > maxHeightDifference)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsCollision()
        {
            Vector3 buildingSize = currentBuilding.GetBuildingSize() / 2;
            float radius = 1;
            buildingSize.x += radius;//给x,z轴额外增加一些距离
            buildingSize.z += radius;
            Transform tran = currentBuilding.transform;
            Collider[] colliders = Physics.OverlapBox(tran.position, buildingSize, tran.rotation, GameLayerName.GameUnit.GetLayerMask());
            foreach (var collider in colliders)
            {
                if (collider.transform != tran)//碰撞检测时，会碰撞到自己
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }

}