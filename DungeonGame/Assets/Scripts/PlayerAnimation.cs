using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;

    const string MOVE = "Move";
    const string DODGE = "Dodge";
    const string GETHIT = "GetHit";

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        animator.SetBool(MOVE, playerController.IsWalking());
        animator.SetBool(DODGE, playerController.CanDodge != true);

        if(playerController.IsGetHit()) 
        {
            animator.SetTrigger(GETHIT);
        }

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
