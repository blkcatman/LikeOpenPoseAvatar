using System;
using UnityEngine;

namespace LikeOpenPoseAvatar.UI.Components
{
    public class CirclePlotter : IShapePlotter
    {
        private int segments = 16;
        
        private Vector2 textureCoord = Vector2.zero;

        public void SetColorCoord(Vector2 coord)
        {
            textureCoord = coord;
        }
        
        public void SetNumberOfSegments(int value)
        {
            segments = value;
        }

        public Geometry DrawShape(Vector3 center, float width, float height, float angle)
        {
            Vector3[] circleVerts = new Vector3[segments + 2]; // segments(first to last) + center + segment(first)
            float radianPerSegments = (1.0f / (float)segments) * Mathf.PI * 2.0f;
            float halfWidth = width / 2.0f;
            float halfHeight = height / 2.0f;
            circleVerts[0] = center;
            for (int i = 0; i < segments + 1; i++)
            {
                var radian = radianPerSegments * (i % segments);
                circleVerts[i + 1] = new Vector3(
                    Mathf.Cos(radian) * halfWidth,
                    Mathf.Sin(radian) * halfHeight,
                    0f) + center;
            }
            
            Vector2[] coords = new Vector2[segments + 2];
            Array.Fill(coords, textureCoord);
            int[] indices = new int[segments * 3];
            for (int i = 0; i < segments; i++)
            {
                var idx = i * 3;
                indices[idx + 0] = i + 1;
                indices[idx + 1] = 0;
                indices[idx + 2] = i + 2;
            }

            return new Geometry
            {
                Vertices = circleVerts,
                Coords = coords,
                Indices = indices
            };
        }
    }
}
