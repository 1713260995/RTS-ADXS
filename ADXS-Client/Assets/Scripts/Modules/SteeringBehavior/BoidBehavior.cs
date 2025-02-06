//using System.Collections.Generic;
//using System.Linq;
//using Assets.GameClientLib.Scripts.Utils;
//using Assets.Scripts.Modules.SteeringBehaviors;
//using Test;
//using UnityEngine;

//namespace Modules.SteeringBehaviors
//{
//    public class BoidBehavior
//    {
//        public List<List<IBoid>> SetGroup(IBoid leader, List<IBoid> boids, int groupNum = 3)
//        {
//            Vector3 force = Vector3.zero;
//            boids = boids.OrderBy(o => (o.Position - leader.Position).magnitude).ToList();
//            List<List<IBoid>> neighbors = new() { new List<IBoid>() { leader } };
//            int s = boids.Count / groupNum;
//            if (boids.Count % groupNum != 0)
//            {
//                s += 1;
//            }
//            for (int i = 0; i < s; i++)
//            {
//                IBoid[] group = boids.Skip(i * groupNum).Take(groupNum).ToArray();
//                neighbors.Add(group.ToList());
//            }
//        }


//        public Vector3 BoidBehavior(IBoid leader, List<IBoid> boids, int groupNum = 3)
//        {
//            Vector3 force = Vector3.zero;
//            boids = boids.OrderBy(o => (o.Position - leader.Position).magnitude).ToList();
//            List<List<IBoid>> neighbors = new() { new List<IBoid>() { leader } };
//            int s = boids.Count / groupNum;
//            int add1 = boids.Count % groupNum == 0 ? 0 : 1;
//            s += add1;
//            int index = 0;
//            for (int i = 0; i < s; i++)
//            {
//                IBoid[] group = boids.Skip(i * groupNum).Take(groupNum).ToArray();
//                neighbors.Add(group.ToList());
//                if (group.ToList().Exists(o => o == host))
//                {
//                    index = i + 1;
//                }
//            }

//            boids.Add(leader);
//            List<IBoid> otherBoids = boids.Where(o => o.transform != host.transform).ToList();
//            force += Separation(otherBoids, 5, UnitTest_Steering.boidSeparateDistance);
//            force += Cohesion(neighbors[index - 1], 2, UnitTest_Steering.boidArriveDistance);
//            Vector3 steering = force - host.Velocity;
//            return steering;
//        }

//        /// <summary>
//        /// 聚集——使该Boid靠近上一梯队的鸟群
//        /// </summary>
//        private Vector3 Cohesion(List<IBoid> lastBoids, int num = 3, float arriveDistance = 3)
//        {
//            num = Mathf.Min(lastBoids.Count, num);
//            List<IBoid> nearbyBoids = GetRecentlyBoids(lastBoids, num);
//            Vector3 force = Vector3.zero;
//            foreach (var boid in nearbyBoids)
//            {
//                force = boid.Position - host.Position;
//            }

//            force /= num;
//            if (force.magnitude > arriveDistance)
//            {
//                return force;
//            }

//            return Vector3.zero;
//        }

//        /// <summary>
//        /// 分离——使该Boid与其他Boid保持距离
//        /// </summary>
//        private Vector3 Separation(List<IBoid> otherBoids, int num = 5, float separateDistance = 2, float maxSeparationForce = 3)
//        {
//            var boids = GetRecentlyBoids(otherBoids, num);
//            Vector3 force = Vector3.zero;
//            int neighborCount = 0;
//            for (int i = 0; i < boids.Count; i++)
//            {
//                var b = boids[i];
//                if ((b.Position - host.Position).magnitude <= separateDistance)
//                {
//                    force += b.Position - host.Position;
//                    neighborCount++;
//                }
//            }

//            if (neighborCount == 0)
//            {
//                return Vector3.zero;
//            }
//            force /= neighborCount;
//            Vector2 f = force.GetXZ().normalized * -maxSeparationForce;
//            return f.XZToXYZ();
//        }


//        /// <summary>
//        /// 获取离该Boid最近的Boids,按距离从近到远排序
//        /// </summary>
//        public List<IBoid> GetRecentlyBoids(List<IBoid> otherBoids, int num)
//        {
//            num = Mathf.Min(otherBoids.Count, num);
//            return otherBoids.OrderBy(o => (o.Position - host.Position).magnitude).Skip(0).Take(num).ToList();
//        }
//    }
//}