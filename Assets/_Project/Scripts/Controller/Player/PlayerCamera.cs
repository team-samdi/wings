using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PlayerCamera : MonoBehaviour {

    public Transform cam;

    [Serializable]
    public struct SpineRig {

        public Transform spine;
        public Transform dirxReference;
        [Space(5f)]
        public MultiRotationConstraint rig;
        public Vector3 rigOffset;

        public void SetUp() {

            rig.data.constrainedObject = spine;

            var data = rig.data.sourceObjects;
            data = new WeightedTransformArray(1);
            data.SetTransform(0, dirxReference);
            data.SetWeight(0, 1);
            rig.data.sourceObjects = data;

            rig.data.offset = rigOffset;
        }
    }
    [Space(5f)] public SpineRig spineRig;
    // use from PlayerCameraEditor
    
    
    [Serializable]
    public struct HeadRig {

        public Transform head;
        public Transform dirxReference;
        [Space(5f)]
        public OverrideTransform rig;
        
        public void SetUp() {

            rig.data.constrainedObject = head;

            var data = rig.data.sourceObject;
            rig.data.sourceObject = dirxReference;
            
        }
    }
    [Space(5f)] public HeadRig headRig;
    // use from CameraControlEditor


    
    [Header("Setting")]
    [SerializeField] public CameraSetting set;

    [Space(20f)]
    [SerializeField] private float rotationMultiplier;
    [SerializeField] private bool showGizmos;




    private Vector3 targetRotation;
    private Quaternion currentRotation;

    private Vector3 camLocalPosition;
    
    private Vector3 tiltAngle;
    private Vector3 twistAngle;

    private Vector3 camStaticPosition;
    
    [Space(10f)]
    public UnityEvent camIsPlaced;

    
    
    private PlayerMove moveCtrl;



    public void GetRotation(out Quaternion _currentRotation, out Quaternion _targetRotation) {
        
        _currentRotation = currentRotation;
        _targetRotation = Quaternion.Euler(targetRotation);
    }

    public Vector3 RotationInput {

        get => new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X")) * rotationMultiplier;
    }
    

    public Vector3 CamStaticPosition {
        get => camStaticPosition;
    }




    void Awake() {

        targetRotation = transform.eulerAngles;

        // CameraArmRealize SetUp
        camLocalPosition = cam.position - headRig.head.position;

        moveCtrl = GetComponent<PlayerMove>();
    }






    void Update() {
        
        targetRotation += RotationInput;

        targetRotation.x = Mathf.Clamp(targetRotation.x, set.angleLimitAbove, set.angleLimitBelow);

        var tParam = Time.unscaledDeltaTime * set.lerpSpeed;
        currentRotation = Quaternion.Slerp(currentRotation, Quaternion.Euler(targetRotation), tParam);

        
        var deltaAngle = Mathf.DeltaAngle(currentRotation.eulerAngles.y, targetRotation.y);
        var outAmount = Mathf.Abs(deltaAngle) - set.lerpMaxDistance;
        if (outAmount > 0)
            currentRotation.eulerAngles += new Vector3(0, (outAmount) * Mathf.Sign(deltaAngle), 0);
        // targetRotation과 currentRotation의 각도가 일정 값 이상 커지지 않게 한다
        // 한계각도를 벗어난 만큼 각을 다시 더해주는 원리



        
        MakeSwayData(out var swayedAngle);
        MakeShakeData(out var shakenPosition, out var shakenAngle);

        cam.rotation = currentRotation 
                       * Quaternion.Euler(shakenAngle)
                       * Quaternion.Euler(swayedAngle);

        
        transform.eulerAngles = new Vector3(0, targetRotation.y, 0);
        spineRig.dirxReference.rotation = Quaternion.Euler(targetRotation);

        camStaticPosition = headRig.head.TransformPoint(camLocalPosition);
        cam.position = camStaticPosition
                       + (transform.rotation * shakenPosition);

        camIsPlaced.Invoke();

    }
    


    void OnAnimatorIK(int layerIndex) {
        
        MakeShakeData(out var shakenPosition, out _);
        
        camStaticPosition = headRig.head.TransformPoint(camLocalPosition);
        cam.position = camStaticPosition
                       + (transform.rotation * shakenPosition);


        
        camIsPlaced.Invoke();
    }
    
    
    
    
    
    void MakeShakeData(out Vector3 shakenPosition, out Vector3 shakenAngle) {

        moveCtrl.GetShakeData(out float shakeIndex, out float shakeMultiplier);

        
        Vector3 shakingPosition, shakingAngle;
        
        var curves = set.shakeInfo.positionCurve;
        shakingPosition.x = curves.x.Evaluate(shakeIndex);
        shakingPosition.y = curves.y.Evaluate(shakeIndex);
        shakingPosition.z = curves.z.Evaluate(shakeIndex);
        shakingPosition *= curves.amplitudeMultiplier;

        curves = set.shakeInfo.rotationCurve;
        shakingAngle.x = curves.x.Evaluate(shakeIndex);
        shakingAngle.y = curves.y.Evaluate(shakeIndex);
        shakingAngle.z = curves.z.Evaluate(shakeIndex);
        shakingAngle *= curves.amplitudeMultiplier;
        
        
        shakenPosition = shakingPosition * shakeMultiplier;
        shakenAngle = shakingAngle * shakeMultiplier;
    }
    
    
    void MakeSwayData(out Vector3 swayedAngle) {

        
        moveCtrl.GetInput(out var xInput, out var zInput, out _);
        var data = set.swayInfo;
        


        var lerpT = Time.deltaTime * data.tiltSpeed;
        
        if (moveCtrl.IsRun) {
            tiltAngle.x = Mathf.Lerp(tiltAngle.x, data.tiltAngleOnRun, lerpT);
        } else tiltAngle.x = Mathf.Lerp(tiltAngle.x, 0, lerpT);
        
        if (xInput != 0) {
            tiltAngle.z = Mathf.Lerp(tiltAngle.z, data.tiltAngleOnHorizMove * -xInput, lerpT);
        } else tiltAngle.z = Mathf.Lerp(tiltAngle.z, 0, lerpT);

        

        lerpT = Time.deltaTime * data.twistSpeed;
        
        if (Input.GetAxisRaw("Mouse X") != 0) {
            twistAngle.z = Mathf.Lerp(twistAngle.z, 
                data.twistAngle * Mathf.Clamp(Input.GetAxisRaw("Mouse X"), -2, 2), lerpT);
        } twistAngle.z = Mathf.Lerp(twistAngle.z, 0, lerpT);



        swayedAngle = tiltAngle + twistAngle;
    }
    
    
    
    


    void OnDrawGizmos() {

        if (showGizmos) {

            Gizmos.color = Color.white;
            Gizmos.DrawRay(cam.position, cam.forward * 100);
            
            Gizmos.DrawSphere(headRig.head.position, 0.01f);
            Gizmos.DrawSphere(cam.position, 0.01f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(headRig.head.position, cam.position);
        }
    }
}