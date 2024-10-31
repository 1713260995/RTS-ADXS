using Assets.Scripts.Factory;
using UnityEngine;

namespace Assets.Scripts.Modules.Farmer
{
    [CreateAssetMenu(fileName = "FarmerFactory", menuName = "ScriptableObject/Factory/Farmer/Human Farmer")]
    public class FarmerFactory : RoleBaseFactorySO<Farmer>
    {
        public override Farmer Create()
        {
            throw new System.NotImplementedException();
        }
    }
}
