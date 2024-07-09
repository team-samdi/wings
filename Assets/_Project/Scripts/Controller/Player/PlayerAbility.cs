using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;
using LensDistortion = UnityEngine.Rendering.HighDefinition.LensDistortion;

public class PlayerAbility : MonoBehaviour {

    [Serializable]
    private struct FlashSetting {

        public Volume volume;
        [Space(10f)]
        public AnimationCurve weightCurve;
        public float flashDistance;
        public float flashTime;
    }
    [SerializeField] private FlashSetting flashSetting;
    
    private bool isFlashing;
    private float flashIndex;
    private Vector3 flashOldPosition;
    private Vector3 flashTargetPosition;
    private Vector2 flashDirx;



    private PlayerCamera camCtrl;
    private PlayerMove moveCtrl;
    private PlayerVolume volumeCtrl;


    void Awake() {

        camCtrl = GetComponent<PlayerCamera>();
        moveCtrl = GetComponent<PlayerMove>();
        volumeCtrl = GetComponent<PlayerVolume>();
    }
    
    
    
    void Update() {

            
        Flash();

    }

    
    
   
    void Flash() {
        
        moveCtrl.GetInput(out float xInput, out float zInput, out _);
        camCtrl.GetRotation(out var currentRotation, out _);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!isFlashing) {

                if (new Vector2(xInput, zInput).magnitude == 0) return;
                
                flashOldPosition = transform.position;
                flashTargetPosition = transform.position + transform.rotation 
                                 * (new Vector3(xInput, 0, zInput) * flashSetting.flashDistance);
                flashDirx = new Vector2(xInput, zInput).normalized;

                isFlashing = true;
            }
        }

        if (isFlashing) {

            flashIndex += Time.deltaTime / flashSetting.flashTime;
            if (flashIndex >= 1) {

                isFlashing = false;
                flashIndex = 0;
                return;
            }
            
            transform.position = Vector3.Lerp(flashOldPosition, flashTargetPosition, flashIndex);
            
            
            Vector2 distortionCenter = new Vector2(0.5f, 0.5f);
            if (flashDirx.y < 0) {
                distortionCenter.x -= (flashDirx.x * 0.3f); // 0.5 +- 0.5 (0.2~0.8)
            } else distortionCenter.x += (flashDirx.x * 0.3f); // 0.5 +- 0.5 (0.2~0.8)
            
            float xAngle = Mathf.DeltaAngle(0, currentRotation.eulerAngles.x);
            distortionCenter.y = 0.5f + ( (xAngle / 90) * 0.5f);

            flashSetting.volume.profile.TryGet(out LensDistortion lensDist);
            lensDist.center.value = distortionCenter;
            flashSetting.volume.weight = flashSetting.weightCurve.Evaluate(flashIndex);

            
            
        } else {
            
            flashSetting.volume.weight = 0;

        }

    }
}
