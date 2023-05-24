using System.Collections;
using UnityEngine;
using GlobalConstants;

public class PlayerController : MonoBehaviour
{
    #region VARIAVEIS...
    [Header("REFERENCES")]
    [SerializeField] private Inputs inputs;
    [SerializeField] private GameObject hitParticle;

    [Header("STATUS")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float swordDamage;

    private Rigidbody playerRigdbody;
    private Animator animator;
    private Vector2 inputVector;
    private float timeAnimationDodge = 0.6f;
    private float timeAnimationAttack = 0.533f;
    private bool isWalking;
    public bool isTakeDamage = false;
    private bool isAttacking = false;
    private bool isDodge = false;

    #endregion

    #region STARTS, UPDATES...

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRigdbody = GetComponent<Rigidbody>();
        
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        inputVector = inputs.GetMovementVectorNormalized();
        inputs.OnInteractAction += Inputs_OnInteractAction;
        inputs.OnAttackAction += Inputs_OnAttackAction;
        inputs.OnDodgeAction += Inputs_OnDodgeAction;
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
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
        Instantiate(hitParticle, hitPoint, Quaternion.identity);
        //health -= damage;
        animator.SetTrigger(Constants.GETHIT);
    }

    IEnumerator CalculatorCooldown(bool x, float timeAnimation)
    {
        yield return new WaitForSeconds(timeAnimation);
        if (x == isAttacking) { isAttacking = false; }     
        if (x == isDodge) { isDodge = false; }
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
