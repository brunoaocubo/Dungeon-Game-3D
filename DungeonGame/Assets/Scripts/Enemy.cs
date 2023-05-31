using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalConstants;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform lookAt;
    [SerializeField] private GameObject hitParticle;

    [Header("STATUS")]
    [SerializeField] private float[] randomValueDamage = new float[2];
    [SerializeField] private float detectionRange;
    [SerializeField] private float forceKnockback;

    [Header("Sound FX")]
    [SerializeField] private GameObject walkSound;
    [SerializeField] private AudioSource slashSound;

    private Rigidbody enemyRigdbody;
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
        enemyRigdbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentLife = life;
        agent.stoppingDistance = attackDistance;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < detectionRange) 
        {
            chaseMode = true;

            if (canAttack) 
            {
                StartCoroutine(AttackPlayer(timeAnimationAttack));
            }  
        }

        if(currentLife <= 0) 
        {
            animator.SetTrigger(Constants.DIE);
            agent.velocity = Vector3.zero;
            Destroy(gameObject, timeAnimationDie);
        }

        if (chaseMode)
        {
            agent.destination = player.transform.position;

        }
        else
        {
            //back to patrol
        }

        if (agent.velocity != Vector3.zero) 
        {
            animator.SetBool(Constants.WALK, true);
            walkSound.SetActive(true);
        }
        else 
        {
            animator.SetBool(Constants.WALK, false);
            walkSound.SetActive(false);
        }
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
        damageTaken = true;
        currentLife -= damage;
        animator.SetTrigger(Constants.GETHIT);

        Instantiate(hitParticle, hitPoint, Quaternion.identity);

        StartCoroutine(Knockback());
        Invoke(NEGATE_DMG_TAKEN, timeRecoveryHit);
    }

    private void NegateDmgTaken()
    {
        if (damageTaken)
        {
            damageTaken = false;
        }
    }

    IEnumerator AttackPlayer(float timeAnimationAttack) 
    {
        if (!damageTaken && currentLife > 0) 
        {
            canAttack = false;

            if (Physics.Raycast(lookAt.position, transform.forward, out hit, attackDistance))
            {
                if (hit.collider.gameObject == player)
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

    IEnumerator Knockback() 
    {
        Vector3 direction = player.transform.forward;

        agent.speed = agent.speed * 2;
        agent.angularSpeed = agent.angularSpeed - agent.angularSpeed;
        agent.acceleration = agent.acceleration * 2;
        agent.velocity = direction * forceKnockback;

        yield return new WaitForSeconds(timeRecoveryHit);

        agent.speed = agent.speed /2;
        agent.angularSpeed = 120;
        agent.acceleration = agent.acceleration * 2;
        agent.velocity = direction * forceKnockback;

    }
}
