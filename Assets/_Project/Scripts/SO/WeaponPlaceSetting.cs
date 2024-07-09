	using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations.Rigging;
using UnityEngine;
using UnityEngine.TestTools;

[CreateAssetMenu(fileName = "Weapon Place Setting", menuName = ("Project Wing/Weapon/Place Setting"))]
public class WeaponPlaceSetting : ScriptableObject {
	
	public Vector3 rotationOffset;
	[Space(10f)]
	public Vector3 hipPosition;
	public Vector3 adsPosition;
	

	[Serializable]
	public struct SwaySettings {
		public float inSpeed;	
		public float recoverySpeed;
		[Space(3f)]
		public float displaceAmount;
		public float tiltAmount;
	}

	[Serializable]
	public struct SwaySetting {
		public SwaySettings onRotation;
		public SwaySettings onMove;
	}
	[Space(5f)]
	public SwaySetting swaySetting;
	
	[Serializable]
	public struct Curve3 {
		public AnimationCurve x, y, z;
		[Space(5f)]
		public float amplitudeMultiplier;
	}

	[Serializable]
	public struct ShakeSetting {
		public WeaponShakeCurves referenceCurve;
		[Space(2f)]
		public Curve3 positionCurve;
		public Curve3 rotationCurve;

		public void CopyCurve() {
			
			positionCurve.x.keys = referenceCurve.positionCurve.x.keys;
			positionCurve.y.keys = referenceCurve.positionCurve.y.keys;
			positionCurve.z.keys = referenceCurve.positionCurve.z.keys;
			positionCurve.amplitudeMultiplier = referenceCurve.positionCurve.amplitudeMultiplier;
			
			rotationCurve.x.keys = referenceCurve.rotationCurve.x.keys;
			rotationCurve.y.keys = referenceCurve.rotationCurve.y.keys;
			rotationCurve.z.keys = referenceCurve.rotationCurve.z.keys;
			rotationCurve.amplitudeMultiplier = referenceCurve.rotationCurve.amplitudeMultiplier;

		}
		
		public void OverrideCurve() {
			
			referenceCurve.positionCurve.x.keys = positionCurve.x.keys;
			referenceCurve.positionCurve.y.keys = positionCurve.y.keys;
			referenceCurve.positionCurve.z.keys = positionCurve.z.keys;
			referenceCurve.positionCurve.amplitudeMultiplier = positionCurve.amplitudeMultiplier;
			
			referenceCurve.rotationCurve.x.keys = rotationCurve.x.keys;
			referenceCurve.rotationCurve.y.keys = rotationCurve.y.keys;
			referenceCurve.rotationCurve.z.keys = rotationCurve.z.keys;
			referenceCurve.rotationCurve.amplitudeMultiplier = rotationCurve.amplitudeMultiplier;

		}
	}
	public ShakeSetting shakeSetting;
}