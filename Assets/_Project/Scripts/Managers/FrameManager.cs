using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FrameManager : MonoBehaviour {

    
    [Range(30, 220)]
    public int frame;
    
    void Awake() {
        
        Application.targetFrameRate = frame;
        
    }
}
