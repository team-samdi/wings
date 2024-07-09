using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CustomEditor(typeof(WeaponPlaceSetting))]
public class WeaponPlaceSettingEditor : Editor {

	private WeaponPlaceSetting tg;

	private bool isClickCopy;
	private bool isClickOverride;
	
	void OnEnable() {

		tg = target as WeaponPlaceSetting;
	}

	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		
		EditorGUILayout.Space(10f);
		if (GUILayout.Button("Copy Setting")) {

			isClickOverride = false;
			isClickCopy = !isClickCopy;
		}
		
		if (GUILayout.Button("Override Setting")) {
			
			isClickCopy = false;
			isClickOverride = !isClickOverride;
		}

		
		
		EditorGUILayout.Space(10f);
		if (isClickCopy) {
			if (GUILayout.Button("Are you sure to copy?")) {

				isClickCopy = false;
				tg.shakeSetting.CopyCurve();
			}
		}
		
		if (isClickOverride) {
			if (GUILayout.Button("Are you sure to override?")) {

				isClickOverride = false;
				tg.shakeSetting.OverrideCurve();
			}
		}

	}
}
