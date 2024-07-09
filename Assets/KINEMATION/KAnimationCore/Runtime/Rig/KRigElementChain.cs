﻿// Designed by KINEMATION, 2024.

using KINEMATION.KAnimationCore.Runtime.Core;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
    [Serializable]
    public class KRigElementChain
    {
        public string chainName;
        [HideInInspector] public List<KRigElement> elementChain;
    }

    // A simplified version of the KRigElementChain, which contains transforms only.
    public class KTransformChain
    {
        public List<Transform> transformChain = new List<Transform>();
        public List<KTransform> cachedTransforms = new List<KTransform>();
        public ESpaceType spaceType;

        public void CacheTransforms(ESpaceType targetSpace, Transform root = null)
        {
            cachedTransforms.Clear();
            spaceType = targetSpace;
            
            foreach (var element in transformChain)
            {
                KTransform cache = new KTransform();
                
                if (targetSpace == ESpaceType.WorldSpace)
                {
                    cache.position = element.position;
                    cache.rotation = element.rotation;
                } 
                else if (targetSpace == ESpaceType.ComponentSpace)
                {
                    if (root == null)
                    {
                        root = element.root;
                    }
                    
                    cache.position = root.InverseTransformPoint(element.position);
                    cache.rotation = Quaternion.Inverse(root.rotation) * element.rotation;
                }
                else
                {
                    cache.position = element.localPosition;
                    cache.rotation = element.localRotation;
                }
                
                cachedTransforms.Add(cache);
            }
        }

        public void BlendTransforms(float weight)
        {
            int count = transformChain.Count;

            for (int i = 0; i < count; i++)
            {
                Transform element = transformChain[i];
                KTransform cache = cachedTransforms[i];

                KPose pose = new KPose()
                {
                    modifyMode = EModifyMode.Replace,
                    pose = cache,
                    space = spaceType
                };
                
                KAnimationMath.ModifyTransform(element.root, element, pose, weight);
            }
        }
    }
}