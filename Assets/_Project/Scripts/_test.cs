using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public GameObject test;

    private float time;
    private bool isHoldLight;
    private bool isFlashOn;
    
    void Start() {
        
    }


    void Update() {

        if (Input.GetKeyDown(KeyCode.T)) {
            test.SetActive(true);
        }
        
        if (Input.GetKeyUp(KeyCode.T)) {
            test.SetActive(false);
        }
    }

    private void OnDrawGizmos() {
        
    }
}
