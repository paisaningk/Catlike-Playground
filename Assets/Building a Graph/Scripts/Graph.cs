using System;
using Building_a_Graph.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class Graph : MonoBehaviour
{
    [SerializeField] public Transform PointPrefab;
    [SerializeField, Range(10, 1000)] public int Resolution = 10;
    [SerializeField] public Transform[] Points;
    [SerializeField, Range(0, 1)] public float TimeScale;

    [SerializeField] FunctionLibrary.FunctionName Function;

    private void Start()
    {
        //แบ่งช่วง −1 ถึง 1 ออกเป็นส่วนย่อย ๆ ให้เหมาะกับการวางวัตถุลายชิ้นเรียงกันแบบพอดีเป๊ะ
        var step = 2f / Resolution;
        var scale = Vector3.one * step;

        Points = new Transform[Resolution * Resolution];

        for (var i = 0; i < Points.Length; i++) 
        {
            var point = Instantiate(PointPrefab, transform);

            Points[i] = point;
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }

    private void Update()
    {
        var f = FunctionLibrary.GetFunction(Function);
        
        var time = Time.time;
        var step = 2f / Resolution;

        // - 1 เพราะว่าเราอยากให้มันเป็นใน -1 ถึง 1
        // จะใช้ (z + 0.5f) * step - 1f;

        // setup v for before the start of the loop
        var v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < Points.Length; i++, x++)
        {
            if (x == Resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;

            Points[i].localPosition = f(u, v, time);
        }
    }
}
