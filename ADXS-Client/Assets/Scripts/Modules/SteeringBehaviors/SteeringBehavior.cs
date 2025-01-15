using UnityEngine;

namespace Modules.SteeringBehaviors
{
    public class SteeringBehavior : MonoBehaviour
    {
        public Vector3 currentVelocity;
        public float maxForce = 10;
        public float maxSpeed = 9;
        public Vector3 currentPos => transform.position;
        public float seekWeight = 1;
        private Vector3 targetPos;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) {
                var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out var hit, 100, LayerMask.NameToLayer("Ground"));
                targetPos = hit.point;
            }

            if ((currentPos - targetPos).magnitude < 0.1f) {
                return;
            }

            Vector3 speed = Seek(targetPos);
            UpdateVelocity(speed);
            transform.position += currentVelocity * Time.deltaTime;
        }

        public void UpdateVelocity(Vector3 steering, float weight = 1)
        {
            Vector3 newVelocity = Vector3.ClampMagnitude(currentVelocity + steering, maxForce);
            newVelocity /= weight;
            currentVelocity = Vector3.ClampMagnitude(newVelocity + currentVelocity, maxSpeed);
        }

        public Vector3 Seek(Vector3 _targetPos)
        {
            Vector3 desiredVelocity = (_targetPos - currentPos).normalized * maxSpeed;
            Vector3 steering = desiredVelocity - currentVelocity;
            return steering;
        }
    }
}