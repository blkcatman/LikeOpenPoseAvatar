using System;
using LikeOpenPoseAvatar.UI.Components;
using UnityEngine;

namespace LikeOpenPoseAvatar.UI.Rules
{
    public class DummyAvatarRule : IAvatarRule
    {
        public void SetScreenSize(float width, float height) {}

        public void SetOriginCamera(Camera camera) {}

        public void SetKeypointThickness(float value) {}

        public void SetJointThickness(float value) {}

        public Geometry[] GetGeometries(LikeOpenPoseAvatar avatar)
        {
            return Array.Empty<Geometry>();
        }
    }
}
