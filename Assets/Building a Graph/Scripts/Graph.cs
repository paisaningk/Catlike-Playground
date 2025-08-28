using System;
using Building_a_Graph.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public enum TransitionMode { Cycle, Random }

public class Graph : MonoBehaviour
{
    [SerializeField] public Transform PointPrefab;
    [SerializeField, Range(10, 1000)] public int Resolution = 10;
    [SerializeField] public Transform[] Points;
    [SerializeField, Range(0, 1)] public float TimeScale;

    [SerializeField] FunctionLibrary.FunctionName Function;
    [SerializeField] private TransitionMode transitionMode;
    [SerializeField, Min(0f)] private float functionDuration = 1f;
    [SerializeField, Min(0f)] private float transitionDuration = 1f;
    
    private float duration;
    private bool transitioning;

    private FunctionLibrary.FunctionName transitionFunction;
    
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
        duration += Time.deltaTime;
        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = Function;
            PickNextFunction();
        }
        
        if (transitioning) 
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateFunction();
        }
    }

    private void PickNextFunction ()
    {
        Function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(Function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(Function);
    }
    
    private void UpdateFunction()
    {
        var f = FunctionLibrary.GetFunction(Function);
        
        var time = Time.time * TimeScale;
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

            var u = (x + 0.5f) * step - 1f;

            Points[i].localPosition = f(u, v, time);
        }
    }
    
    private void UpdateFunctionTransition()
    {
        
        var from = FunctionLibrary.GetFunction(transitionFunction);
        var to = FunctionLibrary.GetFunction(Function);
        
        var progress = duration / transitionDuration;
        var time = Time.time * TimeScale;
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

            var u = (x + 0.5f) * step - 1f;

            Points[i].localPosition = FunctionLibrary.Morph(
                u, v, time, from, to, progress
            );
        }
    }
    
}
