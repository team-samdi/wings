using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager> {
    
    [SerializeField] private GameObject currentWeapon;

    public GameObject CurrentWeapon {
        get => currentWeapon;
    }

    private int curSlot;
    void Start() {
        
        
    }
    
    
    
    void Update() {
        
    }
}
