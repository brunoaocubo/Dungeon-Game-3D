using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject hitParticle;
    [SerializeField] private float startHealth = 100f;

    private float currentHealth; 
    private bool damageTaken;

    public float StartHealth { get { return startHealth; } private set { } }
    public float CurrentHealth { get { return currentHealth; } private set { } }
    public bool DamageTaken { get { return damageTaken; } private set { } }

    private const string NEGATE_DMG_TAKEN = "NegateDmgTaken";

    void Start()
    {
        currentHealth = startHealth;
    }

    public void TakeDamage(Vector3 hitPoint, float damage)
    {
        if(damage > 0) 
        {
            currentHealth -= damage;
            damageTaken = true;

            Instantiate(hitParticle, hitPoint, Quaternion.identity);
            Invoke(NEGATE_DMG_TAKEN, 0);
        }
        
    }

    private void NegateDmgTaken() 
    {
        if(damageTaken) 
        {
            damageTaken = false;
        }
    }
}
