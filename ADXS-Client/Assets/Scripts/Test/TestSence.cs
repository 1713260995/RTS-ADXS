using Assets.GameClientLib.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class TestSence : MonoBehaviour
{
    public Transform A1;
    public Transform B1;

    public Transform A2;
    public Transform B2;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = frame;
        StartCoroutine(TestA1());
        StartCoroutine(TestA2());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator TestA1()
    {
        while (true)
        {
            var s = MyMath.GetRotateAngle(A1, B1.position);
            var rotate = A1.localEulerAngles;
            //rotate.y = MyMath.Lerp(rotate.y, rotate.y + s, 0.01f);
            A1.localEulerAngles = rotate;

            if (Mathf.Abs(s) < 0.5f)
            {
                yield return new WaitForSeconds(1);
                A1.localEulerAngles = new Vector3(0, 60, 0);
                yield return new WaitForSeconds(1);
                break;
            }

            yield return null;
        }
        StartCoroutine(TestA1());
    }


    private IEnumerator TestA2()
    {
        while (true)
        {
            var s = MyMath.GetRotateAngle(A2, B2.position);
            var rotate = A2.localEulerAngles;
            rotate.y += s;
            A2.localEulerAngles = Vector3.Lerp(A2.localEulerAngles, rotate, speedLerp * Time.deltaTime);
            if (Mathf.Abs(s) < 0.5f)
            {
                yield return new WaitForSeconds(1);
                A2.localEulerAngles = new Vector3(0, 60, 0);
                yield return new WaitForSeconds(1);
                break;
            }

            yield return null;
        }
        StartCoroutine(TestA2());
    }


    public int frame = 60;

    public float speedLerp = 0.01f;




}


