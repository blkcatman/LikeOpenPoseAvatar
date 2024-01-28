using UnityEngine;

namespace LikeOpenPoseAvatar.UI.Components
{
    public interface ILinePlotter
    {
        public void SetColorCoord(Vector2 coord);

        Geometry DrawLine(Vector3 from, Vector3 to, float thickness);
    }
}
