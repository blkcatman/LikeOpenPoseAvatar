using System.IO;
using System.Linq;
using LikeOpenPoseAvatar.Keypoints;
using LikeOpenPoseAvatar.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using LikeOpenPoseAvatar.Utilities;

namespace LikeOpenPoseAvatar.Editor
{
    public class LikeOpenPoseCameraSetup : EditorWindow
    {
        private ObjectField cameraSelector;
        private EnumField displaySelector;
        private EnumField keypointTypeSelector;
        private Toggle copyMaterialToggle;
        
        private string layerName = "LikeOpenPoseAvatar";
        
        public void CreateGUI()
        {
            rootVisualElement.Add(new HelpBox()
            {
                text = "Select Main Camera, and press \"Create\".",
            });
            cameraSelector = new ObjectField("Target Camera")
            {
                objectType = typeof(Camera)
            };
            rootVisualElement.Add(cameraSelector);

            displaySelector = new EnumField("Target Display", TargetDisplay.Display2);
            rootVisualElement.Add(displaySelector);
            keypointTypeSelector = new EnumField("Keypoint Type", KeypointType.Coco18);
            rootVisualElement.Add(keypointTypeSelector);
            copyMaterialToggle = new Toggle("Copy Material");
            copyMaterialToggle.value = true;
            rootVisualElement.Add(copyMaterialToggle);
            
            var generateButton = new Button(() => OnCreatePrefab())
            {
                text = "Create"
            };
            rootVisualElement.Add(generateButton);
        }
        
        [MenuItem("Window/LikeOpenPose/CameraSetup")]
        public static void ShowCameraSetup()
        {
            EditorWindow window = GetWindow<LikeOpenPoseCameraSetup>();
            window.titleContent = new GUIContent("CameraSetup");
        }
        
        private void OnCreatePrefab()
        {
            var cameraComponent = cameraSelector.value as Camera;
            if (cameraComponent != null)
            {
                CreateTagIfRequired();
                
                var guids = AssetDatabase.FindAssets(
                    "LikeOpenPoseCamera t:GameObject",
                    new[] { "Packages" });

                if (guids.Length > 0)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                    GameObject go = Instantiate(prefab, cameraComponent.transform, false) as GameObject;
                    if (go != null)
                    {
                        go.name = "LikeOpenPoseCamera";
                        var camera = go.GetComponent<Camera>();
                        var layer = LayerMask.NameToLayer(layerName);
                        camera.cullingMask = 1 << layer;
                        SetLayerWithChildren(go, layer);
                        var targetDisplay = (TargetDisplay)displaySelector.value;
                        camera.targetDisplay = (int)targetDisplay;

                        camera.fieldOfView = cameraComponent.fieldOfView;
                        camera.nearClipPlane = cameraComponent.nearClipPlane;
                        camera.farClipPlane = cameraComponent.farClipPlane;
                        camera.orthographic = cameraComponent.orthographic;

                        var graph = go.GetComponentInChildren<LikeOpenPoseGraph>();
                        if (graph != null)
                        {
                            var requiredCopyMaterial = copyMaterialToggle.value;
                            if (requiredCopyMaterial)
                            {
                                if(TryCopyMaterial(
                                       graph.material,
                                       graph.material.name,
                                       out var copiedMaterial))
                                {
                                    graph.material = copiedMaterial;
                                }
                            }

                            var keypointType = (KeypointType)keypointTypeSelector.value;
                            graph.SupportedKeypointType = keypointType;
                            var avatars = FindObjectsOfType<LikeOpenPoseAvatar>();
                            if (avatars != null)
                            {
                                var supportedAvatars = avatars.Where(
                                    a => a.KeypointType == keypointType);
                                graph.Avatars = supportedAvatars.ToArray();
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("prefab \"LikeOpenPoseCamera\" not found in \"Package\" Directory.");
                }
            }
        }

        private void CreateTagIfRequired()
        {
            var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty it = tagManager.GetIterator();
            int minimumUserLayerNum = 6;
            bool requiredApply = false;
            SerializedProperty layerProperty = tagManager.FindProperty("layers");
            if (layerProperty != null)
            {
                var layers =
                    Enumerable.Range(0,32).Select(i=> layerProperty.GetArrayElementAtIndex(i)).ToList();
                
                if (layers.Any(layer => layer.stringValue.Contains(layerName)))
                {
                    return;
                }
                
                for (int i = minimumUserLayerNum; i < 32; i++)
                {
                    if (string.IsNullOrEmpty(layers[i].stringValue))
                    {
                        layers[i].stringValue = layerName;
                        requiredApply = true;
                        break;
                    }
                }
            }

            if (requiredApply)
            {
                tagManager.ApplyModifiedProperties();
            }
        }

        private bool TryCopyMaterial(Material original, string materialName, out Material copied)
        {
            var guids = AssetDatabase.FindAssets(
                $"{materialName} t:Material",
                new[] { "Assets/LikeOpenPoseAvatar/Materials" });

            if (guids.Length > 0)
            {
                Debug.Log($"Material {materialName} already exists in Assets/LikeOpenPoseAvatar/Materials.");

                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                copied = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
                return true;
            }
            else
            {
                if (!Directory.Exists("Assets/LikeOpenPoseAvatar"))
                {
                    AssetDatabase.CreateFolder("Assets", "LikeOpenPoseAvatar");
                }

                if (!Directory.Exists("Assets/LikeOpenPoseAvatar/Materials"))
                {
                    AssetDatabase.CreateFolder("Assets/LikeOpenPoseAvatar", "Materials");
                }

                var copiedMaterial = Instantiate(original);

                AssetDatabase.CreateAsset(copiedMaterial, $"Assets/LikeOpenPoseAvatar/Materials/{materialName}.mat");

                Debug.Log($"Material {materialName} has been copied in Assets/LikeOpenPoseAvatar/Materials.");

                guids = AssetDatabase.FindAssets(
                    $"{materialName} t:Material",
                    new[] { "Assets/LikeOpenPoseAvatar/Materials" });

                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                copied = AssetDatabase.LoadAssetAtPath(path, typeof(Material)) as Material;
                return true;
            }
        }

        private void SetLayerWithChildren(GameObject go, int layer)
        {
            if (go == null)
            {
                return;
            }

            go.layer = layer;

            var childCount = go.transform.childCount;
            if (childCount < 1)
            {
                return;
            }

            for (int i = 0; i < childCount; i++)
            {
                var child = go.transform.GetChild(i).gameObject;
                SetLayerWithChildren(child, layer);
            }
        }
    }
}
