using UnityEngine;

namespace Modules.SteeringBehaviors
{
    public class Boid : MonoBehaviour, IBoid
    {
        public Vector3 Position => transform.position;
        public Vector3 Velocity { get; set; }
        public float MaxSpeed => maxSpeed;
        public float MaxForce => maxForce;
        public float Mass => mass;
        public float maxSpeed;
        public float maxForce;
        public float mass = 1;
        public float slowingRadius = 1;
        private SteeringManager steeringManager;

        private void Awake()
        {
            steeringManager = new SteeringManager(this);
        }

        private Vector3 targetPos;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)) {
                Ray ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    targetPos = hit.point;
                }
            }

            steeringManager.Seek(targetPos);
            steeringManager.Update();
        }
    }
}