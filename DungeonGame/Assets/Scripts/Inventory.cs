using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int flaskLife = 3;

    private void Awake()
    {
        flaskLife = 3;
    }

    public void UseFlaskLife()
    {
        StartCoroutine(FlaskCooldown());
    }

    IEnumerator FlaskCooldown()
    {
        yield return new WaitForSeconds(3f);
        if(flaskLife > 0)
        {
            flaskLife--;
        }
    }
}
