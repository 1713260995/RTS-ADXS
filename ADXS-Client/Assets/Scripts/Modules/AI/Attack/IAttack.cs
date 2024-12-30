using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.AI.Attack
{
    public interface IAttack
    {
        void Attack(GameUnitCtrl target);
    }


    public class AttackByNormal : IAttack
    {
        public GameRoleCtrl role;

        public float attackDistance = 0.1f;

        public AttackByNormal(GameRoleCtrl role)
        {
            this.role = role;
        }

        public void Attack(GameUnitCtrl target)
        {

        }

        public IEnumerator AutoAttack(GameUnitCtrl target)
        {
            //if (role.transform.position != target)
            //{

            //}


            yield return null;
        }


    }
}
