using UnityEngine;
using LikeOpenPoseAvatar.Keypoints;
using UnityEngine.UI;
using LikeOpenPoseAvatar.UI.Components;
using LikeOpenPoseAvatar.UI.Rules;
using UnityEngine.Serialization;

namespace LikeOpenPoseAvatar.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(RectTransform))]
    [ExecuteAlways]
    public class LikeOpenPoseGraph : Graphic
    {
        [SerializeField]
        private KeypointType supportedKeypointType;

        public KeypointType SupportedKeypointType
        {
            get => supportedKeypointType;
            set => supportedKeypointType = value;
        }

        [SerializeField]
        private LikeOpenPoseAvatar[] targetAvatars;

        [SerializeField]
        private Camera sourceCamera;
        
        [SerializeField]
        private float keypointThickness = 10f;

        [SerializeField]
        private float jointThickness = 5f;

        public LikeOpenPoseAvatar[] Avatars
        {
            set => targetAvatars = value;
            get => targetAvatars;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            var rect = rectTransform.rect;

            int indexOffset = 0;

            IAvatarRule avatarRule;
            switch (supportedKeypointType)
            {
                case KeypointType.Coco18:
                    avatarRule = new Coco18AvatarRule();
                    break;
                default:
                    avatarRule = new DummyAvatarRule();
                    break;
            }

            avatarRule.SetOriginCamera(sourceCamera);
            avatarRule.SetScreenSize(rect.width, rect.height);
            avatarRule.SetKeypointThickness(keypointThickness);
            avatarRule.SetJointThickness(jointThickness);

            foreach (var avatar in targetAvatars)
            {
                if (avatar == null) continue;

                if (avatar.KeypointType == supportedKeypointType)
                {
                    var geometries = avatarRule.GetGeometries(avatar);
                    foreach (var geometry in geometries)
                    {
                        indexOffset = Geometry.Set(geometry, vh, indexOffset);
                    }
                }
            }
        }

        protected override void Start()
        {
            SetVerticesDirty();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetVerticesDirty();
        }

        protected void LateUpdate()
        {
            SetVerticesDirty();
        }
    }
}
