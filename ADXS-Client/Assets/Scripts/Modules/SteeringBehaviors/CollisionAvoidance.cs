using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : MonoBehaviour
{
    public float detectionRadius = 5.0f;    // 障碍物检测半径
    public float avoidanceStrength = 10.0f; // 避障的强度（影响调整速度的大小）
    public float maxSpeed = 3.0f;           // 最大速度
    public List<Transform> obstacles;       // 障碍物列表

    private Vector2 currentVelocity;        // 当前速度

    // Update is called once per frame
    void Update()
    {
        Vector2 avoidanceForce = Vector2.zero;

        // 遍历障碍物列表，检测每一个障碍物
        foreach (var obstacle in obstacles)
        {
            Vector2 directionToObstacle = (obstacle.position - transform.position).normalized;
            float distanceToObstacle = Vector2.Distance(obstacle.position, transform.position);

            // 如果障碍物在检测范围内
            if (distanceToObstacle < detectionRadius)
            {
                // 计算避障力，越接近障碍物，避障力越强
                float avoidanceFactor = (detectionRadius - distanceToObstacle) / detectionRadius;
                avoidanceForce -= directionToObstacle * (avoidanceStrength * avoidanceFactor);
            }
        }

        // 将避障力应用到角色的速度上
        ApplySteeringForce(avoidanceForce);

        // 更新角色的位置
        MoveCharacter();
    }

    // 应用避障力到角色
    void ApplySteeringForce(Vector2 steeringForce)
    {
        // 更新当前速度
        currentVelocity += steeringForce * Time.deltaTime;

        // 限制速度不超过最大速度
        if (currentVelocity.magnitude > maxSpeed)
        {
            currentVelocity = currentVelocity.normalized * maxSpeed;
        }
    }

    // 根据当前速度移动角色
    void MoveCharacter()
    {
        transform.position += (Vector3)currentVelocity * Time.deltaTime;
    }
}