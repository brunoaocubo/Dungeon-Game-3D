using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    
    void Update()
    {
        animator.SetBool("Move", playerController.IsWalking());
        animator.SetBool("Dodge", playerController.CanDodge != true);
        if(playerController.CanAttack != true) 
        {
            animator.SetLayerWeight(1, 1f);
        }
        else 
        {
            animator.SetLayerWeight(1, 0f);
        }
    }
}
