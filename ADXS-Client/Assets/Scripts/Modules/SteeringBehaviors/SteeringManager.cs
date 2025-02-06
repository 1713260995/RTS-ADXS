using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameClientLib.Scripts.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Modules.SteeringBehaviors
{
    public interface IBoid
    {
        Transform transform { get; }
        Vector3 Position { get; }
        Vector3 Velocity { get; set; }
        float MaxSpeed { get; }
        float MaxForce { get; }
        float Radius { get; }
        float AvoidanceRadius { get; }

        /// <summary>
        /// 整个转向行为对当前速度影响的权重
        /// </summary>
        float Mass { get; }
    }

    public class SteeringManager
    {
        public IBoid host { get; }

        public SteeringManager(IBoid host)
        {
            this.host = host;
        }

        public void Update(Vector3 steering)
        {
            steering = Vector3.ClampMagnitude(steering, host.MaxForce);
            steering /= host.Mass;
            host.Velocity = Vector3.ClampMagnitude(host.Velocity + steering, host.MaxSpeed);
            var v = host.Velocity * Time.deltaTime;

            host.transform.position += new Vector3(v.x, v.y, v.z);
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

                result.TryGetComponent<Boid>(out var boid);
                obstacles.Add(new Obstacle(boid));
            }

            return obstacles.ToArray();
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
        public Vector3 Arrive(Vector3 targetPos, float slowingRadius)
        {
            Vector3 desired_velocity = targetPos - host.Position;
            var distance = desired_velocity.magnitude;
            if (distance < slowingRadius) {
                desired_velocity = desired_velocity.normalized * (host.MaxSpeed * (distance / slowingRadius));
            }
            else {
                desired_velocity = desired_velocity.normalized * host.MaxSpeed;
            }

            Vector3 steering = desired_velocity - host.Velocity;
            return steering;
        }

        #region Wander

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

        private void SetAngle(ref Vector3 vector, float value)
        {
            float len = vector.magnitude;
            vector.x = Mathf.Cos(value) * len;
            vector.y = Mathf.Sin(value) * len;
        }

        #endregion

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

        #region Avoidance

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
            if (mostThreatening.transform != null) {
                avoidance = ahead - mostThreatening.center;
                avoidance.Normalize();
                avoidance *= maxAvoidForce;
                //Debug.Log(avoidance);
            }

            return avoidance;
        }


        /// <summary>
        /// 查找最近的障碍物,且将会碰撞到的障碍物
        /// </summary>
        private Obstacle FindMostThreateningObstacle(Vector3 ahead, Vector3 ahead2)
        {
            Obstacle[] obstacles = GetNearestObstacles(host.AvoidanceRadius);
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

        #endregion

        #region FollowLeader

        /// <summary>
        /// 跟随领导者行为
        /// </summary>
        /// <param name="leader"></param>
        /// <param name="boids">所有跟随者</param>
        /// <param name="leaderBehindDist">跟随者离领导的距离</param>
        /// <param name="separationRadius">跟随者之间分离的半径</param>
        /// <param name="maxSeparationForce">分离力</param>
        /// <param name="leaderSightRadius">领导者前方的可视范围</param>
        /// <returns></returns>
        public Vector3 FollowLeader(IBoid leader, IBoid[] boids, float leaderBehindDist, float separationRadius, float maxSeparationForce, float leaderSightRadius)
        {
            Vector2 tv = leader.Velocity.GetXZ();
            tv = tv.normalized * leaderBehindDist; //这里使用Vector2是因为如果使用Vector3归一化后再乘以距离会使y轴的值变大而使结果错误
            Vector2 ahead = leader.Position.GetXZ() + tv;
            tv *= -1;
            Vector2 behind = leader.Position.GetXZ() + tv;
            Vector3 force = Vector3.zero;
            //是否在领导者的前方
            bool isOnLeaderSight = (ahead - host.Position.GetXZ()).magnitude <= leaderSightRadius || (leader.Position - host.Position).magnitude <= leaderSightRadius;
            if (isOnLeaderSight) {
                //如果当前角色在领导者的前方视线范围内，给领导者让路
                // force += Evade(leader);
            }

            force += Arrive(behind.XZToXYZ(), 3);
            force += Separation(boids, separationRadius, maxSeparationForce);
            return force;
        }

        /// <summary>
        /// 分离行为，避免拥挤
        /// </summary>
        private Vector3 Separation(IBoid[] boids, float separationRadius, float maxSeparationForce)
        {
            Vector2 force = Vector2.zero;
            int neighborCount = 0;
            for (int i = 0; i < boids.Length; i++) {
                var b = boids[i];
                if (b != host && (b.Position - host.Position).magnitude <= separationRadius) {
                    force += b.Position.GetXZ() - host.Position.GetXZ();
                    neighborCount++;
                }
            }

            if (neighborCount != 0) {
                force /= neighborCount;
                force *= -1;
            }

            force.Normalize();
            force *= maxSeparationForce;
            return force.XZToXYZ();
        }


        public Vector3 Leader(IBoid leader)
        {
            Vector3 force = Vector3.zero;
            var separation = Vector3.zero;
            var alignment = leader.transform.forward;
            var cohesion = leader.transform.position;
            var nearbyBoids = GetNearestObstacles(host.AvoidanceRadius);
            if (nearbyBoids.Length == 0) {
                return force;
            }

            foreach (var boid in nearbyBoids) {
                if (boid.transform == host.transform) continue;
                var t = boid.transform;
                separation += GetSeparationVector(t);
                alignment += t.forward;
                cohesion += t.position;
            }

            var avg = 1.0f / nearbyBoids.Length;
            alignment *= avg;
            cohesion *= avg;
            cohesion = (cohesion - host.Position).normalized;
            force = separation + alignment + cohesion;

            var rotation = Quaternion.FromToRotation(Vector3.forward, force.normalized);

            // Applys the rotation with interpolation.
            if (rotation != host.transform.rotation) {
                var ip = Mathf.Exp(-4 * Time.deltaTime);
                host.transform.rotation = Quaternion.Slerp(rotation, host.transform.rotation, ip);
            }

            return force;
        }

        private Vector3 GetSeparationVector(Transform target)
        {
            var diff = host.transform.position - target.transform.position;
            var diffLen = diff.magnitude;
            var scale = Mathf.Clamp01(1.0f - diffLen / host.AvoidanceRadius);
            return diff * (scale / diffLen);
        }

        #endregion

        #region Queue(排队行为) 狭窄地形有序出入

        /// <summary>
        /// 排队行为（排队行为需要放到所有转向行为的最后）
        /// 排队是排队的过程，排成一排角色，耐心等待到达某个地方。
        /// 当队伍中的第一个人移动时，其余的人也跟着移动，形成一个看起来像火车拉货车的模式。等待时，角色不应离开该行。
        /// </summary>
        /// <returns></returns>
        private Vector3 Queue(Vector3 finalSteering, IBoid[] boids)
        {
            float MAX_QUEUE_RADIUS = 5; //TODO: need set
            Vector3 v = host.Velocity;
            Vector3 brake = Vector3.zero;
            IBoid neighbor = getNeighborAhead();
            if (neighbor != null) {
                brake = -finalSteering * 0.8f;
                v *= -1;
                brake = brake + v + Separation(boids, 5, 1);
                if ((host.Position - neighbor.Position).magnitude <= MAX_QUEUE_RADIUS) {
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
    }
}