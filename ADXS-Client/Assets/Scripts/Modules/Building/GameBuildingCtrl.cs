using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Assets.Scripts.Modules.Role
{
    public class GameBuildingCtrl : GameUnitCtrl
    {
        private Collider m_collider;
        public bool isInitComplete { get; private set; }

        protected override void Start()
        {
            base.Start();
            m_collider = GetComponent<Collider>();
            isInitComplete = true;
        }



        #region Build

        public float buildTime = 5;
        [SerializeField] private List<Renderer> buildRenderers;

        public bool isBuildComplete { get; private set; }

        public void BuildComplete()
        {
            isBuildComplete = true;
        }

        public Material buildMat { get; private set; }

        public void AddBuildMat(Material _buildMat)
        {
            if (buildMat != null) throw new System.Exception("buildMat is exist");
            foreach (var renderer in buildRenderers)
            {
                List<Material> materials = renderer.materials.ToList();
                materials.Add(_buildMat);
                buildMat = _buildMat;
                renderer.SetMaterials(materials);
            }
        }

        public void RemoveBuildMat()
        {
            if (buildMat == null) throw new System.Exception("buildMat is null");
            foreach (var renderer in buildRenderers)
            {
                List<Material> materials = renderer.materials.ToList();
                materials.Remove(buildMat);
                buildMat = null;
                Destroy(buildMat);
                renderer.SetMaterials(materials);
            }
        }


        [SerializeField] private AssetReference buildProgressBarPrefab;
        public float buildProgressBarPosY;

        private BuildProgressBar buildProgressBar { get; set; }
        public float buildProgress { get; private set; }

        public void SetBuildProgress(float progress)
        {
            if (buildProgressBar == null)
            {
                buildProgressBar = ResSystem.Instantiate<GameObject>(buildProgressBarPrefab).GetComponent<BuildProgressBar>();
                buildProgressBar.transform.SetParent(transform);

            }
            buildProgressBar.SetBuildProgress(progress);
            buildProgressBar.transform.localPosition = new Vector3(0, GetBuildingSize().y + buildProgressBarPosY, 0);
        }
        #endregion

        #region Helper

        public Vector3 GetBuildingSize()
        {
            return m_collider.bounds.size;
        }

        #endregion
    }
}
