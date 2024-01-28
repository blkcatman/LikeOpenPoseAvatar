using System;
using UnityEngine;

namespace LikeOpenPoseAvatar.UI.Components
{
    public class UniformLinePlotter : ILinePlotter
    {
        private Vector2 textureCoord = Vector2.zero;
        
        public void SetColorCoord(Vector2 coord)
        {
            textureCoord = coord;
        }
        
        public Geometry DrawLine(Vector3 from, Vector3 to, float thickness)
        {
            var fromZ = from.z;
            var toZ = to.z;
            var fromVec2 = new Vector3(from.x, from.y, 0f);
            var toVec2 = new Vector3(to.x, to.y, 0f);
            var direction = Vector3.Normalize(toVec2 - fromVec2);
            var tangent = Vector3.Cross(direction, Vector3.forward);
            var halfThickness = thickness * 0.5f;
            var fromLeft = from - (tangent * halfThickness);
            var fromRight = from + (tangent * halfThickness);
            var toLeft = to - (tangent * halfThickness);
            var toRight = to + (tangent * halfThickness);

            return new Geometry
            {
                Vertices = new[] { fromLeft, fromRight, toLeft, toRight },
                Coords = new[] {textureCoord, textureCoord, textureCoord, textureCoord},
                Indices = new[] { 0, 3, 1, 3, 0, 2 }
            };
        }
    }
}
