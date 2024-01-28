using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LikeOpenPoseAvatar.Keypoints;

namespace LikeOpenPoseAvatar.Generators
{
    public class Coco18AvatarGenerator : IAvatarGenerator
    {
        private readonly HumanBodyBones[] preCullBones = new[]
        {
            HumanBodyBones.Head, //OpenPose Nose
            HumanBodyBones.Neck, //OpenPose Neck
            HumanBodyBones.RightUpperArm, // OpenPose RightShoulder
            HumanBodyBones.RightLowerArm, // OpenPose RightElbow
            HumanBodyBones.RightHand, // OpenPose RightWrist
            HumanBodyBones.LeftUpperArm, // OpenPose LeftShoulder
            HumanBodyBones.LeftLowerArm, // Openpose LeftElbow
            HumanBodyBones.LeftHand, // Openpose LeftWrist
            HumanBodyBones.RightUpperLeg, // OpenPose RightHip
            HumanBodyBones.RightLowerLeg, // OpenPose RightKnee
            HumanBodyBones.RightFoot, // OpenPose RightAnkle
            HumanBodyBones.LeftUpperLeg, // OpenPose LeftHip
            HumanBodyBones.LeftLowerLeg, // OpenPose LeftKnee
            HumanBodyBones.LeftFoot, // OpenPose LeftAnkle
            HumanBodyBones.RightEye, // OpenPose RightEye
            HumanBodyBones.LeftEye, // OpenPose leftEye
            HumanBodyBones.RightEye, // OpenPose RightEar
            HumanBodyBones.LeftEye // OpenPose LeftEar
        };

        private readonly LikeOpenPoseBone[] openPoseBones = new[]
        {
            new LikeOpenPoseBone { Keypoint0 = 1, Keypoint1 = 2, BoneIndex = 0 }, // RightShoulderBlade
            new LikeOpenPoseBone { Keypoint0 = 1, Keypoint1 = 5, BoneIndex = 1 }, // LeftShoulderBlade
            new LikeOpenPoseBone { Keypoint0 = 2, Keypoint1 = 3, BoneIndex = 2 }, // RightArm
            new LikeOpenPoseBone { Keypoint0 = 3, Keypoint1 = 4, BoneIndex = 3 }, // RightForearm
            new LikeOpenPoseBone { Keypoint0 = 5, Keypoint1 = 6, BoneIndex = 4 }, // LeftArm
            new LikeOpenPoseBone { Keypoint0 = 6, Keypoint1 = 7, BoneIndex = 5 }, // LeftForearm
            new LikeOpenPoseBone { Keypoint0 = 1, Keypoint1 = 8, BoneIndex = 6 }, // RightTorso
            new LikeOpenPoseBone { Keypoint0 = 8, Keypoint1 = 9, BoneIndex = 7 }, // RightUpperLeg
            new LikeOpenPoseBone { Keypoint0 = 9, Keypoint1 = 10, BoneIndex = 8 }, // RightLowerLeg
            new LikeOpenPoseBone { Keypoint0 = 1, Keypoint1 = 11, BoneIndex = 9 }, // LeftTorso
            new LikeOpenPoseBone { Keypoint0 = 11, Keypoint1 = 12, BoneIndex = 10 }, // LeftUpperLeg
            new LikeOpenPoseBone { Keypoint0 = 12, Keypoint1 = 13, BoneIndex = 11 }, // LeftLowerLeg
            new LikeOpenPoseBone { Keypoint0 = 1, Keypoint1 = 0, BoneIndex = 12 }, // Head
            new LikeOpenPoseBone { Keypoint0 = 0, Keypoint1 = 14, BoneIndex = 13 }, // RightEyebrow
            new LikeOpenPoseBone { Keypoint0 = 14, Keypoint1 = 16, BoneIndex = 14 }, // RightEar
            new LikeOpenPoseBone { Keypoint0 = 0, Keypoint1 = 15, BoneIndex = 15 }, // LeftEyebrow
            new LikeOpenPoseBone { Keypoint0 = 15, Keypoint1 = 17, BoneIndex = 16 }, // LeftEar
        };

        public LikeOpenPoseAvatar Generate(Animator animator)
        {
            if (!animator.avatar.isHuman)
            {
                throw new ArgumentException("Selected animator has not humanoid avatar.");
            }

            var keypointDataResolver = new Coco18KeypointDataResolver();
            var avatarName = $"LikeOpenPose Avatar";
            var avatarGameObject = new GameObject(avatarName);
            var keypointsGameObject = new GameObject("KeyPoints");
            keypointsGameObject.transform.SetParent(avatarGameObject.transform, true);
            
            LikeOpenPoseKeypoint[] keypoints = new LikeOpenPoseKeypoint[18];
            
            for (int i = 0; i < preCullBones.Length; i++)
            {
                var bone = preCullBones[i];
                var targetBone = animator.GetBoneTransform(bone);
                var keypointName = $"KeyPoint {keypointDataResolver.GetKeypointName(i)}";
                bool alternatives = false;
                if (targetBone == null)
                {
                    // If eyes transforms not found in avatar, set head transform instead.
                    if (i >= 14)
                    {
                        targetBone = animator.GetBoneTransform(HumanBodyBones.Head);
                        alternatives = true;
                    }
                }

                Vector3 position;
                if (alternatives)
                {
                    switch (i)
                    {
                        case 14: //RightEye index
                            position = targetBone.position + new Vector3(0.1f, 0.1f, 0f);
                            break;
                        case 15: //LeftEye index
                            position = targetBone.position + new Vector3(-0.1f, 0.1f, 0f);
                            break;
                        case 16: //RightEar index
                            position = targetBone.position + new Vector3(0.2f, 0.1f, 0f);
                            break;
                        case 17: //LeftEar index
                            position = targetBone.position + new Vector3(-0.2f, 0.1f, 0f);
                            break;
                        default:
                            position = targetBone.position;
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 16: //RightEar index
                            position = targetBone.position + new Vector3(0.1f, 0.1f, 0f);
                            break;
                        case 17: //LeftEar index
                            position = targetBone.position + new Vector3(-0.1f, 0.1f, 0f);
                            break;
                        default:
                            position = targetBone.position;
                            break;
                    }
                }
                
                var go = new GameObject
                {
                    name = keypointName,
                    transform =
                    {
                        position = position
                    }
                };
                var keypoint = go.AddComponent<LikeOpenPoseKeypoint>();
                keypoint.KeypointIndex = i;
                keypoint.KeypointColor = keypointDataResolver.GetKeypointColor(i);
                keypoint.ParentTransform = targetBone;
                keypoint.name = $"KeyPoint {keypointDataResolver.GetKeypointName(i)}";
                if (i == 0 || i == 14 || i == 15)
                {
                    var offset = new Vector3(0f, 0f, 0.1f);
                    keypoint.Offset = offset;
                    go.transform.localPosition += offset;
                }
                go.transform.SetParent(keypointsGameObject.transform, true);
                keypoints[i] = keypoint;
            }
            var openPoseAvatar = avatarGameObject.AddComponent<LikeOpenPoseAvatar>();
            openPoseAvatar.KeypointType = KeypointType.Coco18;
            openPoseAvatar.Keypoints = keypoints;
            openPoseAvatar.Bones = openPoseBones;

            return openPoseAvatar;
        }
    }
}
