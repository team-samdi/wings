	using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Hand Attach Setting", menuName = ("Project Wing/Weapon/Hand Attach Setting"))]
public class WeaponHandAttachSetting : ScriptableObject {
	
	
	[Serializable]
	public struct HandTransform {
		public Vector3 position;
		public Vector3 angle;
	}
	
	[Serializable]
	public struct AttachData {
		public HandTransform handDataLeft;
		[Space(10f)]
		public HandTransform handDataRight;
	}
	public AttachData attachData;

	
	
	[Serializable]
	public struct HintData {
		public Vector3 hintPositionLeft;
		[Space(3f)]
		public Vector3 hintPositionRight;
	}
	[Space(10f)]
	public HintData hintData;


}