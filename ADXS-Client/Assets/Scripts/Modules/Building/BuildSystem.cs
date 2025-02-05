using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Spawn;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
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
    public class BuildSystem : SystemBase<BuildSystem>
    {
        private Terrain terrain;
        private Camera mainCamera;

        protected override void Awake()
        {
            base.Awake();
            terrain = FindObjectOfType<Terrain>();
            mainCamera = mainCamera == null ? Camera.main : mainCamera;
        }



        #region Preview

        [SerializeField] private Color normalGridColor = new Color(18 / 255f, 52 / 255f, 191 / 255f, 1f); //预览建筑，正常的网格颜色
        [SerializeField] private Color faileGridColor = new Color(191 / 255f, 19 / 255f, 18 / 255f, 1f);//预览建筑，处于碰撞的网格颜色
        [SerializeField] private Color previewBuildingColor = new Color(0f, 66 / 255f, 299 / 255f, 194 / 255f);//预览建筑的颜色
        [SerializeField] private float colorIntensity = 4.7f;//网格颜色的强度。因为网格颜色的类型时HDR
        [SerializeField] private float maxHeightDifference = 0.55f; // 建造时允许最大的地形高度差
        [SerializeField] private GameObject previewGridPrefab; //预览的网格预制体
        [SerializeField] private Material buildMaterialPrefab; //预览建筑需要添加的材质的预制体
        [SerializeField] private float colorLerp = 5f;

        private readonly int shaderId_BuildProgress = Shader.PropertyToID("_BuildValue");//shader属性-建造进度
        private readonly int shaderId_PreviewGridColor = Shader.PropertyToID("_Color");//shader属性-网格颜色
        private readonly int shaderId_PreviewBuildingColor = Shader.PropertyToID("_NoneBuildColor");//shader属性-预览时建造的颜色

        public void EnterPreview(GameUnitName name, FarmerCtrl farmer)
        {
            GameBuildingCtrl previewBuilding = SpawnSystem.Instance.CreateCtrl<GameBuildingCtrl>(name);
            previewBuilding.SetAgent(farmer.agent);
            StartCoroutine(UpdatePreview(previewBuilding, farmer));
        }

        private void CancelPreview(GameBuildingCtrl previewBuilding)
        {
            Destroy(previewBuilding.gameObject);
            previewBuilding = null;
        }

        private IEnumerator UpdatePreview(GameBuildingCtrl previewBuilding, FarmerCtrl farmer)
        {
            GameObject previewGrid = Instantiate(previewGridPrefab, previewBuilding.transform);
            Material previewGridMat = previewGrid.GetComponent<Projector>().material;
            previewGrid.transform.localPosition = new Vector3(0, 10, 0);

            Material buildMat = Instantiate(buildMaterialPrefab);
            previewBuilding.AddBuildMat(buildMat);
            buildMat.SetColor(shaderId_PreviewBuildingColor, previewBuildingColor);
            while (true)
            {
                if (!previewBuilding.isInitComplete)
                {
                    yield return null;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    CancelPreview(previewBuilding);
                    yield break;
                }

                Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, 1 << terrain.gameObject.layer);
                previewBuilding.transform.position = hit.point;
                Vector3 buildingSize = previewBuilding.GetBuildingSize();
                bool canBuild = IsTerrainFlat(hit.point, buildingSize.x, buildingSize.z) && !IsCollideWithUnits(previewBuilding);
                if (canBuild)
                {
                    Color hdrColor = normalGridColor * colorIntensity;
                    Color currentColor = previewGridMat.GetColor(shaderId_PreviewGridColor);
                    hdrColor = Color.Lerp(currentColor, hdrColor, colorLerp * Time.deltaTime);

                    previewGridMat.SetColor(shaderId_PreviewGridColor, hdrColor);
                }
                else
                {
                    Color hdrColor = faileGridColor * colorIntensity;

                    Color currentColor = previewGridMat.GetColor(shaderId_PreviewGridColor);
                    hdrColor = Color.Lerp(currentColor, hdrColor, colorLerp * Time.deltaTime);

                    previewGridMat.SetColor(shaderId_PreviewGridColor, hdrColor);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!canBuild)
                    {
                        print("无法建造");
                    }
                    else
                    {
                        Destroy(previewGrid.gameObject);
                        buildMat.SetColor(shaderId_PreviewBuildingColor, new Color(0, 0, 0, 0));
                        StartBuild(previewBuilding, buildMat, farmer);
                        yield break;
                    }
                }
                yield return null;
            }
        }

        #endregion

        #region Build



        private void StartBuild(GameBuildingCtrl previewBuilding, Material buildMat, FarmerCtrl farmer)
        {
            BuildInfo buildInfo = new BuildInfo(previewBuilding, (onComplete) => StartCoroutine(ExecuteBuild(previewBuilding, onComplete)));
            farmer.OnBuild(buildInfo);
        }

        private IEnumerator ExecuteBuild(GameBuildingCtrl previewBuilding, Action onComplete)
        {
            float progress = 0;
            float currentTime = 0;
            while (currentTime < previewBuilding.buildTime)
            {
                progress = currentTime / previewBuilding.buildTime;
                previewBuilding.buildMat.SetFloat(shaderId_BuildProgress, Mathf.Clamp01(1 - progress));
                currentTime += Time.deltaTime;
                previewBuilding.SetBuildProgress(progress);
                yield return null;
            }
            previewBuilding.RemoveBuildMat();
            previewBuilding.BuildComplete();
            onComplete?.Invoke();
        }

        #endregion

        #region Helper

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

        /// <summary>
        /// 当前预览建筑是否和其他单位碰撞
        /// </summary>
        public bool IsCollideWithUnits(GameBuildingCtrl previewBuilding)
        {
            Vector3 buildingSize = previewBuilding.GetBuildingSize() / 2;
            float radius = 1;
            buildingSize.x += radius;//给x,z轴额外增加一些距离
            buildingSize.z += radius;
            Transform tran = previewBuilding.transform;
            Collider[] colliders = Physics.OverlapBox(tran.position, buildingSize, tran.rotation, GameLayerName.GameUnit.GetLayerMask());
            foreach (var item in colliders)
            {
                if (item.transform != tran)//碰撞检测时，会碰撞到自己
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }

}