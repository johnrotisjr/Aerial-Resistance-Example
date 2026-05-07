using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Extensions
{
 
    public static class FrameworkExtensions
    {
        public static Rect BoundsToRect(this Bounds bounds)
        {
            var center = bounds.center;
            var extents = bounds.extents;
            return new Rect(
                center.x - extents.x,
                center.y - extents.y,
                extents.x * 2f,
                extents.y * 2f
            );
        }

        public static Vector3 Half(this Vector3 vec)
        {
            return new Vector3(vec.x * .5f, vec.y * .5f, vec.z * .5f);
        }
        
        public static float Cross2d(this Vector2 a, Vector2 b) => a.x * b.y - a.y * b.x;

        public static bool IsBetweenWedge(this Vector2 dir, Vector2 min, Vector2 max)
        {
            float s = Cross2d(min, max);
            float a = Cross2d(min, dir);
            float b = Cross2d(dir, max);
            
            if (s >= 0f)// <= 180° wedge
                return a >= -float.Epsilon && b >= -float.Epsilon;          
            else// > 180° wedge
                return !(a > float.Epsilon && b > float.Epsilon);           
        }
    }
}