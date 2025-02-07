using Assets.Scripts.Modules.Spawn;

namespace Assets.Scripts.Modules
{
    /// <summary>
    /// 需要使用对象池的角色
    /// </summary>
    public class RoleByPoolCtrl : GameRoleCtrl, ISpawnPool
    {
        public virtual void GetFromPool()
        {
            gameObject.SetActive(true);
        }

        public virtual void ReturnedToPool()
        {
            gameObject.SetActive(false);
        }
    }
}
