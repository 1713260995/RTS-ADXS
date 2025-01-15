using System;
using UnityEngine;

namespace Modules.SteeringBehaviors
{
    public class BoidBehavior : MonoBehaviour
    {
        public float maxSpeed;
        public float maxForce;
        public float seekWeight = 1;
        public SteeringBehavior steeringBehavior;

        [HideInInspector]
        private Vector3 currentVelocity => body.velocity;

        private void Awake()
        {
            steeringBehavior = GetComponent<SteeringBehavior>();
            body = GetComponent<Rigidbody>();
        }

        private Vector3 targetPos;
        private Rigidbody body;

        public void Update()
        {
            if (Input.GetMouseButtonDown(1)) {
                Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    targetPos = hit.point;
                }
            }

            body.AddForce((Seek(targetPos) * seekWeight) / mass);
            if (body.velocity.magnitude > maxSpeed) {
                print(body.velocity.magnitude);
            }
        }

        public int mass = 3;
        
        public Vector3 Seek(Vector3 target)
        {
            Vector3 steerForce = GetDesiredVelocity(target) - currentVelocity;
            if (steerForce.magnitude > maxForce) {
                steerForce = steerForce.normalized * maxForce;
            }

            return steerForce;
        }

        Vector3 GetDesiredVelocity(Vector3 target)
        {
            return (target - transform.position).normalized * maxSpeed;
        }
    }
}