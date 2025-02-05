using Assets.GameClientLib.Scripts.Utils;
using UnityEngine;

namespace Modules.SteeringBehaviors
{
    public class Boid : MonoBehaviour, IBoid
    {
        [Header("最大速度")]
        public float maxSpeed;

        [Header("steering force最大值")]
        public float maxForce;

        [Header("steering force整体缩放权重")]
        public float mass = 1;

        [Header("CollisionAvoidance行为 boid的自身半径")]
        public float radius = 2;

        [Header("CollisionAvoidance行为 检测半径 查看周围多远的障碍物")]
        public float avoidanceRadius = 5;

        [Header("arrive行为 减速半径")]
        public float arriveSlowingRadius = 4;

        public Vector3 Position => transform.position;
        public Vector3 Velocity { get; set; }
        public float Mass => mass;
        public float MaxSpeed => maxSpeed;
        public float MaxForce => maxForce;
        public float Radius => radius;
        public float AvoidanceRadius => avoidanceRadius;

        public SteeringManager steeringManager { get; private set; }

        private void Awake()
        {
            steeringManager = new SteeringManager(this);
        }
    }
}