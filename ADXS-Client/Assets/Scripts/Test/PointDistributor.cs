using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PointDistributor : MonoBehaviour
{
    public int n = 10; // 总点数
    public float spacing = 1.0f; // 点之间的间距

    List<GameObject> arr = new List<GameObject>();
    private int frame;
    void Update()
    {
        if (Time.time - frame < 1)
        {
            return;
        }
        frame += 1;
        foreach (GameObject go in arr)
        {
            Destroy(go);
        }
        arr.Clear();
        List<Vector2> points = GeneratePoints(n, spacing);

        foreach (Vector2 point in points)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            arr.Add(sphere);
            sphere.transform.position = new Vector3(point.x, point.y, 0);
            sphere.transform.localScale = Vector3.one * 0.2f;
        }
    }

    List<Vector2> GeneratePoints(int n, float spacing)
    {
        List<Vector2> points = new List<Vector2>();
        points.Add(Vector2.zero); // 中心点

        int currentLayer = 1; // 当前层数
        int pointsPlaced = 1;  // 已放置的点数
        while (pointsPlaced < n)
        {
            int pointsInLayer = currentLayer * 4; // 每层增加 4 个点
            for (int i = 0; i < pointsInLayer && pointsPlaced < n; i++)
            {
                float angle = Mathf.PI / 2 * (i / (float)currentLayer); // 90° 旋转
                int x = (int)(Mathf.Cos(angle) * currentLayer * spacing);
                int y = (int)(Mathf.Sin(angle) * currentLayer * spacing);
                points.Add(new Vector2(x, y));
                pointsPlaced++;
            }
            currentLayer++;
        }
        return points;
    }
}
