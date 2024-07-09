using System;
using System.Collections;
using System.Collections.Generic;
using KINEMATION.FPSAnimationFramework.Runtime.Core;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour {
    
    private enum GameStates {
        Null,
        InGame,
        OutGame
    }
    [SerializeField]
    private GameStates gameStates;

    
    [Serializable]
    public struct CameraTypes {
        public GameObject playerCamera;
        public GameObject menuCamera;
    }
    [SerializeField] private CameraTypes cameraTypes;

    public GameObject GetCurrentCamera {

        get {
            switch (gameStates) {
                case GameStates.InGame:
                    return cameraTypes.playerCamera;
                
                case GameStates.OutGame:
                    return cameraTypes.menuCamera;
            }

            return null;
        }
    }
    
    



    void Awake() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start() {

        gameStates = GameStates.InGame; // 임시
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.G)) {
            Time.timeScale = 0.01f;
            Time.fixedDeltaTime *= 0.01f;
        }

        if (Input.GetKeyUp(KeyCode.G)) {
            Time.timeScale = 1f;
            Time.fixedDeltaTime *= 1f;
        };
    }
    
    
}
