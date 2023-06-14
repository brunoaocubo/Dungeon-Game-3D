using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConstants;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject target;
    [SerializeField] private Transform lookAt;
    [SerializeField] private GameObject hitParticle;

    [Header("STATUS")]
    [SerializeField] private float[] randomValueDamage = new float[2];
    [SerializeField] private float detectionRange;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float forceKnockback;

    [Header("Sound FX")]
    [SerializeField] private AudioSource slimeSteeps;
    [SerializeField] private AudioSource slashSound;

    private NavMeshAgent agent;
    private Animator animator;
    private RaycastHit hit;
    private float life = 3f;
    private float currentLife;
    private int attackDistance = 2;
    private float timeRecoveryHit = 0.83f;
    private float timeAnimationAttack = 0.83f;
    private float timeAnimationDie = 1.34f;

    private bool canAttack = true;
    private bool damageTaken = false;
    private bool chaseMode = false;

    private const string NEGATE_DMG_TAKEN = "NegateDmgTaken";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentLife = life;
        agent.updateRotation = false;
        agent.stoppingDistance = attackDistance;
    }

    void Update()
    {
        FollowTarget();

        if (agent.velocity != Vector3.zero) 
        {
            animator.SetBool(Constants.WALK, true);
            if (!slimeSteeps.isPlaying) 
            {
                slimeSteeps.Play();
            }
        }
        else 
        {
            animator.SetBool(Constants.WALK, false);
        }
    }

    private void FollowTarget() 
    {
        if (Vector3.Distance(transform.position, target.transform.position) < detectionRange)
        {
            ChaseTarget();
            LookAtDirection();
            chaseMode = true;

            if (canAttack)
            {
                StartCoroutine(AttackPlayer(timeAnimationAttack));
            }
        }
    }

    private void ChaseTarget() 
    {
        if (chaseMode && currentLife > 0)
        {
            agent.destination = target.transform.position;

        }
        else
        {
            //back to patrol
        }
    }

    private void LookAtDirection() 
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
        if(currentLife > 0) 
        {
            damageTaken = true;
            currentLife -= damage;
            animator.SetTrigger(Constants.GETHIT);
        }
        if (currentLife <= 0) 
        {
            Die();
        }
        
        Instantiate(hitParticle, hitPoint, Quaternion.identity);
        Invoke(NEGATE_DMG_TAKEN, timeRecoveryHit);
    }

    private void NegateDmgTaken()
    {
        if (damageTaken)
        {
            damageTaken = false;
        }
    }

    private void Die() 
    {
        GameManager.instance.EnemyCount(1);
        animator.SetTrigger(Constants.DIE);
        Destroy(gameObject, timeAnimationDie);    
    }

    IEnumerator AttackPlayer(float timeAnimationAttack) 
    {
        if (!damageTaken && currentLife > 0) 
        {
            canAttack = false;

            if (Physics.Raycast(lookAt.position, transform.forward, out hit, attackDistance))
            {
                if (hit.collider.gameObject == target)
                {
                    animator.SetTrigger(Constants.ATTACK);
                    slashSound.Play();
                    hit.collider.gameObject.GetComponentInChildren<Health>().TakeDamage(hit.point, Random.Range(randomValueDamage[0], randomValueDamage[1]));
                }
            }

            yield return new WaitForSeconds(timeAnimationAttack);
            canAttack = true;
        }
    }
}
