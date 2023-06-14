using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ForDestructableObj : MonoBehaviour
{
    [SerializeField] private GameObject particleDestroy;

    public void InstantiateParticle() 
    {
        Instantiate(particleDestroy, transform.position, Quaternion.identity);
    }
}
