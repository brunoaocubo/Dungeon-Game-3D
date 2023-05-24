using System.Collections;
using UnityEngine;
using GlobalConstants;

public class PlayerController : MonoBehaviour
{
    #region VARIAVEIS...
    [Header("REFERENCES")]
    [SerializeField] private Inputs inputs;
    [SerializeField] private GameObject hitParticle;
<<<<<<< HEAD


    [Header("STATUS")]
=======

    [Header("STATUS")]
    [SerializeField] private float health = 100;
>>>>>>> parent of b2bf63e (DungeonGame)
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float swordDamage;

    private Rigidbody playerRigdbody;
<<<<<<< HEAD
    private Animator animator;
    private Vector2 inputVector;
    private float timeAnimationDodge = 0.6f;
    private float timeAnimationAttack = 0.533f;
    private bool isWalking;
    public bool isTakeDamage = false;
    private bool isAttacking = false;
    private bool isDodge = false;
=======
    private bool isWalk;
    private bool isTakeDamage = false;
    private float timeAnimationDodge = 0.6f;
    private float timeAnimationAttack = 0.8f;
    private bool canAttack = true;
    private bool canDodge = true;
    public bool CanDodge { get { return canDodge; }}
    public bool CanAttack { get { return canAttack; }}

>>>>>>> parent of b2bf63e (DungeonGame)
    #endregion

    #region STARTS, UPDATES...

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
<<<<<<< HEAD
        playerRigdbody = GetComponent<Rigidbody>();
=======
        playerRigdbody = GetComponent<Rigidbody>();  
>>>>>>> parent of b2bf63e (DungeonGame)
    }

    void Start()
    {
<<<<<<< HEAD
        animator = GetComponentInChildren<Animator>();
        inputVector = inputs.GetMovementVectorNormalized();
        inputs.OnInteractAction += Inputs_OnInteractAction;
        inputs.OnAttackAction += Inputs_OnAttackAction;
        inputs.OnDodgeAction += Inputs_OnDodgeAction;
=======
        if (inputs.GetDodgeButton())
        {
            if (canDodge)
            {
                canDodge = false;
                playerRigdbody.AddForce(transform.forward * dodgeForce, ForceMode.Impulse);
                StartCoroutine(CalculatorCooldown(canDodge, timeAnimationDodge));             
            }
        }

        if (inputs.GetAttackButton())
        {
            if (canAttack)
            {
                canAttack = false;
                Attack();
            }
        }
>>>>>>> parent of b2bf63e (DungeonGame)
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
    }

    private void Update()
    {
        animator.SetBool(Constants.WALK, IsWalking());
        animator.SetBool(Constants.DODGE, IsDodge()); 
        if (IsAttacking())
        {
            animator.SetLayerWeight(1, 1f);
        }
        else
        {
            animator.SetLayerWeight(1, 0f);
        }       
    }
    #endregion

    private void Inputs_OnInteractAction(object sender, System.EventArgs e)
    {

    }

    private void Inputs_OnAttackAction(object sender, System.EventArgs e)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            Attack();
            StartCoroutine(CalculatorCooldown(isAttacking, timeAnimationAttack));
        }
    }

    private void Inputs_OnDodgeAction(object sender, System.EventArgs e)
    {
        if (!isDodge)
        {
            isDodge = true;
            playerRigdbody.AddForce(transform.forward * dodgeForce, ForceMode.Impulse);
            StartCoroutine(CalculatorCooldown(isDodge, timeAnimationDodge));
        }
    }

    private void PlayerMovement() 
    {
        inputVector = inputs.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        moveDir = playerRigdbody.rotation * moveDir;
        playerRigdbody.MovePosition(playerRigdbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);

        isWalking = (moveDir != Vector3.zero);
    }

    private void PlayerRotation() 
    {
        Vector3 rotate = new Vector3(0, inputVector.x, 0) * rotateSpeed * 100f;
        playerRigdbody.MoveRotation(playerRigdbody.rotation * Quaternion.Euler(rotate * Time.fixedDeltaTime));
    }

    private void Attack() 
    {
        Collider[] getEnemies = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider collider in getEnemies) 
        {
            if(collider != null) 
            {
                if (collider.gameObject.layer == Constants.OBJECT_DESTRUCTABLE)
                {
                    Destroy(collider.gameObject);
                }
                else if (collider.gameObject.layer == Constants.ENEMY)
                {
                    collider.GetComponent<Enemy>().TakeDamage(collider.transform.position, swordDamage);
                }
            }
        }
<<<<<<< HEAD
        StartCoroutine(CalculatorCooldown(isAttacking, timeAnimationAttack));
=======
        StartCoroutine(CalculatorCooldown(canAttack, timeAnimationAttack));
>>>>>>> parent of b2bf63e (DungeonGame)
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
        Instantiate(hitParticle, hitPoint, Quaternion.identity);
<<<<<<< HEAD
        //health -= damage;
        animator.SetTrigger(Constants.GETHIT);
        /*
        if(canTakeDamage)
        {
            health -= damage;
            canTakeDamage = false;
            Instantiate(hitParticle, hitPoint, Quaternion.identity);
            CooldownAnimation(canTakeDamage, 1f);
        }     */
=======
        isTakeDamage = false;

        if(damage > 0)
        {
            health -= damage;
            isTakeDamage = true;
        }
>>>>>>> parent of b2bf63e (DungeonGame)
    }

    IEnumerator CalculatorCooldown(bool x, float timeAnimation)
    {
        yield return new WaitForSeconds(timeAnimation);
<<<<<<< HEAD
        if (x == isAttacking) { isAttacking = false; }     
        if (x == isDodge) { isDodge = false; }
=======

        if (x == canAttack) { canAttack = true; }
        else if (x == canDodge) { canDodge = true; }
>>>>>>> parent of b2bf63e (DungeonGame)
    }

    public bool IsGetHit ()
    {
        return isTakeDamage;
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsDodge() 
    { 
        return isDodge;  
    }

    public bool IsAttacking() 
    {  
        return isAttacking;  
    }
}
