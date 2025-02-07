using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameClientLib.Scripts.Utils;
using Test;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Modules.SteeringBehaviors
{
    public class SteeringBehavior
    {
        public IBoid host { get; }

        public SteeringBehavior(IBoid host)
        {
            this.host = host;
        }

        public Vector3 Update(Vector3 steering)
        {
            steering = Vector3.ClampMagnitude(steering, host.MaxForce);
            steering /= host.Mass;
            host.Velocity = Vector3.ClampMagnitude(host.Velocity + steering, host.MaxSpeed);
            //var v = host.Velocity * Time.deltaTime;
            // host.transform.position += new Vector3(v.x, v.y, v.z);
            return host.Velocity;
        }

        #region Steering 一般的转向行为

        /// <summary>
        /// 在每帧的速度上增加转向力将使角色平稳地放弃其旧的直线路线并朝着目标前进
        /// </summary>
        private Vector3 Seek(Vector3 target)
        {
            Vector3 desiredVelocity = (target - host.Position).normalized * host.MaxSpeed;
            Vector3 steering = desiredVelocity - host.Velocity;
            return steering;
        }

        /// <summary>
        /// 逃逸行为
        /// 新向量的计算方法是从目标的位置中减去角色的位置，从而生成从目标到角色的向量
        /// </summary>
        /// <param name="target"></param>
        public Vector3 Flee(Vector3 target)
        {
            Vector3 desired_velocity = (host.Position - target).normalized * host.MaxSpeed;
            return desired_velocity - host.Velocity;
        }

        /// <summary>
        /// 到达行为
        /// 当角色进入减速区域时，其速度将线性下降到零。
        /// </summary>
        public Vector3 Arrive(Vector3 targetPos, float slowingRadius = 1.5f)
        {
            Vector3 desired_velocity = targetPos - host.Position;
            var distance = desired_velocity.magnitude;
            if (distance < slowingRadius)
            {
                desired_velocity = desired_velocity.normalized * (host.MaxSpeed * (distance / slowingRadius));
            }
            else
            {
                desired_velocity = desired_velocity.normalized * host.MaxSpeed;
            }

            Vector3 steering = desired_velocity - host.Velocity;
            return steering;
        }

        /// <summary>
        /// 游荡行为
        /// 模拟游戏的角色在环境中随机移动
        /// </summary>
        public Vector3 Wander(float circleDistance, float circleRadius, float wanderAngle, float angleChange)
        {
            Vector3 circleCenter = host.Velocity.normalized * circleDistance;
            Vector3 displacement = new Vector3(0, 0, -1) * circleRadius;
            SetAngle(ref displacement, wanderAngle);
            wanderAngle += Random.Range(0f, angleChange) - angleChange * 0.5f;
            Vector3 wanderForce = circleCenter + displacement;
            return wanderForce;
        }

        /// <summary>
        /// 追逐行为
        /// 追逐行为的工作方式与Seek的方式几乎相同，唯一的区别是追逐者不会搜索目标本身，而是搜索目标在不久的将来的位置。
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public Vector3 Pursuit(IBoid boid)
        {
            Vector3 distance = boid.Position - host.Position;
            float t = distance.magnitude / host.MaxSpeed;
            Vector3 futurePosition = boid.Position + boid.Velocity * t;
            return Seek(futurePosition);
        }

        /// <summary>
        /// 逃避行为
        /// 逃避行为与追逐行为相反。在闪避行为中，角色不会寻找目标的未来位置，而是逃离该位置
        /// </summary>
        public Vector3 Evade(IBoid t)
        {
            Vector3 distance = t.Position - host.Position;
            float updatesAhead = distance.magnitude / host.MaxSpeed;
            Vector3 futurePosition = t.Position + t.Velocity * updatesAhead;
            return Flee(futurePosition);
        }

        #endregion

        #region Avoidance 避障行为

        /// <summary>
        /// 避障行为
        /// </summary>
        public Vector3 CollisionAvoidance(float maxAvoidForce = 10)
        {
            float dynamic_length = host.Velocity.magnitude / host.MaxSpeed; //变量的范围是 0 到 1。当角色全速移动时，为 1;当角色减速或加速时，为 0 或更大（例如 0.5）。
            Vector3 ahead = host.Position + host.Velocity.normalized * dynamic_length;
            Vector3 ahead2 = host.Position + host.Velocity.normalized * (dynamic_length * 0.5f);
            Obstacle mostThreatening = FindMostThreateningObstacle(ahead, ahead2);
            Vector3 avoidance = Vector3.zero;
            if (mostThreatening.transform != null)
            {
                avoidance = ahead - mostThreatening.center;
                avoidance.Normalize();
                avoidance *= maxAvoidForce;
            }

            return avoidance;
        }

        /// <summary>
        /// 查找最近的障碍物,且将会碰撞到的障碍物
        /// </summary>
        private Obstacle FindMostThreateningObstacle(Vector3 ahead, Vector3 ahead2, float avoidanceRadius = 5)
        {
            Obstacle[] obstacles = GetNearestObstacles(avoidanceRadius);
            Obstacle mostThreatening = new Obstacle();
            for (int i = 0; i < obstacles.Length; i++)
            {
                Obstacle obstacle = obstacles[i];
                bool isCollision = (obstacle.center - ahead).magnitude <= obstacle.radius || (obstacle.center - ahead2).magnitude <= obstacle.radius;
                if (isCollision && (mostThreatening.transform == null || (host.Position - obstacle.center).magnitude < (host.Position - mostThreatening.center).magnitude))
                {
                    mostThreatening = obstacle;
                }
            }

            return mostThreatening;
        }


        /// <summary>
        /// 分离——使该Boid与其他Boid保持距离
        /// </summary>
        public Vector3 Separation(List<IBoid> otherBoids, int num = 5, float separateDistance = 2, float maxSeparationForce = 3)
        {
            var boids = GetRecentlyBoids(otherBoids, num);
            Vector3 force = Vector3.zero;
            int neighborCount = 0;
            for (int i = 0; i < boids.Count; i++)
            {
                var b = boids[i];
                if ((b.Position - host.Position).magnitude <= separateDistance)
                {
                    force += b.Position - host.Position;
                    neighborCount++;
                }
            }

            if (neighborCount == 0)
            {
                return Vector3.zero;
            }

            force /= neighborCount;
            Vector2 f = force.GetXZ().normalized * -maxSeparationForce;
            return f.XZToXYZ();
        }



        #endregion

        #region BoidBehavior 鸟群行为

        public Vector3 BoidBehavior(IBoid leader, List<IBoid> boids, int groupNum = 4, float arriveDistance = 2, float SeparateDistance = 1.8f)
        {
            Vector3 force = Vector3.zero;
            int lastIndex = (boids.IndexOf(host) / groupNum) - 1;
            List<IBoid> lastGroup = null;
            if (lastIndex == -1)
            {
                lastGroup = new List<IBoid> { leader };
            }
            else
            {
                lastGroup = boids.Skip(lastIndex * groupNum).Take(groupNum).ToList();
            }
            List<IBoid> otherBoids = boids.Where(o => o.transform != host.transform).ToList();
            force += host.SteeringBehavior.Separation(otherBoids, 5, SeparateDistance);
            force += Follow(lastGroup, 2, arriveDistance);
            Vector3 steering = force - host.Velocity;
            return steering;
        }

        /// <summary>
        /// 跟随——使该Boid靠近上一梯队的鸟群
        /// </summary>
        private Vector3 Follow(List<IBoid> lastBoids, int num = 3, float arriveDistance = 3)
        {
            num = Mathf.Min(lastBoids.Count, num);
            List<IBoid> nearbyBoids = host.SteeringBehavior.GetRecentlyBoids(lastBoids, num);
            Vector3 force = Vector3.zero;
            foreach (var boid in nearbyBoids)
            {
                force = boid.Position - host.Position;
            }

            force /= num;
            if (force.magnitude < arriveDistance)
            {
                force = Vector3.zero;
            }

            return force;
        }

        #endregion


        #region Queue(排队行为) 狭窄地形有序出入-未测试

        /// <summary>
        /// 排队行为（排队行为需要放到所有转向行为的最后）
        /// 排队是排队的过程，排成一排角色，耐心等待到达某个地方。
        /// 当队伍中的第一个人移动时，其余的人也跟着移动，形成一个看起来像火车拉货车的模式。等待时，角色不应离开该行。
        /// </summary>
        /// <returns></returns>
        private Vector3 Queue(Vector3 finalSteering, List<IBoid> boids)
        {
            float MAX_QUEUE_RADIUS = 5; //TODO: need set
            Vector3 v = host.Velocity;
            Vector3 brake = Vector3.zero;
            IBoid neighbor = getNeighborAhead();
            if (neighbor != null)
            {
                brake = -finalSteering * 0.8f;
                v *= -1;
                brake = brake + v + Separation(boids, 5, 1);
                if ((host.Position - neighbor.Position).magnitude <= MAX_QUEUE_RADIUS)
                {
                    host.Velocity *= 0.3f;
                }
            }

            return brake;
        }

        private IBoid getNeighborAhead()
        {
            float MAX_QUEUE_AHEAD = 10; //TODO: need set
            IBoid[] boids = { }; //TODO: get all boids
            float MAX_QUEUE_RADIUS = 5; //TODO: need set
            IBoid ret = null;
            Vector3 qa = host.Velocity;
            qa.Normalize();
            qa *= MAX_QUEUE_AHEAD;
            Vector3 ahead = host.Position + qa;

            for (int i = 0; i < boids.Length; i++)
            {
                IBoid neighbor = boids[i];
                float d = (ahead - neighbor.Position).magnitude;
                if (neighbor != host && d <= MAX_QUEUE_RADIUS)
                {
                    ret = neighbor;
                    break;
                }
            }

            return ret;
        }

        #endregion

        #region Helper

        private void SetAngle(ref Vector3 vector, float value)
        {
            float len = vector.magnitude;
            vector.x = Mathf.Cos(value) * len;
            vector.y = Mathf.Sin(value) * len;
        }


        public Obstacle[] GetNearestObstacles(float radius)
        {
            Collider[] results = new Collider[10];
            Physics.OverlapSphereNonAlloc(host.Position, radius, results, GameLayerName.GameUnit.GetLayerMask());
            List<Obstacle> obstacles = new();
            for (int i = 0; i < results.Length; i++)
            {
                var result = results[i];
                if (result == null || result.transform == host.transform)
                {
                    continue;
                }

                if (result.TryGetComponent<GameRoleCtrl>(out var role))
                {
                    obstacles.Add(new Obstacle(role.boid));
                };
            }

            return obstacles.ToArray();
        }


        /// <summary>
        /// 获取离该Boid最近的Boids,按距离从近到远排序
        /// </summary>
        public List<IBoid> GetRecentlyBoids(List<IBoid> otherBoids, int num)
        {
            num = Mathf.Min(otherBoids.Count, num);
            return otherBoids.OrderBy(o => (o.Position - host.Position).magnitude).Skip(0).Take(num).ToList();
        }

        public struct Obstacle
        {
            public Transform transform;
            public float radius;
            public Vector3 center => transform.position;

            public Obstacle(Transform transform, float radius)
            {
                this.transform = transform;
                this.radius = radius;
            }

            public Obstacle(IBoid boid)
            {
                transform = boid.transform;
                radius = boid.Radius;
            }
        }

        #endregion
    }
}