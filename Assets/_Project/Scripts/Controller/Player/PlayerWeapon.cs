using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {
    
    [SerializeField] private GameObject weapon; // 확인용 캐싱

    private float aimWeight; // 0~1
    private bool isAim;
    
    
    private WeaponPlacer wpPlacer;

    private Weapon wpData;
    private WeaponStatus wpStatus;
    private WeaponPlaceSetting wpPlaceSetting;

    private PlayerMove moveCtrl;
    
    

    void Awake() {

        wpPlacer = GetComponent<WeaponPlacer>();
        moveCtrl = GetComponent<PlayerMove>();
        
        SetupWeapon();
    }


    void SetupWeapon() {
        
        weapon = WeaponManager.Instance.CurrentWeapon;
        
        wpData = weapon.GetComponent<Weapon>();
        wpPlaceSetting = wpData.PlaceSetting;
        wpStatus = wpData.Status;
        
        wpPlacer.WeaponTransform = weapon.transform;
        wpPlacer.Setting = wpPlaceSetting;
        wpPlacer.HandSetting = wpData.HandSetting;
        wpPlacer.Status = wpStatus;

    }


    void Update() {

        if ( Input.GetMouseButton(1) ) {
            aimWeight = Mathf.Lerp(aimWeight, 1, Time.deltaTime / wpStatus.aimingTime);
        } else aimWeight = Mathf.Lerp(aimWeight, 0, Time.deltaTime / wpStatus.aimingTime);
        
        wpPlacer.AimWeight = aimWeight;


        if (aimWeight > 0.5f) {
            isAim = true;
        } else isAim = false;

        moveCtrl.IsAim = isAim;


        moveCtrl.SpeedMultiplier *= wpStatus.weaponSpeed;
        if (isAim) moveCtrl.SpeedMultiplier *= wpStatus.whileAdsSpeed;

    }

}
