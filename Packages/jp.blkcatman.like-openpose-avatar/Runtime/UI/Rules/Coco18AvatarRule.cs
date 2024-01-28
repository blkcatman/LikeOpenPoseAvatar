using System.Collections.Generic;
using System.Linq;
using LikeOpenPoseAvatar.UI.Components;
using UnityEngine;

namespace LikeOpenPoseAvatar.UI.Rules
{
    public class Coco18AvatarRule : IAvatarRule
    {
        private Camera originCamera;
        private float screenWidth;
        private float screenHeight;

        private float keypointThickness;
        private float jointThickness;

        public void SetScreenSize(float width, float height)
        {
            screenWidth = width;
            screenHeight = height;
        }

        public void SetOriginCamera(Camera camera)
        {
            originCamera = camera;
        }

        public void SetKeypointThickness(float value)
        {
            keypointThickness = value;
        }

        public void SetJointThickness(float value)
        {
            jointThickness = value;
        }

        public Geometry[] GetGeometries(LikeOpenPoseAvatar avatar)
        {
            List<Geometry> geometries = new List<Geometry>();
            IShapePlotter circlePlotter = new CirclePlotter();
            ILinePlotter linePlotter = new UniformLinePlotter();
            
            var keypoints = avatar.Keypoints;
            var bones = avatar.Bones;
            var ct = originCamera.transform;

            int numberOfColorsInHorizontal = 16;

            float distanceBetweenColors = 1f / (float)numberOfColorsInHorizontal;
            float coordOffset = distanceBetweenColors / 2f;

            for (int i = 0; i < keypoints.Length; i++)
            {
                var position = keypoints[i].transform.position;
                var direction = ct.position - position;
                var screenPosition = originCamera.WorldToScreenPoint(position);
                var idx = keypoints[i].KeypointIndex;

                var coordPosX = distanceBetweenColors * (idx % numberOfColorsInHorizontal) + coordOffset;
                // ReSharper disable once PossibleLossOfFraction
                var coordPosY = 1.0f - (distanceBetweenColors * (idx / numberOfColorsInHorizontal) + coordOffset);

                circlePlotter.SetColorCoord(new Vector2(coordPosX, coordPosY));    
                var geometry = circlePlotter.DrawShape(
                    new Vector3(
                        screenPosition.x - (screenWidth/2f),
                        screenPosition.y - (screenHeight/2f),
                        direction.magnitude
                    ),
                    keypointThickness,
                    keypointThickness,
                    0f);
                
                geometries.Add(geometry);
            }
            
            for (int i = 0; i < bones.Length; i++)
            {
                var bone = bones[i];
                var keypoint0 = keypoints[bone.Keypoint0];
                var keypoint1 = keypoints[bone.Keypoint1];
                var idx = bone.BoneIndex;
                
                var position0 = keypoint0.transform.position;
                var direction0 = ct.position - position0;
                var position1 = keypoint1.transform.position;
                var direction1 = ct.position - position1;
                
                var screenPosition0 = originCamera.WorldToScreenPoint(position0);
                var screenPosition1 = originCamera.WorldToScreenPoint(position1);

                var coordPosX = distanceBetweenColors * (idx % numberOfColorsInHorizontal) + coordOffset;
                // ReSharper disable once PossibleLossOfFraction
                var coordPosY = 0.5f - (distanceBetweenColors * (idx / numberOfColorsInHorizontal) + coordOffset);

                linePlotter.SetColorCoord(new Vector2(coordPosX, coordPosY));
                var geometry = linePlotter.DrawLine(
                    new Vector3(
                        screenPosition0.x - (screenWidth / 2f),
                        screenPosition0.y - (screenHeight / 2f),
                        direction0.magnitude
                    ),
                    new Vector3(
                        screenPosition1.x - (screenWidth / 2f),
                        screenPosition1.y - (screenHeight / 2f),
                        direction1.magnitude
                    ),
                    jointThickness
                );

                geometries.Add(geometry);
            }

            return geometries.ToArray();
        }
    }
}
