using UnityEngine;
using System;

namespace LikeOpenPoseAvatar
{
    [Serializable]
    public struct LikeOpenPoseBone
    {
        [SerializeField]
        private int keypointIndex0;
        [SerializeField]
        private int keypointIndex1;

        [SerializeField]
        private int boneIndex;

        public int Keypoint0
        {
            get => keypointIndex0;
            set => keypointIndex0 = value;
        }

        public int Keypoint1
        {
            get => keypointIndex1;
            set => keypointIndex1 = value;
        }
        
        public int BoneIndex
        {
            get => boneIndex;
            set => boneIndex = value;
        }
    }
}
