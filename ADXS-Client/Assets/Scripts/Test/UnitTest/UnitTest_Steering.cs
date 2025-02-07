using System.Collections.Generic;
using Assets.Scripts.Modules.SteeringBehaviors;
using Assets.Scripts.Test.UnitTest;
using UnityEngine;

namespace Test
{
    public class UnitTest_Steering : UnitTest_Base
    {
        public Boid host;
        public Transform group;
        public float maxAvoidForce = 10;


        private List<IBoid> boids = new();
        public static Vector3 targetPos;

        protected override void Start()
        {
            base.Start();
            targetPos = host.Position;
            for (int i = 0; i < group.childCount; i++) {
                Boid boid = group.GetChild(i).GetComponent<Boid>();
                if (!boid.gameObject.activeInHierarchy) {
                    continue;
                }

                boids.Add(boid);
            }
        }

        private void Update()
        {
            Arrive();
            FollowLeader();
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
            steering += host.SteeringBehavior.Arrive(targetPos, host.arriveSlowingRadius);
            //steering += host.steeringManager.CollisionAvoidance(maxAvoidForce);
            host.SteeringBehavior.Update(steering);
        }


        private void FollowLeader()
        {
            foreach (var boid in boids) {
                Vector3 steering = Vector3.zero;
                steering += ((Boid)boid).BoidBehavior(host, boids, 4, _boidArriveDistance, _boidSeparateDistance);
                boid.SteeringBehavior.Update(steering);
            }
        }

        public float _boidArriveDistance = 5;
        public float _boidSeparateDistance = 5;
        public float ignoreSteering = 0.3f;
        
        
        void Test(Vector3 direction)
        {
            var rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);
            if (rotation != transform.rotation) {
                var rotationCoeff = 4f;
                var ip = Mathf.Exp(-rotationCoeff * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(rotation, transform.rotation, ip);
            }
        }
    }
}