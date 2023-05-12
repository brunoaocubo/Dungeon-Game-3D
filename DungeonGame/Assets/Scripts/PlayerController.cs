using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Inputs inputs;
    [SerializeField] private GameObject particleDestructable;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dodgeForce;
    [SerializeField] private float rotateSpeed;
    
    [SerializeField] private float swordDamage;


    private Rigidbody rb;
    private bool isWalk;

    private float timeDodge;
    private float timeAttack;
    private bool canAttack = true;
    private bool canDodge = true;
    public bool CanDodge { get { return canDodge; }}
    public bool CanAttack { get { return canAttack; }}

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        timeDodge = 0.8f;
        timeAttack = 0.6f;
        
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

        if (canAttack)
        {
            if (inputs.GetAttackButton())
            {
                canAttack = false;
                Attack();              
            }
        }

        AttackTime();
        DodgeTime();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = inputs.GetMovementVectorNormalized();       
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        Vector3 rotate = new Vector3(0, inputVector.x, 0) * rotateSpeed;
        moveDir = rb.rotation * moveDir;
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotate * 100f * Time.fixedDeltaTime));

        isWalk = (moveDir != Vector3.zero);
    }

    private void AttackTime()
    {
        if (canAttack == false)
        {
            timeAttack -= Time.deltaTime;
            if (timeAttack <= 0)
            {
                canAttack = true;
                timeAttack = 0.6f;
            }
        }     
    }

    private void DodgeTime()
    {
        if (canDodge == false)
        {
            timeDodge -= Time.deltaTime;

            if (timeDodge <= 0)
            {
                canDodge = true;
                timeDodge = 0.8f;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1);
    }

    public bool IsWalking()
    {
        return isWalk;
    }
}
