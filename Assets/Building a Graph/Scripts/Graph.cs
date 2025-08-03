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
        var position = Vector3.zero;
        var scale = Vector3.one * step;

        Points = new Transform[Resolution * Resolution];

        for (int i = 0, x = 0, z = 0; i < Points.Length; i++, x++) 
        {
            if (x == Resolution)
            {
                x = 0;
                z += 1;
            }
            
            // - 1 เพราะว่าเราอยากให้มันเป็นใน -1 ถึง 1
            position.x = (x + 0.5f) * step - 1f;
            position.z = (z + 0.5f) * step - 1f;

            var point = Instantiate(PointPrefab, transform);

            point.localPosition = position;
            point.localScale = scale;

            Points[i] = point;
        }
    }

    private void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(Function);
        
        var time = Time.time;
        foreach (var point in Points)
        {
            var position = point.localPosition;

            position.y = f(position.x, position.z, time * TimeScale);

            point.localPosition = position;
        }
    }
}
