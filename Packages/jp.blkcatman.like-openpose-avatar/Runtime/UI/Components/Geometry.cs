using UnityEngine;
using UnityEngine.UI;

namespace LikeOpenPoseAvatar.UI.Components
{
    public record Geometry
    {
        public Vector3[] Vertices { get; set; }
        
        public Vector2[] Coords { get; set; }
        
        public int[] Indices { get; set; }

        public static int Set(Geometry geo, VertexHelper vh, int indexOffset)
        {
            for (int i = 0; i < geo.Vertices.Length; i++)
            {
                var pos = geo.Vertices[i];
                var vert = UIVertex.simpleVert;
                vert.position = pos;
                vert.uv0 = geo.Coords[i];
                vert.color = Color.white;
                vh.AddVert(vert);
            }

            for (int i = 0; i < geo.Indices.Length / 3; i++)
            {
                var idx = i * 3;
                vh.AddTriangle(
                    geo.Indices[idx + 0] + indexOffset,
                    geo.Indices[idx + 1] + indexOffset,
                    geo.Indices[idx + 2] + indexOffset);
            }
            return indexOffset + geo.Vertices.Length;
        }
    }
}
