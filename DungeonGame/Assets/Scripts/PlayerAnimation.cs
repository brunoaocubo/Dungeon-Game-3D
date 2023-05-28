using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConstants;


public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetWalkAnimation(bool isWalking)
    {
        animator.SetBool(Constants.WALK, isWalking);
    }

    public void SetDodgeAnimation(bool isDodge)
    {
        animator.SetBool(Constants.DODGE, isDodge);
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(Constants.ATTACK);
    }

    public void PlayGetHitAnimation()
    {
        animator.SetTrigger(Constants.GETHIT);
    }

    public void PlayDieAnimation()
    {
        animator.SetTrigger(Constants.DIE);
    }
}
