using Assets.Scripts.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Role
{
    public class GameBuildingCtrl : GameUnitCtrl
    {
        private new Collider collider;
        [SerializeField] private List<Renderer> buildRenderers;

        public float buildTime = 5;
        public bool isBuildComplete { get; private set; }
        public bool isInitComplete { get; private set; }

        protected override void Start()
        {
            base.Start();
            collider = GetComponent<Collider>();
            isBuildComplete = false;
            isInitComplete = true;
        }



        #region Build

        public Vector3 GetBuildingSize()
        {
            return collider.bounds.size;
        }


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

        #endregion
    }
}
