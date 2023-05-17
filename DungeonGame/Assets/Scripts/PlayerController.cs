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
    [SerializeField] private float health = 100;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed; 
    [SerializeField] private float swordDamage;

    private Rigidbody playerRigdbody;
    private bool isWalk;
    private bool isTakeDamage = false;
    private float timeAnimationDodge = 0.6f;
    private float timeAnimationAttack = 0.8f;
    private bool canAttack = true;
    private bool canDodge = true;
    public bool CanDodge { get { return canDodge; }}
    public bool CanAttack { get { return canAttack; }}

    #endregion

    #region STARTS, UPDATES...
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRigdbody = GetComponent<Rigidbody>();  
    }

    private void Update()
    {
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
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }
    #endregion

    private void PlayerMovement() 
    {
        Vector2 inputVector = inputs.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 rotate = new Vector3(0, inputVector.x, 0) * rotateSpeed * 100f;
        moveDir = playerRigdbody.rotation * moveDir;
        playerRigdbody.MovePosition(playerRigdbody.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        playerRigdbody.MoveRotation(playerRigdbody.rotation * Quaternion.Euler(rotate * Time.fixedDeltaTime));

        isWalk = (moveDir != Vector3.zero);
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
        StartCoroutine(CalculatorCooldown(canAttack, timeAnimationAttack));
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
        Instantiate(hitParticle, hitPoint, Quaternion.identity);
        isTakeDamage = false;

        if(damage > 0)
        {
            health -= damage;
            isTakeDamage = true;
        }
    }

    IEnumerator CalculatorCooldown(bool x, float timeAnimation)
    {
        yield return new WaitForSeconds(timeAnimation);

        if (x == canAttack) { canAttack = true; }
        else if (x == canDodge) { canDodge = true; }
    }

    public bool IsGetHit ()
    {
        return isTakeDamage;
    }

    public bool IsWalking()
    {
        return isWalk;
    }
}
