using System.Collections.Generic;
using System.Linq;
using Assets.GameClientLib.Scripts.Utils;
using Test;
using UnityEngine;

namespace Modules.SteeringBehaviors
{
    public class BoidBehavior
    {
        public Boid host;

        public Vector3 TestBoids(IBoid leader, List<IBoid> boids, int groupNum = 3)
        {
            Vector3 force = Vector3.zero;
            boids = boids.OrderBy(o => (o.Position - leader.Position).magnitude).ToList();
            List<List<IBoid>> neighbors = new() { new List<IBoid>() { leader } };
            int s = boids.Count / groupNum;
            int add1 = boids.Count % groupNum == 0 ? 0 : 1;
            s += add1;
            int index = 0;
            for (int i = 0; i < s; i++) {
                IBoid[] group = boids.Skip(i * groupNum).Take(groupNum).ToArray();
                neighbors.Add(group.ToList());
                if (group.ToList().Exists(o => o == host)) {
                    index = i + 1;
                }
            }

            boids.Add(leader);
            List<IBoid> otherBoids = boids.Where(o => o.transform != host.transform).ToList();

            force += TestSeparation(otherBoids, out var pos, 5, UnitTest_Steering.boidSeparateDistance);
            force += TestArrive(neighbors[index - 1], pos, 2, UnitTest_Steering.boidArriveDistance);

            // force += TestCohesion(otherBoids, 3, UnitTest_Steering.boidSeparateDistance + 2);
            host.transform.position += force * Time.deltaTime;
            return force;
        }

        Vector3 TestArrive(List<IBoid> lastBoids, Vector3 pos, int num = 3, float arriveDistance = 3)
        {
            num = Mathf.Min(lastBoids.Count, num);
            List<IBoid> nearbyBoids = GetRecentlyBoids(lastBoids, num);
            Vector3 force = Vector3.zero;

            foreach (var boid in nearbyBoids) {
                force = boid.Position - host.Position;
            }

            force /= num;
            // if (((pos - host.Position) - force).magnitude < 2) {
            //     return host.steeringManager.Arrive(pos, 2);
            // }

            if (force.magnitude > arriveDistance) {
                return Vector3.ClampMagnitude(force, host.MaxForce);
            }

            return Vector3.zero;
        }


        Vector3 TestSeparation(List<IBoid> otherBoids, out Vector3 endPos, int num = 5, float separateDistance = 2, float maxSeparationForce = 3)
        {
            var boids = GetRecentlyBoids(otherBoids, num);
            Vector3 force = Vector3.zero;
            endPos = Vector3.zero;
            int neighborCount = 0;
            for (int i = 0; i < boids.Count; i++) {
                var b = boids[i];
                if ((b.Position - host.Position).magnitude <= separateDistance) {
                    force += b.Position - host.Position;
                    endPos += b.Position;
                    neighborCount++;
                }
            }

            if (neighborCount == 0) {
                return Vector3.zero;
            }

            force /= neighborCount;
//            Debug.Log("force: " + force);
            Vector2 f = force.GetXZ().normalized * -maxSeparationForce;
            return f.XZToXYZ();
        }

        Vector3 TestCohesion(List<IBoid> otherBoids, int num = 3, float cohesionDistance = 3)
        {
            num = Mathf.Min(otherBoids.Count, num);
            IBoid[] nearbyBoids = GetRecentlyBoids(otherBoids, num).ToArray();
            Vector3 cohesion = Vector3.zero;
            if ((nearbyBoids.First().Position - host.Position).magnitude < cohesionDistance) {
                return cohesion;
            }

            foreach (var boid in nearbyBoids) {
                cohesion += boid.Position;
            }

            cohesion /= num;
            return cohesion - host.Position;
        }


        public List<IBoid> GetRecentlyBoids(List<IBoid> otherBoids, int num)
        {
            num = Mathf.Min(otherBoids.Count, num);
            return otherBoids.OrderBy(o => (o.Position - host.Position).magnitude).Skip(0).Take(num).ToList();
        }
    }
}