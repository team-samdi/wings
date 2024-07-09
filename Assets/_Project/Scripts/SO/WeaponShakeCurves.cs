using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Shake Curves", menuName = ("Project Wing/Weapon/Shake Curves"))]
public class WeaponShakeCurves : ScriptableObject {

	[Serializable]
	public struct Curve3 {
		public AnimationCurve x, y, z;
		[Space(5f)]
		public float amplitudeMultiplier;
	}

	public Curve3 positionCurve;
	[Space(5f)]
	public Curve3 rotationCurve;
	
}