using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "Camera Setting", menuName = ("Project Wing/Camera Setting"))]
public class CameraSetting : ScriptableObject {
	
	[Space(10f)]
	public float lerpSpeed;
	[Range(30f, 90f)] public float lerpMaxDistance;
	[Space(15f)]
	[Range(0f, -90f)] public float angleLimitAbove;
	[Range(0f, 90f)] public float angleLimitBelow;

	[Serializable]
	public struct SwayInfo {
		
		public float tiltAngleOnRun;
		public float tiltAngleOnHorizMove;
		public float tiltSpeed;
		[Space(15f)]
		public float twistAngle;
		public float twistSpeed;
	}
	[Space(15f)]
	public SwayInfo swayInfo;
	

	[Serializable]
	public struct Curve3 {
		public AnimationCurve x, y, z;
		public float amplitudeMultiplier;
	}
	[Serializable]
	public struct ShakeInfo {
		
		public Curve3 positionCurve;
		[Space(5f)]
		public Curve3 rotationCurve;
	}
	[Space(15f)]
	public ShakeInfo shakeInfo;
}

