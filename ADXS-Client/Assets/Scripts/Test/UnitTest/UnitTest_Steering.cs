using System;
using System.Collections.Generic;
using Assets.Scripts.Modules;
using Assets.Scripts.Test.UnitTest;
using Modules.SteeringBehaviors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Test
{
    public class UnitTest_Steering : UnitTest_Base
    {
        public Boid host;
        private Vector3 targetPos;
        public Transform group;

        List<Boid> boids = new List<Boid>();

        protected override void Start()
        {
            base.Start();
            //  CreateBoids();
            targetPos = host.Position;
            obstacles = new List<SteeringManager.Obstacle>();
            for (int i = 0; i < group.childCount; i++) {
                Boid boid = group.GetChild(i).GetComponent<Boid>();
                boids.Add(boid);
                obstacles.Add(new SteeringManager.Obstacle() { center = boid.Position, radius = 2 });
            }
        }

        private void Update()
        {
           // Arrive();
            //Wander();
            FollowLeader();
        }


        private void Wander()
        {
            foreach (var item in boids) {
                Vector3 steering1 = Vector3.zero;
                steering1 += item.steeringManager.Wander(2, 2, 180, 20);
                item.steeringManager.Update(steering1);
            }
        }
        
        private void Arrive()
        {
            if (Input.GetMouseButtonDown(1)) {
                Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000)) {
                    targetPos = hit.point;
                }
            }

            Vector3 steering = Vector3.zero;
            steering += host.steeringManager.Arrive(targetPos, host.arriveSlowingRadius);
            steering += host.steeringManager.CollisionAvoidance(obstacles.ToArray());
            host.steeringManager.Update(steering);
        }

        private void FollowLeader()
        {
            Arrive();

            Vector3 steering = Vector3.zero;
            foreach (var boid in boids) {
                steering += boid.steeringManager.FollowLeader(host, 5, boids!.ToArray(), 2, 1);
                boid.steeringManager.Update(steering);
            }
           
        }
        
        
        private List<SteeringManager.Obstacle> obstacles;

        private void CreateBoids()
        {
            for (int i = 0; i < 10; i++) {
                Vector3 birthPos = Random.insideUnitSphere * 10;
                Boid boid = Instantiate(host, birthPos, Quaternion.identity);
                boids.Add(boid);
            }
        }
    }
}