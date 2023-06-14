using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private InputController inputs;
    private int enemyRemain = 7;
    public int EnemyRemain { get { return enemyRemain; }  }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        inputs.OnSettingsAction += Inputs_OnSettingsAction;
    }

    private void OnDisable()
    {
        inputs.OnSettingsAction -= Inputs_OnSettingsAction;
    }

    private void Inputs_OnSettingsAction(object sender, System.EventArgs e)
    {
        Time.timeScale = 0;
    }

    public void EnemyCount(int count)
    {
        enemyRemain += count;
        if (enemyRemain <= 0)
        {
            Debug.Log("Acabou");
        }
    }
}
