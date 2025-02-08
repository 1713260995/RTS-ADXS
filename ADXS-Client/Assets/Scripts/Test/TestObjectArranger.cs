using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.GameClientLib.Scripts.Utils;

public class TestObjectArranger : MonoBehaviour
{
    public Transform origin; // 给定的点
    public Transform parentObj;
    public Transform[] objects; // 需要摆放的游戏对象数组
    public int groupSize = 5; // 每组的数量
    public float horizontalSpacing = 2.0f; // 左右间隔
    public float verticalSpacing = 3.0f; // 组之间的间隔
    
    void Start()
    {
        // objects = parentObj.GetComponentsInChildren<Transform>();
        // objects = objects.Where(o => o.gameObject.activeInHierarchy).ToArray();
        objects = new Transform[parentObj.childCount];
        for (int i = 0; i < parentObj.childCount; i++) {
            objects[i] = parentObj.GetChild(i);
        }
    
        List<Vector3> list = MyMath.CreateMatrixPoints(origin.position,objects.Length , groupSize, horizontalSpacing, verticalSpacing);
        foreach (var item in objects) {
            var index = Array.IndexOf(objects, item);
            item.position = list[index];
            print($"item={item.name}" + list[index]);
        }
    }

    /// <summary>
    /// 创建一组点，形状类似方阵。
    /// 根据给定点位置，在该点左右两侧创建一组相同间隔的点。
    /// 后续的点按照这个规则在给定点的后面创建。
    /// </summary>
    // List<Vector3> CreateMatrixPoints(Vector3 origin, int pointsNum, int rowCount, float _horizontalSpacing, float _verticalSpacing)
    // {
    //     List<Vector3> positions = new List<Vector3>();
    //     int groupCount = Mathf.CeilToInt((float)pointsNum / rowCount);
    //     bool evenNumber = rowCount % 2 == 0;
    //     Vector3 deltaRight = evenNumber ? _horizontalSpacing / 2 * Vector3.right : Vector3.zero;
    //     Vector3 deltaLeft = evenNumber ? _horizontalSpacing / 2 * Vector3.left : Vector3.zero;
    //     for (int i = 0; i < groupCount; i++) {
    //         Vector3 currentGroupCenter = origin + Vector3.back * i * _verticalSpacing; // 计算每组的基准位置
    //         if (!evenNumber)
    //             positions.Add(currentGroupCenter);
    //         for (int j = 1; j <= rowCount / 2; j++) {
    //             Vector3 left = currentGroupCenter + (j * _horizontalSpacing * Vector3.left);
    //             Vector3 right = currentGroupCenter + (j * _horizontalSpacing * Vector3.right);
    //             positions.Add(left - deltaLeft);
    //             positions.Add(right - deltaRight);
    //         }
    //     }
    //
    //     return positions;
    // }
}