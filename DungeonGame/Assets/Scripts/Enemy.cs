using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConstants;

public class Enemy : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform lookAt;
    [SerializeField] private GameObject hitParticle;

    [Header("STATUS")]
    [SerializeField] private float[] randomValueDamage = new float[2];
    [SerializeField] private float attackRange; 

    private Animator animator;
    private RuntimeAnimatorController runTimeAnimator;
    private RaycastHit hit;
    private float life = 3;
    private float currentLife;
    private float timeAnimationAttack;
    private bool canAttack = true;

    void Start()
    {
        currentLife = life;  
        animator = GetComponent<Animator>();
        runTimeAnimator = animator.runtimeAnimatorController;

        for (int i = 0; i < runTimeAnimator.animationClips.Length; i++) 
        {
            if (runTimeAnimator.animationClips[i].name == Constants.ATTACK_CLIP) 
            {
                timeAnimationAttack = runTimeAnimator.animationClips[i].length;
            }
        }
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < attackRange) 
        {           
            if(canAttack) 
            {
                StartCoroutine(AttackPlayer(timeAnimationAttack));
            }  
        }

        if(currentLife <= 0) 
        {
            animator.SetTrigger(Constants.DIE);
            Destroy(gameObject, 2f);
        }

        Debug.DrawRay(lookAt.position, transform.forward, Color.black);
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
        Instantiate(hitParticle, hitPoint, Quaternion.identity);
        currentLife -= damage;

        animator.SetTrigger(Constants.GETHIT);
    }

    IEnumerator AttackPlayer(float timeAnimationAttack) 
    {
        canAttack = false;

        if(Physics.Raycast(lookAt.position, transform.forward, out hit, attackRange)) 
        {
            if (hit.collider.gameObject == player) 
            {
                animator.SetBool(Constants.ATTACK, !canAttack);
                hit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(hit.point, Random.Range(randomValueDamage[0], randomValueDamage[1]));
            }
        }
       
        yield return new WaitForSeconds(timeAnimationAttack);
        canAttack = true;
    }
}
