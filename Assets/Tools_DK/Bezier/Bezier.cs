using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public LineRenderer line;
    public List<Transform> Ts;
    public int num = 2;
    public bool setLine;

    public List<Color> colors;
    void Start()
    {
        


    }

    [ContextMenu("color")]
    void setColor()
    {
        colors.Clear();
        for (int i = 0; i < 255; i++)
        {
            colors.Add(new Color(255, (float)i / (float)255,0,1));
        }

        for (int i = 0; i < 1000; i++)
        {

            Debug.Log(i % 254);
        }
    }

    // Update is called once per frame
    void Update()
    {
        line.positionCount = 0;
        Vector3[] vector3 = new Vector3[num + 1];
        for (int i = 0; i <= num; i++)
        {
            float value = ((float)i / (float)num);
            vector3[i]=BezierLine(value,Ts);
        }

        line.positionCount = vector3.Length;
        line.SetPositions(vector3);
        for (int i = 0; i < vector3.Length; i++)
        {
            line.SetPosition(i, vector3[i]);
        }

        
    }


    public Vector3 BezierLine(float t, List<Vector3> p)
    {
        //colors.Clear();
        if (p.Count < 2)
            return p[0];
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            if(setLine)
            Debug.DrawLine(p[i], p[i + 1], colors[i%254]);

            Debug.Log(i%254);
            //colors.Add(new Color((float)i / (float)p.Count, (float)i / (float)p.Count, (float)i / (float)p.Count, 1));
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }
        return BezierLine(t, newp);
    }
    // transform转换为vector3，在调用参数为List<Vector3>的Bezier函数
    public Vector3 BezierLine(float t, List<Transform> p)
    {
        if (p.Count < 2)
            return p[0].position;
        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count; i++)
        {
            newp.Add(p[i].position);
        }
        return BezierLine(t, newp);
    }

}
