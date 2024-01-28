using LikeOpenPoseAvatar.Keypoints;
using UnityEngine;

namespace LikeOpenPoseAvatar
{
    [ExecuteAlways]
    public class LikeOpenPoseAvatar : MonoBehaviour
    {
        [SerializeField]
        private KeypointType keypointType;

        public KeypointType KeypointType
        {
            get => keypointType;
            set => keypointType = value;
        }

        [SerializeField]
        private LikeOpenPoseKeypoint[] keypoints;

        public LikeOpenPoseKeypoint[] Keypoints
        {
            get => keypoints;
            set => keypoints = value;
        }
        
        [SerializeField]
        private LikeOpenPoseBone[] bones;

        public LikeOpenPoseBone[] Bones
        {
            get => bones;
            set => bones = value;
        }

        [SerializeField]
        private float gizmoSize = 0.02f;

        private float currentGizmoSize;

        private void Start()
        {
            UpdateGizmoSize();
        }
        
        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (!Mathf.Approximately(currentGizmoSize, gizmoSize))
            {
                UpdateGizmoSize();
            }
#endif
        }

        private void UpdateGizmoSize()
        {
            foreach (var keypoint in keypoints)
            {
                if (keypoint != null)
                {
                    keypoint.GizmoSize = gizmoSize;
                }
            }
            currentGizmoSize = gizmoSize;
        }
    }
}
