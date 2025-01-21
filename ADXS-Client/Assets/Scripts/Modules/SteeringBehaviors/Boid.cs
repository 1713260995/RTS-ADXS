using UnityEngine;

namespace Modules.SteeringBehaviors
{
    public class Boid : MonoBehaviour, IBoid
    {
        public Vector3 Position => transform.position;
        public Vector3 Velocity { get; set; }
        public float MaxSpeed => maxSpeed;
        public float MaxForce => maxForce;
        public float Mass => mass;
        [Header("最大速度")]
        public float maxSpeed;
        [Header("steering force最大值")]
        public float maxForce;
        [Header("steering force整体缩放权重")]
        public float mass = 1;
        [Header("arrive行为 减速半径")]
        public float arriveSlowingRadius = 4;
        public SteeringManager steeringManager { get;private set; }

        private void Awake()
        {
            steeringManager = new SteeringManager(this);
        }

        
        
    }
}