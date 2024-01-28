using System.Collections.Generic;
using UnityEngine;
using LikeOpenPoseAvatar;
using LikeOpenPoseAvatar.UI;
using LikeOpenPoseAvatar.UI.Components;

namespace LikeOpenPoseAvatar.UI.Rules
{
    public interface IAvatarRule
    {
        void SetScreenSize(float width, float height);
        void SetOriginCamera(Camera camera);
        void SetKeypointThickness(float value);
        void SetJointThickness(float value);
        Geometry[] GetGeometries(LikeOpenPoseAvatar avatar);
    }
}
