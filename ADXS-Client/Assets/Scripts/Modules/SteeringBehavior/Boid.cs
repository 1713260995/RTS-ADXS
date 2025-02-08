using System.Collections.Generic;
using System.Linq;
using Assets.GameClientLib.Scripts.Utils;
using Test;
using UnityEngine;

namespace Assets.Scripts.Modules.SteeringBehaviors
{
    public class Boid : IBoid
    {
        /// <summary>
        /// 最大速度
        /// </summary>
        public float maxSpeed = 5;

        /// <summary>
        /// steering force最大值
        /// </summary>
        public float maxForce = 6;

        /// <summary>
        /// steering force整体缩放权重
        /// </summary>
        public float mass = 1;

        /// <summary>
        /// CollisionAvoidance行为 boid的自身半径
        /// </summary>
        public float radius = 2;

        public Transform transform { get; }
        public Vector2 Position => transform.position.GetXZ();
        public Vector2 Velocity { get; set; }
        public float Mass => mass;
        public float MaxSpeed => maxSpeed;
        public float MaxForce => maxForce;
        public float Radius => radius;
        public SteeringBehavior SteeringBehavior { get; private set; }

        public Boid(Transform transform, float maxSpeed)
        {
            this.transform = transform;
            this.maxSpeed = maxSpeed;
            this.maxForce = maxSpeed / 0.8f;
            SteeringBehavior = new SteeringBehavior(this);
        }

    }
}