using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CustomEditor(typeof(PlayerCamera))]
public class CameraControlEditor : Editor {

	private PlayerCamera stat;
	
	void OnEnable() {

		stat = target as PlayerCamera;
	}

	
	public override void OnInspectorGUI() {

		base.OnInspectorGUI();
		
		EditorGUILayout.Space(10f);

		if (GUILayout.Button("Setup Rigs")) {
			
			stat.spineRig.SetUp();
			stat.headRig.SetUp();
		}
	}
}
