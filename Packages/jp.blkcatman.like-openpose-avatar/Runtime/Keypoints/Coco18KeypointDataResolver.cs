using UnityEngine;

namespace LikeOpenPoseAvatar.Keypoints
{
    /// <summary>
    /// OpenPose Coco Dataset Key points Data
    /// </summary>
    /// <remarks>
    /// Indices of Coco Dataset Key points
    /// Nose = 0,
    /// Neck = 1,
    /// RightShoulder = 2,
    /// RightElbow = 3,
    /// RightWrist = 4,
    /// LeftShoulder = 5,
    /// LeftElbow = 6,
    /// LeftWrist = 7,
    /// RightHip = 8,
    /// RightKnee = 9,
    /// RightAnkle = 10,
    /// LeftHip = 11,
    /// LeftKnee = 12,
    /// LeftAnkle = 13,
    /// RightEye = 14,
    /// LeftEye = 15,
    /// RightEar = 16,
    /// LeftEar = 17,
    /// Background = 18(unused)
    /// </remarks>
    public class Coco18KeypointDataResolver : IKeypointDataResolver
    {
        private readonly string[] keypointNames = {
            "Nose",
            "Neck",
            "RightShoulder",
            "RightElbow",
            "RightWrist",
            "LeftShoulder",
            "LeftElbow",
            "LeftWrist",
            "RightHip",
            "RightKnee",
            "RightAnkle",
            "LeftHip",
            "LeftKnee",
            "LeftAnkle",
            "RightEye",
            "LeftEye",
            "RightEar",
            "LeftEar",
            "Background", // (unused)
        };

        private readonly Color[] keypointColors =
        {
            new Color(1f, 0f/255f, 0f), // Nose
            new Color(1f, 85f/255f, 0f), // Neck
            new Color(1f, 170f/255f, 0f), // RightShoulder
            new Color(1f, 1f, 0f), // RightElbow
            new Color(170f/255f, 1f, 0f), // RightWrist
            new Color(85f/255f, 1f, 0f), // LeftShoulder
            new Color(0f/255f, 1f, 0f), // LeftElbow
            new Color(0f, 1f, 85f/255f), // LeftWrist
            new Color(0f, 1f, 170f/255f), // RightHip
            new Color(0f, 1f, 1f), // RightKnee
            new Color(0f, 170f/255f, 1f), // RightAnkle
            new Color(0f, 85f/255f, 1f), // LeftHip
            new Color(0f, 0f, 1f), // LeftKnee
            new Color(85f/255f, 0f, 1f), // LeftAnkle
            new Color(170f/255f, 0f, 1f), // RightEye
            new Color(1f, 0f, 1f), // LeftEye
            new Color(1f, 0f, 170f/255f), // RightEar
            new Color(1f, 0f, 85f/255f), // LeftEar
            new Color(0f,0f, 0f) // Background(unused)
        };
        
        private readonly int numberOfKeypoints = 18;

        public Color GetKeypointColor(int keypointIndex)
        {
            return keypointColors[keypointIndex];
        }

        public string GetKeypointName(int keypointIndex)
        {
            return keypointNames[keypointIndex];
        }

        public int GetNumberOfKeypoints()
        {
            return numberOfKeypoints;
        }
    }
}
