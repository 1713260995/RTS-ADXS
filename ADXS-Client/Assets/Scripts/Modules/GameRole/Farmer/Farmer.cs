using Assets.Scripts.Modules.Role;
using Assets.Scripts.Modules.Spawn;

namespace Assets.Scripts.Modules
{
    public class Farmer : GameRoleCtrl, ISpwanPool
    {
        public void GetFromPool()
        {
            gameObject.SetActive(false);
        }

        public void ReturnedToPool()
        {
            gameObject.SetActive(true);
        }
    }
}
