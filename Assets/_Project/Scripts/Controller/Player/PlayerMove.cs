using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float baseSpeed;
    [SerializeField] private float runMultiplier;
    [SerializeField] private float accelTime;
    [SerializeField] private float speedTransitionSpeed;
    [Space(10f)] 
    [SerializeField] private float shakeTime;
    [SerializeField] private float amplitudeMultiplierWhenRun;
    [SerializeField] private float frequencyMultiplierWhenRun;
    [SerializeField] private float recoveryTime;
    

    
    
    
    private Vector3 inputDirx;
    private Vector3 moveVel;

    private bool isRun;

    private float runWeight;

    private float speedMultiplier;
    private float curSpeed;

    [SerializeField, Range(0f, 1f)]
    private float shakeIndex;
    private float shakeMultiplier; // 0~1의 흔들림 Damping * 배수
    private bool isShaking;

    private bool isAim;
    
    
    
    private Rigidbody rigbody;
    
    

    void Awake() {

        rigbody = GetComponent<Rigidbody>();
        
    }
    
    
    
    
    

    public void GetInput(out float xInput, out float zInput, out bool isRunInput) {
        
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
        
        isRunInput = Input.GetKey(KeyCode.LeftShift);
    }
    

    public bool IsRun {
        get => isRun;
    }

    public float SpeedMultiplier {
        get => speedMultiplier;
        set => speedMultiplier = value;
    }

    public bool IsAim {
        set => isAim = value;
    }

    public Vector3 MoveVel {
        get => moveVel;
    }
    
    
    
    
    
    void Update() {
        
        GetInput(out float xInput, out float zInput, out bool isRunInput);
        
        

        inputDirx = transform.rotation * new Vector3(xInput, 0, zInput).normalized;

        if (inputDirx.magnitude != 0) {
            moveVel += inputDirx * Time.deltaTime / accelTime;
            moveVel = Vector3.ClampMagnitude(moveVel, 1);

        } else {
            moveVel = Vector3.Lerp(moveVel, Vector3.zero, Time.deltaTime / accelTime);
            if (moveVel.magnitude < 0.01) moveVel = Vector3.zero;
        }
        
        
        
        if (isRunInput & zInput > 0 & !isAim) {
            isRun = true;
        } else isRun = false;

        
        if (isRun) {
            runWeight += Time.deltaTime / accelTime;
        } else runWeight -= Time.deltaTime / accelTime;
        
        runWeight = Mathf.Clamp(runWeight, 0f, 1f);
        speedMultiplier += (runMultiplier - 1) * runWeight; 
            // 이번 프레임에 runWeight와 속도 스텟을 반영한 속도 추가값을 더한다.
            
        
        ShakeManaging();
    }
    
    

    void LateUpdate() {
        
//        moveVel.y = rigbody.velocity.y;
        curSpeed = Mathf.Lerp(curSpeed, baseSpeed * speedMultiplier, Time.deltaTime * speedTransitionSpeed);
        
        rigbody.velocity = moveVel * curSpeed;
        
        
        
        speedMultiplier = 1;
    }

    
    
    
    
    void ShakeManaging() {

        GetInput(out float xInput, out float zInput, out _);

        shakeIndex += Time.deltaTime / shakeTime
                      * (isRun ? frequencyMultiplierWhenRun : 1);
            
        if (shakeIndex > 1)
            shakeIndex -= 1;
        
        
        
        if (new Vector2(xInput, zInput).magnitude != 0) {
            shakeMultiplier += Time.deltaTime / recoveryTime;
        } else shakeMultiplier -= Time.deltaTime / recoveryTime;
        
        shakeMultiplier = Mathf.Clamp(shakeMultiplier, 0, 1);
        shakeMultiplier += (amplitudeMultiplierWhenRun - 1) * runWeight;
    }

    
    
    public void GetShakeData(out float _shakeIndex, out float _shakeMultiplier) {
        _shakeIndex = shakeIndex;
        _shakeMultiplier = shakeMultiplier;
    }   
    
    
    
}
