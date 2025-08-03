using System;
using static UnityEngine.Mathf;

namespace Building_a_Graph.Scripts
{
    public static class FunctionLibrary
    {
        public delegate float Function(float x, float z, float t);

        public enum FunctionName { Wave, MultiWave, Ripple }

        private static readonly Function[] Functions = { Wave, MultiWave, Ripple };

        public static Function GetFunction(int index)
        {
            return Functions[index];
        }

        public static Function GetFunction(FunctionName name)
        {
            return Functions[(int)name];
        }

        public static float Wave(float x, float z, float time)
        {
            return Sin(PI * (x + time));
        }

        public static float MultiWave(float x, float z, float t)
        {
            var y = Sin(PI * (x + 0.5f * t));
            y += 0.5f * Sin(2f * PI * (x + t));
            return y * (2f / 3f);
        }

        public static float Ripple(float x, float z, float t)
        {
            var d = Abs(x);
            var y = Sin(PI * (4f * d - t));
            return y / (1f + 10f * d);
        }
    }
}
