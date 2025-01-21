using UnityEngine;
using UnityEngine.UIElements;

namespace Modules.SteeringBehaviors
{
    public interface IBoid
    {
        Transform transform { get; }
        Vector3 Position { get; }
        Vector3 Velocity { get; set; }
        float MaxSpeed { get; }
        float MaxForce { get; }
        float Mass { get; }
    }

    public class SteeringManager
    {
        public IBoid host { get; }

        public SteeringManager(IBoid host)
        {
            this.host = host;
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
                desired_velocity = desired_velocity.normalized * host.MaxSpeed * (distance / slowingRadius);
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
        public Vector3 CollisionAvoidance(Obstacle[] obstacles, float MAX_AVOID_FORCE = 10)
        {
            float dynamic_length = host.Velocity.magnitude / host.MaxSpeed; //变量的范围是 0 到 1。当角色全速移动时，为 1;当角色减速或加速时，为 0 或更大（例如 0.5）。
            Vector3 ahead = host.Position + host.Velocity.normalized * dynamic_length;
            Vector3 ahead2 = host.Position + host.Velocity.normalized * dynamic_length * 0.5f;
            Obstacle mostThreatening = FindMostThreateningObstacle(ahead, ahead2, obstacles);
            Vector3 avoidance = Vector3.zero;
            if (mostThreatening != null) {
                avoidance = ahead - mostThreatening.center;
                avoidance.Normalize();
                avoidance *= MAX_AVOID_FORCE;
            }
            else {
                avoidance *= 0; // nullify the avoidance force 
            }

            return avoidance;
        }

        /// <summary>
        /// 查找最近的障碍物
        /// </summary>
        private Obstacle FindMostThreateningObstacle(Vector3 ahead, Vector3 ahead2, Obstacle[] obstacles)
        {
            Obstacle mostThreatening = null;
            for (int i = 0; i < obstacles.Length; i++) {
                Obstacle obstacle = obstacles[i];
                bool isCollision = (obstacle.center - ahead).magnitude <= obstacle.radius || (obstacle.center - ahead2).magnitude <= obstacle.radius;
                // "position" is the character's current position 
                if (isCollision && (mostThreatening == null || (host.Position - obstacle.center).magnitude < (host.Position - mostThreatening.center).magnitude)) {
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
        public Vector3 FollowLeader(IBoid leader, float LEADER_BEHIND_DIST, IBoid[] boids, float SEPARATION_RADIUS, float MAX_SEPARATION)
        {
            var tv = leader.Velocity;
            Vector3 force = Vector3.zero;
            tv.Normalize();
            tv *= LEADER_BEHIND_DIST;
            var ahead = leader.Position + tv;
            tv *= -1;
            var behind = leader.Position + tv;
            if (isOnLeaderSight(leader, ahead, leaderSightRadius: 50)) {
                force += Evade(leader);
            }

            force += Arrive(behind, 50);
            force += Separation(boids, SEPARATION_RADIUS, MAX_SEPARATION);
            return force;
        }

        /// <summary>
        /// 避免拥挤
        /// </summary>
        private Vector3 Separation(IBoid[] boids, float SEPARATION_RADIUS, float MAX_SEPARATION)
        {
            Vector3 force = Vector3.zero;
            int neighborCount = 0;
            for (int i = 0; i < boids.Length; i++) {
                var b = boids[i];
                if (b != host && (b.Position - host.Position).magnitude <= SEPARATION_RADIUS) {
                    force.x += b.Position.x - host.Position.x;
                    force.y += b.Position.y - host.Position.y;
                    neighborCount++;
                }
            }

            if (neighborCount != 0) {
                force.x /= neighborCount;
                force.y /= neighborCount;
                force *= -1;
            }

            force.Normalize();
            force *= MAX_SEPARATION;
            return force;
        }

        private bool isOnLeaderSight(IBoid leader, Vector3 ahead, float leaderSightRadius)
        {
            return (ahead - host.Position).magnitude <= leaderSightRadius || (leader.Position - host.Position).magnitude <= leaderSightRadius;
        }

        #endregion

        #region Queue(排队行为) 狭窄地形有序出入

        /// <summary>
        /// 排队行为（排队行为需要放到所有转向行为的最后）
        /// 排队是排队的过程，排成一排角色，耐心等待到达某个地方。
        /// 当队伍中的第一个人移动时，其余的人也跟着移动，形成一个看起来像火车拉货车的模式。等待时，角色不应离开该行。
        /// </summary>
        /// <returns></returns>
        private Vector3 Queue(Vector3 finalSteering,IBoid[] boids)
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

        public void Update(Vector3 steering)
        {
            steering = Vector3.ClampMagnitude(steering, host.MaxForce);
            steering /= host.Mass;
            host.Velocity = Vector3.ClampMagnitude(host.Velocity + steering, host.MaxSpeed);

            host.transform.position += host.Velocity * Time.deltaTime;
            if (host.transform.position.y >= 0.3f) {
                Debug.Log(host.transform.position);
            }
        }

        public class Obstacle
        {
            public Vector3 center;
            public float radius;
        }
    }
}