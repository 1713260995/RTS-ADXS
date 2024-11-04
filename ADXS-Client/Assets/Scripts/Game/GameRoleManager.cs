using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.GameClientLib.Scripts.Utils.Singleton;
using Assets.Scripts.Factory;
using Assets.Scripts.Modules.Role;

namespace Assets.Scripts.Modules.Builder
{
    public class GameRoleManager : SingletonMono<GameRoleManager>
    {
        private RoleBaseFactorySO roleBaseFactorySO;

        public IFactory<RoleBase> roleFactory { get; private set; }




    }
}
