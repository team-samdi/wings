using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlacer : MonoBehaviour {

    private Transform weapon;
    private WeaponPlaceSetting setting;
    private WeaponHandAttachSetting handSet;
    private WeaponStatus status;
    
    private Animator animator;
    private PlayerCamera camCtrl;
    private PlayerMove moveCtrl;

    private float aimWeight;

    
    public Transform WeaponTransform {
        set => weapon = value;
    }
    public WeaponPlaceSetting Setting {
        set => setting = value;
    }
    public WeaponHandAttachSetting HandSetting {
        set => handSet = value;
    }

    public WeaponStatus Status {
        set => status = value;
    }


    
    public float AimWeight {
        set => aimWeight = value;
    }
    
    
    
    void Awake() {
        
        camCtrl = GetComponent<PlayerCamera>();
        moveCtrl = GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
    }
    


    public void HoldWeapon() { // use in PlayerCamera
    
        camCtrl.GetRotation(out var currentRotation, out var targetRotation); 
        MakeSwayData(out var swayPosition, out var swayAngle);
        MakeShakeData(out var shakePosition, out var shakeAngle);
        
        swayPosition *= Mathf.Lerp(1, status.adsSwayDisplaceAmount, aimWeight);
        swayAngle *= Mathf.Lerp(1, status.adsSwayTiltAmount, aimWeight);
        shakePosition *= Mathf.Lerp(1, status.adsShakeAmount, aimWeight);
        shakeAngle *= Mathf.Lerp(1, status.adsShakeAmount, aimWeight);


        var hipPos = camCtrl.CamStaticPosition + (currentRotation * setting.hipPosition);
        var adsPos = camCtrl.CamStaticPosition + (currentRotation * setting.adsPosition);
        var wpPos = Vector3.Lerp(hipPos, adsPos, aimWeight);
        
        
        wpPos += weapon.rotation * (swayPosition + shakePosition);

        weapon.position = wpPos;
    
    
    
        weapon.rotation = targetRotation * (Quaternion.Euler(swayAngle + shakeAngle));
    }


    void OnAnimatorIK(int layerIndex) {
        HandAttach(); 
    }
    void HandAttach() {
        
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, 
            weapon.TransformPoint(handSet.attachData.handDataLeft.position));
        
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, 
            weapon.rotation * Quaternion.Euler(handSet.attachData.handDataLeft.angle));
        
        
        
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, 
            weapon.TransformPoint(handSet.attachData.handDataRight.position));
        
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotation(AvatarIKGoal.RightHand, 
            weapon.rotation * Quaternion.Euler(handSet.attachData.handDataRight.angle));
    }

    
    
    private Vector3 swayingPos, swayingAng;
    void MakeSwayData(out Vector3 swayPosition, out Vector3 swayAngle) {
        
        var set = setting.swaySetting.onMove;
        
        moveCtrl.GetInput(out float xInput, out float zInput, out bool isRunInput);
        if (xInput != 0) {
            swayingPos.x = Mathf.Lerp(swayingPos.x, set.displaceAmount * xInput, Time.deltaTime * set.inSpeed);
            swayingAng.z = Mathf.Lerp(swayingAng.z, set.tiltAmount * -xInput, Time.deltaTime * set.inSpeed);
            swayingAng.y = Mathf.Lerp(swayingAng.y, set.tiltAmount * -xInput, Time.deltaTime * set.inSpeed);
        } else {
            swayingPos.x = Mathf.Lerp(swayingPos.x, 0, Time.deltaTime * set.recoverySpeed);
            swayingAng.z = Mathf.Lerp(swayingAng.z, 0, Time.deltaTime * set.recoverySpeed);
            swayingAng.y = Mathf.Lerp(swayingAng.y, 0, Time.deltaTime * set.recoverySpeed);
        }
        
        if (zInput != 0) {
            swayingPos.z = Mathf.Lerp(swayingPos.z, set.displaceAmount * zInput, Time.deltaTime * set.inSpeed);
        } else swayingPos.z = Mathf.Lerp(swayingPos.z, 0, Time.deltaTime * set.recoverySpeed);



        set = setting.swaySetting.onRotation;
        
        var rotationInput = camCtrl.RotationInput.y;
        if (rotationInput != 0) {
            swayingPos.x += Mathf.Sign(rotationInput) * set.inSpeed * set.displaceAmount * Time.deltaTime;
            swayingPos.x = Mathf.Clamp(swayingPos.x, -set.displaceAmount, set.displaceAmount);
            
            swayingAng.z += -Mathf.Sign(rotationInput) * set.inSpeed * set.tiltAmount * Time.deltaTime;
            swayingAng.z = Mathf.Clamp(swayingAng.z, -set.tiltAmount, set.tiltAmount);
        } else {
            swayingPos.x = Mathf.Lerp(swayingPos.x, 0, Time.deltaTime * set.recoverySpeed);
            swayingAng.z = Mathf.Lerp(swayingAng.z, 0, Time.deltaTime * set.recoverySpeed);
        }
        
        
        
        swayPosition = swayingPos;
        swayAngle = swayingAng;
    }

    

    void MakeShakeData(out Vector3 shakePosition, out Vector3 shakeAngle) {
        
        moveCtrl.GetShakeData(out var shakeIndex, out var shakeMultiplier);
        
        Vector3 shakingPosition;
        Vector3 shakingAngle;

        var curves = setting.shakeSetting.positionCurve;
        shakingPosition.x = curves.x.Evaluate(shakeIndex);
        shakingPosition.y = curves.y.Evaluate(shakeIndex);
        shakingPosition.z = curves.z.Evaluate(shakeIndex);
        shakingPosition *= curves.amplitudeMultiplier;

        curves = setting.shakeSetting.rotationCurve;
        shakingAngle.x = curves.x.Evaluate(shakeIndex);
        shakingAngle.y = curves.y.Evaluate(shakeIndex);
        shakingAngle.z = curves.z.Evaluate(shakeIndex);
        shakingAngle *= curves.amplitudeMultiplier;


        shakePosition = shakingPosition * shakeMultiplier;
        shakeAngle = shakingAngle * shakeMultiplier;
    }
}
