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
        public Vector3 Steering { get; private set; }
        public IBoid Host { get; }

        public SteeringManager(IBoid host)
        {
            Host = host;
            Steering = Vector3.zero;
        }

        /// <summary>
        /// 在每帧的速度上增加转向力将使角色平稳地放弃其旧的直线路线并朝着目标前进
        /// </summary>
        public Vector3 Seek(Vector3 target)
        {
            Vector3 desiredVelocity = (target - Host.Position).normalized * Host.MaxSpeed;
            Vector3 steering = desiredVelocity - Host.Velocity;
            return steering;
        }

        /// <summary>
        /// 逃逸行为
        /// 新向量的计算方法是从目标的位置中减去角色的位置，从而生成从目标到角色的向量
        /// </summary>
        /// <param name="target"></param>
        public Vector3 Flee(Vector3 target)
        {
            Vector3 desired_velocity = (Host.Position - target).normalized * Host.MaxSpeed;
            return desired_velocity - Host.Velocity;
        }

        /// <summary>
        /// 到达行为
        /// 当角色进入减速区域时，其速度将线性下降到零。
        /// </summary>
        public Vector3 Array(Vector3 targetPos, float slowingRadius = 5)
        {
            Vector3 desired_velocity = targetPos - Host.Position;
            var distance = desired_velocity.magnitude;
            if (distance < slowingRadius) {
                desired_velocity = desired_velocity.normalized * Host.MaxSpeed * (distance / slowingRadius);
            }
            else {
                desired_velocity = desired_velocity.normalized * Host.MaxSpeed;
            }

            Vector3 steering = desired_velocity - Host.Velocity;
            return steering;
        }

        /// <summary>
        /// 游荡行为
        /// 模拟游戏的角色在环境中随机移动
        /// </summary>
        private Vector3 Wander(float circleDistance, float circleRadius, float wanderAngle, float angleChange)
        {
            Vector3 circleCenter = (Host.Position + Host.Velocity.normalized) * circleDistance;

            // Calculate the displacement force
            Vector3 displacement = new Vector3(0, 0, -1) * circleRadius;

            // Randomly change the vector direction by adjusting its current angle
            SetAngle(ref displacement, wanderAngle);

            // Change wanderAngle just a bit, so it won't have the same value in the next frame
            wanderAngle += Random.Range(0f, angleChange) - angleChange * 0.5f;

            // Finally, calculate and return the wander force
            Vector3 wanderForce = circleCenter + displacement;
            return wanderForce;
        }

        private void SetAngle(ref Vector3 vector, float value)
        {
            float len = vector.magnitude;
            vector.x = Mathf.Cos(value) * len;
            vector.y = Mathf.Sin(value) * len;
        }

        /// <summary>
        /// 追逐行为
        /// 追逐行为的工作方式与Seek的方式几乎相同，唯一的区别是追逐者不会搜索目标本身，而是搜索目标在不久的将来的位置。
        /// </summary>
        /// <param name="boid"></param>
        /// <returns></returns>
        public Vector3 Pursuit(IBoid boid)
        {
            Vector3 distance = boid.Position - Host.Position;
            float time = distance.magnitude / Host.MaxSpeed;
            Vector3 futurePosition = boid.Position + boid.Velocity * time;
            return Seek(futurePosition);
        }

        public Vector3 FollowLeader(IBoid leader, float LEADER_BEHIND_DIST)
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

            force += Array(behind, 50);
            force += Separation();
            return force;
        }

        private Vector3 Evade(IBoid t)
        {
            Vector3 distance = t.Position - Host.Position;
            float updatesAhead = distance.magnitude / Host.MaxSpeed;
            Vector3 futurePosition = t.Position + t.Velocity * updatesAhead;
            return Flee(futurePosition);
        }

        private Vector3 Separation()
        {
            float SEPARATION_RADIUS = 50; //TODO: need set
            float MAX_SEPARATION = 10; //TODO: need set
            IBoid[] boids = { }; //TODO: get all boids
            Vector3 force = Vector3.zero;
            int neighborCount = 0;
            for (int i = 0; i < boids.Length; i++) {
                var b = boids[i];
                if (b != Host && (b.Position - Host.Position).magnitude <= SEPARATION_RADIUS) {
                    force.x += b.Position.x - Host.Position.x;
                    force.y += b.Position.y - Host.Position.y;
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
            return (ahead - Host.Position).magnitude <= leaderSightRadius || (leader.Position - Host.Position).magnitude <= leaderSightRadius;
        }


        public void Update()
        {
            Steering = Vector3.ClampMagnitude(Steering, Host.MaxForce);
            Steering = Steering / Host.Mass;
            Host.Velocity = Vector3.ClampMagnitude(Host.Velocity + Steering, Host.MaxSpeed);
            Host.transform.position += Host.Velocity * Time.deltaTime;
        }


        private void ApplyForce(Vector3 force)
        {
            Steering += force;
        }
    }
}