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
        public void Flee(Vector3 target)
        {
            Vector3 desired_velocity = (Host.Position - target).normalized * Host.MaxSpeed;
            Vector3 steering = desired_velocity - Host.Velocity;
            ApplyForce(steering);
        }

        /// <summary>
        /// 到达行为
        /// 当角色进入减速区域时，其速度将线性下降到零。
        /// </summary>
        public void Array(Vector3 targetPos, float slowingRadius = 5)
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
            ApplyForce(steering);
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



        public void Update()
        {
            Steering = Vector3.ClampMagnitude(Steering, Host.MaxForce);
            Steering = Steering / Host.Mass;
            Host.Velocity = Vector3.ClampMagnitude(Host.Velocity + Steering, Host.MaxSpeed);
            Host.transform.position += Host.Velocity * Time.deltaTime;
        }

        public void Reset()
        {
            Steering = Vector3.zero;
        }

        private void ApplyForce(Vector3 force)
        {
            Steering += force;
        }
    }
}