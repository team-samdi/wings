using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    [SerializeField]
    private float moveBlendTransitionSpeed;


    private Animator animator;


    private float moveParam;

    private PlayerMove move;
    
    void Awake() {

        animator = GetComponent<Animator>();

        move = GetComponent<PlayerMove>();
    }


    
    void Update() {
        
        move.GetInput(out float xInput, out float zInput, out _);
        
        if (new Vector2(xInput, zInput).magnitude != 0) {
            moveParam = Mathf.Lerp(moveParam, move.IsRun ? 2 : 1, Time.deltaTime * moveBlendTransitionSpeed);

        } else moveParam = Mathf.Lerp(moveParam, 0, Time.deltaTime * moveBlendTransitionSpeed);
        
        animator.SetFloat("MoveParameter", moveParam);
    }
}
