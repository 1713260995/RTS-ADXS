using System.Collections.Generic;
using System.Linq;
using Assets.GameClientLib.Scripts.Utils;
using Test;
using UnityEngine;

namespace Assets.Scripts.Modules.SteeringBehaviors
{
    public class Boid : MonoBehaviour, IBoid
    {
        [Header("最大速度")]
        public float maxSpeed = 5;

        [Header("steering force最大值")]
        public float maxForce = 6;

        [Header("steering force整体缩放权重")]
        public float mass = 1;

        [Header("CollisionAvoidance行为 boid的自身半径")]
        public float radius = 2;

        [Header("CollisionAvoidance行为 检测半径 查看周围多远的障碍物")]
        public float avoidanceRadius = 5;

        [Header("arrive行为 减速半径")]
        public float arriveSlowingRadius = 2;

        public Vector3 Position => transform.position;
        public Vector3 Velocity { get; set; }
        public float Mass => mass;
        public float MaxSpeed => maxSpeed;
        public float MaxForce => maxForce;
        public float Radius => radius;
        public float AvoidanceRadius => avoidanceRadius;
        public SteeringBehavior SteeringBehavior => steeringManager;
        private SteeringBehavior steeringManager { get; set; }
        public List<List<IBoid>> boidsGroup { get; set; }
        public int groupIndex { get; set; }

        private void Awake()
        {
            steeringManager = new SteeringBehavior(this);
        }


        #region BoidBehavior 鸟群行为

        public static void SetGroup(Boid leader, List<IBoid> boids, int groupNum = 4)
        {
            boids = boids.OrderBy(o => (o.Position - leader.Position).magnitude).ToList();
            List<List<IBoid>> boidsGroup = new() { new List<IBoid>() { leader } };
            leader.boidsGroup = boidsGroup;
            leader.groupIndex = 0;
            int s = boids.Count / groupNum + (boids.Count % groupNum == 0 ? 0 : 1);
            for (int i = 0; i < s; i++) {
                IBoid[] group = boids.Skip(i * groupNum).Take(groupNum).ToArray();
                boidsGroup.Add(group.ToList());
                foreach (IBoid item in group) {
                    Boid b = (Boid)item;
                    b.boidsGroup = boidsGroup;
                    b.groupIndex = i + 1;
                }
            }
        }


        public Vector3 BoidBehavior(Boid leader, List<IBoid> boids, int groupNum = 4, float arriveDistance = 2, float SeparateDistance = 1.8f)
        {
            Boid host = this;
            Vector3 force = Vector3.zero;
            boids = boids.Concat(host.boidsGroup!.First()).ToList();
            List<IBoid> otherBoids = boids.Where(o => o.transform != host.transform).ToList();
            force += steeringManager.Separation(otherBoids, 5, SeparateDistance);
            force += Cohesion(host.boidsGroup[groupIndex - 1], 2, arriveDistance);
            Vector3 steering = force - host.Velocity;
            return steering;
        }

        /// <summary>
        /// 聚集——使该Boid靠近上一梯队的鸟群
        /// </summary>
        private Vector3 Cohesion(List<IBoid> lastBoids, int num = 3, float arriveDistance = 3)
        {
            Boid host = this;
            num = Mathf.Min(lastBoids.Count, num);
            List<IBoid> nearbyBoids = steeringManager.GetRecentlyBoids(lastBoids, num);
            Vector3 force = Vector3.zero;
            foreach (var boid in nearbyBoids) {
                force = boid.Position - host.Position;
            }

            force /= num;
            if (force.magnitude < arriveDistance) {
                force = Vector3.zero;
            }

            return force;
        }

        #endregion
    }
}