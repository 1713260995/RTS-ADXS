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

        public void SetBuildMat(Material buildMat, bool isAdd)
        {
            foreach (var renderer in buildRenderers)
            {
                List<Material> materials = renderer.materials.ToList();
                if (isAdd)
                {
                    materials.Add(buildMat);
                }
                else
                {
                    materials.Remove(buildMat);
                }
                renderer.SetMaterials(materials);
            }
        }

        #endregion
    }
}
