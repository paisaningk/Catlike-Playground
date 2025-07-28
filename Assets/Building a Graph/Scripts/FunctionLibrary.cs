using System;
using static UnityEngine.Mathf;

namespace Building_a_Graph.Scripts
{
    public static class FunctionLibrary
    {
        public static float Wave(float x, float time)
        {
            return Sin(PI * (x + time));
        }

        public static float MultiWave(float x, float t)
        {
            var y = Sin(PI * (x + 0.5f * t));
            y += 0.5f * Sin(2f * PI * (x + t));
            return y * (2f / 3f);
        }
    }
}
