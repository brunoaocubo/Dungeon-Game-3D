using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConstants;
using Unity.VisualScripting;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        /*
        animator.SetBool(Constants.WALK, playerController.IsWalking());
        animator.SetBool(Constants.DODGE, playerController.IsDodge());

        if(playerController.IsGetHit()) 
        { 
            animator.SetTrigger(Constants.GETHIT); 
        }
        
        if(playerController.IsAttacking()) 
        { 
            animator.SetLayerWeight(1, 1f);
        }
        else 
        { 
            animator.SetLayerWeight(1, 0f); 
        }*/
    }
}
