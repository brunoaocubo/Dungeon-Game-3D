using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float life = 3;
    private float currentLife;

    void Start()
    {
        currentLife = life;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLife <= 0) 
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage) 
    {
        currentLife -= damage;
    }
}
