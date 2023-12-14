using UnityEngine;

namespace LikeOpenPoseAvatar.Keypoints
{
    public interface IKeypointDataResolver
    {
        Color GetKeypointColor(int keypointIndex);
        string GetKeypointName(int keypointIndex);
        int GetNumberOfKeypoints();
    }
}
