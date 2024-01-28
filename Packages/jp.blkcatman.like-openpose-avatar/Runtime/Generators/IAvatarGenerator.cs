using UnityEngine;

namespace LikeOpenPoseAvatar.Generators
{
    public interface IAvatarGenerator
    {
        LikeOpenPoseAvatar Generate(Animator animator);
    }
}
