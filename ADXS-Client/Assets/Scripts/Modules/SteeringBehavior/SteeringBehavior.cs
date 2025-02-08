using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameClientLib.Scripts.Utils;
using Test;
using Unity.VisualScripting;
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

        public void Update(Vector2 steering)
        {
            steering = Vector2.ClampMagnitude(steering, host.MaxForce);
            steering /= host.Mass;
            host.Velocity = Vector2.ClampMagnitude(host.Velocity + steering, host.MaxSpeed);
            // return host.Velocity;
        }

        #region Steering 一般的转向行为

        /// <summary>
        /// 在每帧的速度上增加转向力将使角色平稳地放弃其旧的直线路线并朝着目标前进
        /// </summary>
        private Vector2 Seek(Vector2 target)
        {
            Vector2 desiredVelocity = (target - host.Position).normalized * host.MaxSpeed;
            Vector2 steering = desiredVelocity - host.Velocity;
            return steering;
        }

        /// <summary>
        /// 逃逸行为
        /// 新向量的计算方法是从目标的位置中减去角色的位置，从而生成从目标到角色的向量
        /// </summary>
        public Vector2 Flee(Vector2 target)
        {
            Vector2 desired_velocity = (host.Position - target).normalized * host.MaxSpeed;
            return desired_velocity - host.Velocity;
        }

        /// <summary>
        /// 到达行为
        /// 当角色进入减速区域时，其速度将线性下降到零。
        /// </summary>
        public Vector2 Arrive(Vector2 targetPos, float slowingRadius = 1.5f)
        {
            Vector2 desired_velocity = targetPos - host.Position;
            var distance = desired_velocity.magnitude;
            if (distance < slowingRadius) {
                desired_velocity = desired_velocity.normalized * (host.MaxSpeed * (distance / slowingRadius));
            }
            else {
                desired_velocity = desired_velocity.normalized * host.MaxSpeed;
            }

            Vector2 steering = desired_velocity - host.Velocity;
            return steering;
        }

        /// <summary>
        /// 游荡行为
        /// 模拟游戏的角色在环境中随机移动
        /// </summary>
        public Vector2 Wander(float circleDistance, float circleRadius, float wanderAngle, float angleChange)
        {
            Vector2 circleCenter = host.Velocity.normalized * circleDistance;
            Vector2 displacement = new Vector2(0, -1) * circleRadius;
            SetAngle(ref displacement, wanderAngle);
            wanderAngle += Random.Range(0f, angleChange) - angleChange * 0.5f;
            Vector2 wanderForce = circleCenter + displacement;
            return wanderForce;
        }

        /// <summary>
        /// 追逐行为
        /// 追逐行为的工作方式与Seek的方式几乎相同，唯一的区别是追逐者不会搜索目标本身，而是搜索目标在不久的将来的位置。
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public Vector2 Pursuit(IBoid boid)
        {
            Vector2 distance = boid.Position - host.Position;
            float t = distance.magnitude / host.MaxSpeed;
            Vector2 futurePosition = boid.Position + boid.Velocity * t;
            return Seek(futurePosition);
        }

        /// <summary>
        /// 逃避行为
        /// 逃避行为与追逐行为相反。在闪避行为中，角色不会寻找目标的未来位置，而是逃离该位置
        /// </summary>
        public Vector2 Evade(IBoid t)
        {
            Vector2 distance = t.Position - host.Position;
            float updatesAhead = distance.magnitude / host.MaxSpeed;
            Vector2 futurePosition = t.Position + t.Velocity * updatesAhead;
            return Flee(futurePosition);
        }

        #endregion

        #region Avoidance 避障行为

        /// <summary>
        /// 避障行为
        /// </summary>
        public Vector2 CollisionAvoidance(float maxAvoidForce = 10)
        {
            float dynamic_length = host.Velocity.magnitude / host.MaxSpeed; //变量的范围是 0 到 1。当角色全速移动时，为 1;当角色减速或加速时，为 0 或更大（例如 0.5）。
            Vector2 ahead = host.Position + host.Velocity.normalized * dynamic_length;
            Vector2 ahead2 = host.Position + host.Velocity.normalized * (dynamic_length * 0.5f);
            Obstacle mostThreatening = FindMostThreateningObstacle(ahead, ahead2);
            Vector2 avoidance = Vector2.zero;
            if (mostThreatening.transform != null) {
                avoidance = ahead - mostThreatening.center;
                avoidance.Normalize();
                avoidance *= maxAvoidForce;
            }

            return avoidance;
        }

        /// <summary>
        /// 查找最近的障碍物,且将会碰撞到的障碍物
        /// </summary>
        private Obstacle FindMostThreateningObstacle(Vector2 ahead, Vector2 ahead2, float avoidanceRadius = 5)
        {
            Obstacle[] obstacles = GetNearestObstacles(avoidanceRadius);
            Obstacle mostThreatening = new Obstacle();
            for (int i = 0; i < obstacles.Length; i++) {
                Obstacle obstacle = obstacles[i];
                bool isCollision = (obstacle.center - ahead).magnitude <= obstacle.radius || (obstacle.center - ahead2).magnitude <= obstacle.radius;
                if (isCollision && (mostThreatening.transform == null || (host.Position - obstacle.center).magnitude < (host.Position - mostThreatening.center).magnitude)) {
                    mostThreatening = obstacle;
                }
            }

            return mostThreatening;
        }


        /// <summary>
        /// 分离——使该Boid与其他Boid保持距离
        /// </summary>
        public Vector2 Separation(List<IBoid> otherBoids, out float weight, float separateDistance = 1.5f, float maxSeparationForce = 3)
        {
            var boids = GetRecentlyBoids(otherBoids, 3, o => (o.Position - host.Position).magnitude <= separateDistance);
            Vector2 force = Vector2.zero;
            float allForce = boids.Sum(o => (o.Position - host.Position).magnitude);
            int neighborCount = 0;
            for (int i = 0; i < boids.Count; i++) {
                var item = boids[i];
                force += (item.Position - host.Position);
                neighborCount++;
            }

            weight = 0;
            if (neighborCount == 0) {
                return -force;
            }

            weight = 1 / Mathf.Max(allForce , 3f);
            // return -force / neighborCount;
            Vector2 f = (force / neighborCount).normalized * -maxSeparationForce;
            return f;
        }

        #endregion

        #region BoidBehavior 鸟群行为

        public struct BoidBehaviorInfo
        {
            /// <summary>
            /// 鸟群中除了leader外的所有boid
            /// </summary>
            public List<IBoid> boids;

            public Vector2 destination;
            public float separateDistance;
            public List<Vector2> groupPositions;
            public int groupNum;

            public BoidBehaviorInfo(List<IBoid> boids, Vector2 destination) : this()
            {
                this.boids = boids;
                this.destination = destination;
                this.separateDistance = 1.8f;
                this.groupNum = GetGroupNum();
                this.groupPositions = CreateGroup();
            }

            public List<Vector2> CreateGroup()
            {
                return MyMath.CreateMatrixPoints(destination.XZToXYZ(), boids.Count, groupNum, 3, 3).Select(o => o.GetXZ()).ToList();
            }

            private int GetGroupNum()
            {
                groupNum = boids.Count switch {
                    > 8 => 4,
                    > 4 => 3,
                    _ => 2
                };

                return groupNum;
            }
        }

        public Vector2 BoidBehavior(BoidBehaviorInfo info)
        {
            Vector2 force = Vector2.zero;
            Vector2 arriveForce = ArriveOnBoid(info);
            Vector2 separationForce = Separation(info.boids, out float weight);
            separationForce = separationForce * (1 + weight);
            arriveForce = arriveForce * (1 - weight);
            force = separationForce + arriveForce;
            return force - host.Velocity;
        }

        private Vector2 ArriveOnBoid(BoidBehaviorInfo info)
        {
            Vector2 force = Vector2.zero;
            int index = info.boids.IndexOf(host);
            Vector2 targetPos = info.groupPositions[index];
            if ((host.Position - targetPos).magnitude > 1f) {
                return Arrive(targetPos) + host.Velocity;
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
        private Vector2 Queue(Vector2 finalSteering, List<IBoid> boids)
        {
            // float MAX_QUEUE_RADIUS = 5; //TODO: need set
            // Vector2 v = host.Velocity;
            // Vector2 brake = Vector2.zero;
            // IBoid neighbor = getNeighborAhead();
            // if (neighbor != null) {
            //     brake = -finalSteering * 0.8f;
            //     v *= -1;
            //     brake = brake + v + Separation(boids, 5, 1);
            //     if ((host.Position - neighbor.Position).magnitude <= MAX_QUEUE_RADIUS) {
            //         host.Velocity *= 0.3f;
            //     }
            // }
            //
            // return brake;
            return Vector2.zero;
        }

        private IBoid getNeighborAhead()
        {
            float MAX_QUEUE_AHEAD = 10; //TODO: need set
            IBoid[] boids = { }; //TODO: get all boids
            float MAX_QUEUE_RADIUS = 5; //TODO: need set
            IBoid ret = null;
            Vector2 qa = host.Velocity;
            qa.Normalize();
            qa *= MAX_QUEUE_AHEAD;
            Vector2 ahead = host.Position + qa;

            for (int i = 0; i < boids.Length; i++) {
                IBoid neighbor = boids[i];
                float d = (ahead - neighbor.Position).magnitude;
                if (neighbor != host && d <= MAX_QUEUE_RADIUS) {
                    ret = neighbor;
                    break;
                }
            }

            return ret;
        }

        #endregion

        #region Helper

        private void SetAngle(ref Vector2 vector, float value)
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
            for (int i = 0; i < results.Length; i++) {
                var result = results[i];
                if (result == null || result.transform == host.transform) {
                    continue;
                }

                if (result.TryGetComponent<GameRoleCtrl>(out var role)) {
                    obstacles.Add(new Obstacle(role.boid));
                }

                ;
            }

            return obstacles.ToArray();
        }


        /// <summary>
        /// 获取离该Boid最近的Boids,按距离从近到远排序
        /// </summary>
        public List<IBoid> GetRecentlyBoids(List<IBoid> boids, int num, Func<IBoid, bool> filter = null)
        {
            num = Mathf.Min(boids.Count, num);
            var list = boids.Where(o => {
                if (filter != null) {
                    return o != host && filter(o);
                }

                return o != host;
            }).OrderBy(o => (o.Position - host.Position).magnitude).Skip(0).Take(num).ToList();
            return list;
        }

        public struct Obstacle
        {
            public Transform transform;
            public float radius;
            public Vector2 center => transform.position.GetXZ();

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