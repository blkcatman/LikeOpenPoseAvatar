using UnityEngine;

namespace LikeOpenPoseAvatar.UI.Components
{
    public interface IShapePlotter
    {
        void SetColorCoord(Vector2 coord);
        void SetNumberOfSegments(int value);
        Geometry DrawShape(Vector3 center, float width, float height, float angle);
    }
}
