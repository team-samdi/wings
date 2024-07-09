//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Animations.Rigging;
//
//public class LookManager : MonoBehaviour {
//
//	[Header("Rig Build")]
//	[SerializeField]
//	private MultiRotationConstraint rig;
//	[SerializeField]
//	private Transform camArmBone;
//	
//	[Space(5f)]
//	
//	[Header("Camera Build")]
//    [SerializeField]
//    private Transform cam;
//	[SerializeField, Range(0f, 90f)]
//	private float maxAngleAbove;
//	[SerializeField, Range(0f, 90f)]
//	private float maxAngleBelow;
//	[SerializeField]
//	private bool isBodyCopyCam;
//	
//	[Space(5f)]
//		
//	[Header("Status")]
//    [SerializeField, Range(0f, 10f)]
//    private float mouseSensitive;
//
//	[Space(15f)]
//	
//	[SerializeField]
//	private bool showGizmos;
//
//	
//	
//	private Vector3 camLocalPoint;
//	private Vector3 camAngle;
//
//    private Animator anmt;
//    
//    public float recoilAngle;
//
//
//
//
//
//    void Awake() {
//	    
//	    gameObject.SetActive(false);
//	    gameObject.SetActive(true);
//    }
//    
//    void OnEnable() {
//
//	    camLocalPoint = cam.position - camArmBone.position;
//	    
//	    
//	    SetUpRig();
//    }
//    
//    void LateUpdate() {
//	    
//	    var camMoveDirx = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"));
//	    camAngle = cam.eulerAngles;
//
//	    camAngle += camMoveDirx * mouseSensitive;
//	    
//	    if (camAngle.x > 180) {
//		    camAngle.x = Mathf.Clamp(camAngle.x, 360f-maxAngleAbove, 460f);
//	    } else camAngle.x = Mathf.Clamp(camAngle.x, -100f, maxAngleBelow);
//
//	    
//	    
//	    Recoil();
//	    
//	    cam.eulerAngles = camAngle + Vector3.left*recoilAngle;
//	    
//	    if (isBodyCopyCam) { 
//		    transform.eulerAngles = 
//			    new Vector3(transform.eulerAngles.x, camAngle.y, transform.eulerAngles.z);
//	    }
//	    
//	    cam.position = camArmBone.TransformPoint(camLocalPoint);
//    }
//    
//    
//    
//    void SetUpRig() {
//
//	    rig.data.constrainedObject = camArmBone;
//
//
//
//	    var data = rig.data.sourceObjects;
//	    
//	    data = new WeightedTransformArray(1);
//	    data.SetTransform(0, cam);
//	    data.SetWeight(0, 1f);
//	    rig.data.sourceObjects = data;
//			// sourceObjects는 직접 수정 불가
//    }
//
//
//
//    
//    void Recoil() {
//	    
//	    
//
//	    recoilAngle = Mathf.Lerp(recoilAngle, 0, Time.deltaTime*3);
//	    
//	    if (Input.GetKeyDown(KeyCode.Space)) {
//		    recoilAngle += 10f;
//		    Debug.Log(recoilAngle);
//	    }
//	    
//	    if (recoilAngle < 0.01f) { recoilAngle = 0f; }
//	    camAngle.x -= recoilAngle;
//    }
//
//    
//    
//    void OnDrawGizmos() {
//
//	    if (showGizmos) {
//
//		    Gizmos.color = Color.green;
//		    Gizmos.DrawSphere(camArmBone.position, 0.05f);
//		    Gizmos.DrawSphere(cam.position, 0.05f);
//
//		    Gizmos.DrawLine(camArmBone.position, cam.position);
//		    Gizmos.DrawRay(camArmBone.position, transform.forward * camLocalPoint.magnitude);
//	    }
//    }
//}
//
//// 장전중엔 IK weight 낮게.	
//// 장전키 누르고 0.n초동안 weight를 러프함
//// 
//// 위에거는 reload 애니메이션 넣어보고 어지저찌 했는데도 화면에 고정 안될 때
////
////
////
//// cam fallow speed 낮게