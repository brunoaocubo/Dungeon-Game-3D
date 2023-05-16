using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Inputs inputs;
    [SerializeField] private GameObject particleDestructable;

    [SerializeField] private float life = 5;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed; 
    [SerializeField] private float swordDamage;




    private Rigidbody rb;
    private bool isWalk;

    private float timeToDodge;
    private float timeToAttack;
    private bool canAttack = true;
    private bool canDodge = true;
    public bool CanDodge { get { return canDodge; }}
    public bool CanAttack { get { return canAttack; }}
    private bool isTakeDamage = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        timeToDodge = 0.8f;
        timeToAttack = 0.6f;
        
    }

    private void Update()
    {

        if (inputs.GetDodgeButton())
        {
            if (canDodge)
            {
                rb.AddForce(transform.forward * dodgeForce, ForceMode.Impulse);

                canDodge = false;
                
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

        CalculatorCooldown(canAttack, timeToAttack, 0.6f);
        CalculatorCooldown(canDodge, timeToDodge, 0.8f);
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = inputs.GetMovementVectorNormalized();       
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 rotate = new Vector3(0, inputVector.x, 0) * rotateSpeed * 100f;
        moveDir = rb.rotation * moveDir;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotate * Time.fixedDeltaTime));

        isWalk = (moveDir != Vector3.zero);
    }

    private void AttackTime()
    {
        if (canAttack == false)
        {
            timeToAttack -= Time.deltaTime;
            if (timeToAttack <= 0)
            {
                canAttack = true;

                float timeAnimation = 0.6f;
                timeToAttack = timeAnimation;
            }
        }     
    }

    private void DodgeTime()
    {
        if (canDodge == false)
        {
            timeToDodge -= Time.deltaTime;

            if (timeToDodge <= 0)
            {
                canDodge = true;

                float timeAnimation = 0.8f;
                timeToDodge = timeAnimation;
            }
        }
    }

    private void CalculatorCooldown(bool x, float time, float timeAnimation) 
    {
        if(x == false) 
        {
            time -= Time.deltaTime;

            if(time <= 0) 
            {
                x = true;
                time = timeAnimation; 
            }
        }
    }

    private void Attack() 
    {
        Collider[] getEnemies = Physics.OverlapSphere(transform.position, 1);
        foreach (Collider collider in getEnemies) 
        {
            if(collider.gameObject.layer == 7) 
            {
                Instantiate(particleDestructable, collider.transform.position, Quaternion.identity);
                Destroy(collider.gameObject);
            }
            else if(collider.gameObject.layer == 6) 
            {
                collider.GetComponent<Enemy>().TakeDamage(swordDamage);
            }
        }
    }

    public void TakeDamage(float damage) 
    {
        isTakeDamage = false;

        if(damage > 0)
        {
            life -= damage;
            isTakeDamage = true;
        }
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
