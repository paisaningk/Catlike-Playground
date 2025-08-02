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

    [SerializeField, Range(0, 2)] public int Function;

    private void Start()
    {
        //แบ่งช่วง −1 ถึง 1 ออกเป็นส่วนย่อย ๆ ให้เหมาะกับการวางวัตถุลายชิ้นเรียงกันแบบพอดีเป๊ะ
        var step = 2f / Resolution;
        var position = Vector3.zero;
        var scale = Vector3.one * step;

        Points = new Transform[Resolution];

        for (var i = 0; i < Points.Length; i++)
        {
            // - 1 เพราะว่าเราอยากให้มันเป็นใน -1 ถึง 1
            position.x = (i + 0.5f) * step - 1f;

            var point = Instantiate(PointPrefab, transform);

            point.localPosition = position;
            point.localScale = scale;

            Points[i] = point;
        }
    }

    private void Update()
    {
        var time = Time.time;
        foreach (var point in Points)
        {
            var position = point.localPosition;

            if (Function == 0)
            {
                position.y = FunctionLibrary.Wave(position.x, time * TimeScale);
            }
            else if (Function == 1)
            {
                position.y = FunctionLibrary.MultiWave(position.x, time * TimeScale);
            }
            else
            {
                position.y = FunctionLibrary.Ripple(position.x, time * TimeScale);
            }

            point.localPosition = position;
        }
    }
}
