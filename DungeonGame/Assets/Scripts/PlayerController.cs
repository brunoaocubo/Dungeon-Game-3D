using System.Collections;
using UnityEngine;
using GlobalConstants;

public enum PlayerState 
{
    Walking,
    Attacking,
    Dodging
}

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

    private PlayerAnimation playerAnimation;
    private Health playerHealth;
    private Rigidbody playerRigdbody;
    private Vector2 inputVector;
    private float timeAnimationDodge = 0.6f;
    private float timeCooldownAttack = 1f;
    private bool isWalking;
    private bool isDodge = false;
    #endregion

    #region STARTS, UPDATES...

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRigdbody = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerHealth = GetComponent<Health>();
    }

    void Start()
    {
        inputVector = inputs.GetMovementVectorNormalized();
        inputs.OnInteractAction += Inputs_OnInteractAction;
        inputs.OnAttackAction += Inputs_OnAttackAction;
        inputs.OnDodgeAction += Inputs_OnDodgeAction;

        StartCoroutine(InstancePlayerAnimation());
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerRotation();
    }

    private void Update()
    {
        if(playerAnimation != null) 
        {
            playerAnimation.SetWalkAnimation(isWalking);
            playerAnimation.SetDodgeAnimation(isDodge);
        }

        if (playerHealth.DamageTaken) 
        {
            playerAnimation.PlayGetHitAnimation();
        }
    }
    #endregion

    #region EVENTS
    private void Inputs_OnInteractAction(object sender, System.EventArgs e)
    {

    }

    private void Inputs_OnAttackAction(object sender, System.EventArgs e)
    {
        StartCoroutine(Attack());
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
    #endregion

    #region METHODS
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

    private void AttackComplement() 
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
                    collider.GetComponent<Enemy>().TakeDamage(collider.transform.TransformPoint(Vector3.up), swordDamage);
                }
            }
        }
    }
    #endregion

    #region COROUTINES

    IEnumerator CalculatorCooldown(bool x, float timeAnimation)
    {
        
        yield return new WaitForSeconds(timeAnimation); 
        if (x == isDodge) { isDodge = false; }
    }

    IEnumerator Attack()
    {       
        playerAnimation.PlayAttackAnimation();
        AttackComplement();

        yield return new WaitForSeconds(timeCooldownAttack);
    }

    IEnumerator Die() 
    {
        playerAnimation.PlayDieAnimation();
        //animator.SetTrigger(Constants.DIE);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator InstancePlayerAnimation()
    {
        yield return new WaitForEndOfFrame();
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }
    #endregion

    #region FUNCTIONS
    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsDodge() 
    { 
        return isDodge;  
    }
    #endregion
}
