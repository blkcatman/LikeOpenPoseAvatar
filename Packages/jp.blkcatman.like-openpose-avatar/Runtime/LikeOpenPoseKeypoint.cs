using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using LikeOpenPoseAvatar.Keypoints;

namespace LikeOpenPoseAvatar
{
    [ExecuteAlways]
    public class LikeOpenPoseKeypoint : MonoBehaviour
    {
        [SerializeField]
        private Transform parentTransform;

        public Transform ParentTransform
        {
            get => parentTransform;
            set => parentTransform = value;
        }

        [SerializeField, HideInInspector]
        private int keypointIndex;

        public int KeypointIndex
        {
            get => keypointIndex;
            set => keypointIndex = value;
        }

        [SerializeField, HideInInspector]
        private Color keypointColor = Color.black;

        public Color KeypointColor
        {
            get => keypointColor;
            set => keypointColor = value;
        }

        [SerializeField]
        private Vector3 offset;

        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        }
        
        [SerializeField, HideInInspector]
        private float gizmoSize = 0.02f;

        public float GizmoSize
        {
            get => gizmoSize;
            set => gizmoSize = value;
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                transform.SetParent(parentTransform, true);
            }
        }

        private void LateUpdate()
        {
    #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (!IsSameVector(transform.localPosition, offset))
                {
                    offset = transform.localPosition;
                }
            }
    #endif
        }

    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = keypointColor;
            var t = transform;
            Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation,t.localScale);
            Gizmos.DrawSphere(Vector3.zero, gizmoSize);
        }
    #endif
        
        private bool IsSameVector(Vector3 src1, Vector3 src2)
        {
            return Mathf.Approximately(src1.x, src2.x) &&
                   Mathf.Approximately(src1.y, src2.y) &&
                   Mathf.Approximately(src1.z, src2.z);
        }
    }
}
