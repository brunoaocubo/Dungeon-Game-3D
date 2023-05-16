using Dev.ComradeVanti.WaitForAnim;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    const string GETHIT = "GetHit";
    const string DIE = "Die";

    [SerializeField] private float currentLife;

    private Animator animator;
    private float life = 3;

    void Start()
    {
        currentLife = life;  
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if(currentLife <= 0) 
        {
            animator.SetTrigger(DIE);
            Destroy(gameObject, 2f);
        }
    }

    public void TakeDamage(float damage) 
    {
        currentLife -= damage;
        animator.SetTrigger(GETHIT);
    }
}
