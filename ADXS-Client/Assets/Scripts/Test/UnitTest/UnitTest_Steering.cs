using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.SteeringBehaviors;
using Assets.Scripts.Test.UnitTest;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Test
{
    public class UnitTest_Steering : UnitTest_Base
    {
        public Boid host;
        public Transform group;
        public float maxAvoidForce = 10;


        private List<IBoid> boids = new();
        public static Vector3 targetPos;
        private List<SteeringBehavior.Obstacle> obstacles;

        protected override void Start()
        {
            base.Start();
            //CreateBoids();
            targetPos = host.Position;
            obstacles = new List<SteeringBehavior.Obstacle>();
            for (int i = 0; i < group.childCount; i++)
            {
                Boid boid = group.GetChild(i).GetComponent<Boid>();
                if (!boid.gameObject.activeInHierarchy)
                {
                    continue;
                }

                boids.Add(boid);
                obstacles.Add(new SteeringBehavior.Obstacle(boid.transform, 2));
            }
        }

        private void Update()
        {
            Arrive();
            FollowLeader();
        }

        private void Arrive()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000))
                {
                    targetPos = hit.point;
                }
            }

            Vector3 steering = Vector3.zero;
            steering += host.SteeringBehavior.Arrive(targetPos, host.arriveSlowingRadius);
            //steering += host.steeringManager.CollisionAvoidance(maxAvoidForce);
            host.SteeringBehavior.Update(steering);
        }

        public float leaderBehindDist = 5;
        public float separationRadius = 3;
        public float maxSeparationForce = 3;
        public float leaderSightRadius = 3;

        private void FollowLeader()
        {
            boidArriveDistance = _boidArriveDistance;
            boidSeparateDistance = _boidSeparateDistance;

            foreach (var boid in boids)
            {

                Vector3 steering = Vector3.zero;
                steering += boid.SteeringBehavior.BoidBehavior(host, boids, 4);
                boid.SteeringBehavior.Update(steering);
            }
        }

        public static float boidArriveDistance = 5;
        public static float boidSeparateDistance = 5;
        public float _boidArriveDistance = 5;
        public float _boidSeparateDistance = 5;
        public float ignoreSteering = 0.3f;
    }
}