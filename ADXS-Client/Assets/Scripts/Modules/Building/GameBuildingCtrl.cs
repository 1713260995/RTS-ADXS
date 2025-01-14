using UnityEngine;

namespace Assets.Scripts.Modules.Role
{
    public class GameBuildingCtrl : GameUnitCtrl
    {
        public new Collider collider;

        protected override void Awake()
        {
            base.Awake();
            collider = GetComponent<Collider>();
        }



        public void GetBuildingSize(out float lenX, out float lenY, out float lenZ)
        {
            Vector3 size = collider.bounds.size;
            lenX = size.x;
            lenY = size.y;
            lenZ = size.z;
        }

    }
}
