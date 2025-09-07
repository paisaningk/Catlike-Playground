using System;
using Building_a_Graph.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

public class GPUGraph : MonoBehaviour
{
    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private Material material;
    [SerializeField] private Mesh mesh;
    
    [SerializeField, Range(10, 200)] public int Resolution = 10;
    [SerializeField, Range(0, 1)] public float TimeScale;

    [SerializeField] FunctionLibrary.FunctionName Function;
    [SerializeField] private TransitionMode transitionMode;
    [SerializeField, Min(0f)] private float functionDuration = 1f;
    [SerializeField, Min(0f)] private float transitionDuration = 1f;

    private static readonly int
        PositionsId = Shader.PropertyToID("_Positions"),
        ResolutionId = Shader.PropertyToID("_Resolution"),
        StepId = Shader.PropertyToID("_Step"),
        TimeId = Shader.PropertyToID("_Time");
    
    private float duration;
    private bool transitioning;

    private FunctionLibrary.FunctionName transitionFunction;

    private ComputeBuffer positionsBuffer;

    private void OnEnable() 
    {
        // ราจะเก็บตำแหน่งแบบ 3 มิติ = float3 / Vector3 → มี 3 ค่า float
        // float หนึ่งตัวคือ 32 บิต = 4 ไบต์
        // ดังนั้นขนาดต่อหนึ่งตำแหน่ง = 3 * 4 = 12 ไบต์
        positionsBuffer = new ComputeBuffer(Resolution * Resolution, 3 * 4);
    }

    private void OnDisable () 
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }
    
    private void Update()
    {
        UpdateFunctionOnGPU();
        
        // duration += Time.deltaTime;
        // if (transitioning)
        // {
        //     if (duration >= transitionDuration)
        //     {
        //         duration -= transitionDuration;
        //         transitioning = false;
        //     }
        // }
        // else if (duration >= functionDuration)
        // {
        //     duration -= functionDuration;
        //     transitioning = true;
        //     transitionFunction = Function;
        //     PickNextFunction();
        // }
    }

    private void UpdateFunctionOnGPU () 
    {
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / Resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
        
        var step = 2f / Resolution;
        computeShader.SetInt(PositionsId, Resolution);
        computeShader.SetFloat(StepId, step);
        computeShader.SetFloat(TimeId, Time.time);
        
        computeShader.SetBuffer(0, PositionsId, positionsBuffer);
        
        var groups = Mathf.CeilToInt(Resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);
        
        // เราส่ง int ที่เป็นค่าบวกก็จะแปลกเป็น uint
        // ได้เพราะ uint คือค่า int ที่เป็นบวก
    }

    private void PickNextFunction ()
    {
        Function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(Function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(Function);
    }
    
}
