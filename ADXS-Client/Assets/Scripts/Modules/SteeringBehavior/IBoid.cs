using UnityEngine;

namespace Assets.Scripts.Modules.SteeringBehaviors
{
    public interface IBoid
    {
        Transform transform { get; }
        Vector3 Position { get; }
        Vector3 Velocity { get; set; }
        float MaxSpeed { get; }
        float MaxForce { get; }
        float Radius { get; }

        /// <summary>
        /// 整个转向行为对当前速度影响的权重
        /// </summary>
        float Mass { get; }

        SteeringBehavior SteeringBehavior { get; }
    }
}
