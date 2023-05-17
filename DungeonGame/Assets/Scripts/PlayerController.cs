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
    [SerializeField] private float totalHealth = 100;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed; 
    [SerializeField] private float swordDamage;
    [SerializeField] private int flaskLife = 3;


    private Rigidbody playerRigdbody;
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

    #endregion

    #region STARTS, UPDATES...
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerRigdbody = GetComponent<Rigidbody>();  
        health = totalHealth;
    }

    private void Update()
    {
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
        StartCoroutine(CooldownAnimation(canAttack, timeAnimationAttack));
    }

    public void TakeDamage(Vector3 hitPoint, float damage) 
    {
        if(canTakeDamage)
        {
            health -= damage;
            canTakeDamage = false;
            Instantiate(hitParticle, hitPoint, Quaternion.identity);
            CooldownAnimation(canTakeDamage, 1f);
        }     
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

        if (x == canAttack) { canAttack = true; }
        else if (x == canDodge) { canDodge = true; }
        else if (x == canTakeDamage) { canTakeDamage = true; }
    }

    public bool IsGetHit ()
    {
        return canTakeDamage;
    }

    public bool IsWalking()
    {
        return isWalk;
    }
}
