using System.Collections;
using UnityEngine;
using GlobalConstants;

public class PlayerController : MonoBehaviour
{
    #region VARIAVEIS...
    [Header("REFERENCES")]
    [SerializeField] private Inputs inputs;
    [SerializeField] private GameObject hitParticle;
    //[SerializeField] private Inventory inventory;


    [Header("STATUS")]
<<<<<<< HEAD
=======
    [SerializeField] private float totalHealth = 100;
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float swordDamage;
    [SerializeField] private int flaskLife = 3;


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
    private float health;
    private float timeFlask = 3f;
    private float timeAnimationDodge = 0.6f;
    private float timeAnimationAttack = 0.8f;
    private bool isWalk;
    private bool canTakeDamage = false;
    private bool canUseFlask = true;
    private bool canAttack = true;
    private bool canDodge = true;
  
    public bool CanDodge { get { return canDodge; }}
    public bool CanAttack { get { return canAttack; }}
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647

    #endregion

    #region STARTS, UPDATES...

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
<<<<<<< HEAD
        playerRigdbody = GetComponent<Rigidbody>();
        
=======
        playerRigdbody = GetComponent<Rigidbody>();  
        health = totalHealth;
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647
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
        if(inputs.GetUtilityButton1())
        {
            if(canUseFlask)
            {
                canUseFlask = false;
                float value = 25f;
                StartCoroutine(UseFlask(flaskLife, value));  
            }
                        
        }

        if (inputs.GetDodgeButton())
        {
            if (canDodge)
            {
                canDodge = false;
                playerRigdbody.AddForce(transform.forward * dodgeForce, ForceMode.Impulse);
                StartCoroutine(CooldownAnimation(canDodge, timeAnimationDodge));             
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
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647
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
=======
        StartCoroutine(CooldownAnimation(canAttack, timeAnimationAttack));
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
<<<<<<< HEAD
        Instantiate(hitParticle, hitPoint, Quaternion.identity);
        //health -= damage;
        animator.SetTrigger(Constants.GETHIT);
=======
        if(canTakeDamage)
        {
            health -= damage;
            canTakeDamage = false;
            Instantiate(hitParticle, hitPoint, Quaternion.identity);
            CooldownAnimation(canTakeDamage, 1f);
        }     
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647
    }

    IEnumerator UseFlask(int typeFlask, float value)
    {        
        if(typeFlask == flaskLife)
        {       
            flaskLife --;
            health += value;     
        }  

        yield return new WaitForSeconds(timeFlask);  
        canUseFlask = true;  
        
    }

    IEnumerator CooldownAnimation(bool x, float timeAnimation)
    {
        yield return new WaitForSeconds(timeAnimation);
<<<<<<< HEAD
        if (x == isAttacking) { isAttacking = false; }     
        if (x == isDodge) { isDodge = false; }
=======

        if (x == canAttack) { canAttack = true; }
        else if (x == canDodge) { canDodge = true; }
        else if (x == canTakeDamage) { canTakeDamage = true; }
    }

    public bool IsGetHit ()
    {
        return canTakeDamage;
>>>>>>> b2bf63e9ea4b7ee615659e2f02965ba0d8b2c647
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
