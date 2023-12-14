using System.Linq;
using System.Collections.Generic;
using LikeOpenPoseAvatar.Keypoints;
using LikeOpenPoseAvatar.Generators;
using LikeOpenPoseAvatar.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Toggle = UnityEngine.UIElements.Toggle;

namespace LikeOpenPoseAvatar.Editor
{
    public class LikeOpenPoseAvatarGenerator : EditorWindow
    {
        private ObjectField animatorSelector;
        private EnumField keypointTypeSelector;
        private Toggle registerAvatarToggle;

        public void CreateGUI()
        {
            rootVisualElement.Add(new HelpBox()
            {
                text = "Select Animator which includes Humanoid Avatar, and press \"Generate\".",
            });
            animatorSelector = new ObjectField("Animator");
            animatorSelector.objectType = typeof(Animator);
            rootVisualElement.Add(animatorSelector);
            keypointTypeSelector = new EnumField("Keypoint Type", KeypointType.Coco18);
            rootVisualElement.Add(keypointTypeSelector);
            registerAvatarToggle = new Toggle("Register Avatar to Camera");
            registerAvatarToggle.value = true;
            rootVisualElement.Add(registerAvatarToggle);
            
            var generateButton = new Button(() => OnGenerateAvatar())
            {
                text = "Generate"
            };
            rootVisualElement.Add(generateButton);
        }
    
        [MenuItem("Window/LikeOpenPose/AvatarGenerator")]
        public static void ShowAvatarGenerator()
        {
            EditorWindow window = GetWindow<LikeOpenPoseAvatarGenerator>();
            window.titleContent = new GUIContent("AvatarGenerator");
        }

        private void OnGenerateAvatar()
        {
            var animator = animatorSelector.value as Animator;
            if (animator != null)
            {
                var type = (KeypointType)keypointTypeSelector.value;
                if (type == KeypointType.Coco18)
                {
                    var generator = new Coco18AvatarGenerator();
                    var avatar = generator.Generate(animator);

                    var isAutoRegisterAvatar = registerAvatarToggle.value;

                    if (isAutoRegisterAvatar)
                    {
                        var graphs = FindObjectsOfType<LikeOpenPoseGraph>();
                        var graph = graphs.FirstOrDefault(g =>
                            g.SupportedKeypointType == avatar.KeypointType);
                        if (graph != null)
                        {
                            List<LikeOpenPoseAvatar> avatars = new List<LikeOpenPoseAvatar>(graph.Avatars);
                            avatars.Add(avatar);
                            graph.Avatars = avatars.ToArray();
                        }
                        else
                        {
                            Debug.LogWarning("No cameras found that contain LikeOpenPoseGraph.");
                        }
                    }
                }
            }
        }
    }
}
