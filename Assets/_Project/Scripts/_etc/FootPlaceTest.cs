//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class FootPlaceTests : MonoBehaviour {
//    
//    private Animator anim;
//    [Range(0f, 1f), SerializeField]
//    private float footMaxHeight;
//
//    [Range(0f, 1f), SerializeField]
//    private float footMaxDepth;
//    
//    [Range(0f, 0.5f), SerializeField]
//    private float footPlacingHeight;
//
//
//    private Vector3 footPos;
//    private Ray footRay;
//    private RaycastHit footRayHit;
//
//    
//    
//    void Awake() {
//
//        anim = GetComponent<Animator>();
//    }
//    
//    
//    
//    
//    
//    void OnAnimatorIK(int layerIndex) {
//        
//        
//        
//        FootPlace(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootWeight"));
//        FootPlace(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootWeight"));
//    }
//    
//
//
//
//    void FootPlace(AvatarIKGoal foot, float weightParameter) {
//
//        footRay = new Ray(anim.GetIKPosition(foot) + Vector3.up * footMaxHeight, Vector3.down);
//        if (Physics.Raycast(footRay, out footRayHit, footMaxHeight + footMaxDepth)) {
//
//            footPos = footRayHit.point;
//            footPos.y += footPlacingHeight;
//        }
//        
//        anim.SetIKPositionWeight(foot, weightParameter);
//        anim.SetIKPosition(foot, footPos);
//        
//        anim.SetIKRotationWeight(foot, 1f);
//        anim.SetIKRotation(foot, Quaternion.LookRotation(
//            Vector3.ProjectOnPlane(transform.forward, footRayHit.normal), footRayHit.normal) );
//    }
//    
//    
//    
//    
//    
////    void OnDrawGizmos() {
////
////        Gizmos.color = Color.yellow; 
////        Gizmos.DrawSphere(anim.GetIKPosition(AvatarIKGoal.LeftFoot), 0.03f);
////
////        Gizmos.color = Color.red;
////        Gizmos.DrawRay(footRay.origin, footRay.direction * (footMaxHeight + footMaxDepth));
////        Gizmos.DrawSphere(footRayHit.point, 0.04f);
////        Gizmos.DrawWireSphere(footRay.origin, 0.04f);
////    }
//}
