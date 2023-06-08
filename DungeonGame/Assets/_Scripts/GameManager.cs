using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputController inputs;
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private Collider localSpawn;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        float x = Random.Range(-localSpawn.bounds.size.x, localSpawn.bounds.size.x);
        float z = Random.Range(-localSpawn.bounds.size.z, localSpawn.bounds.size.z);
        Instantiate(enemyToSpawn, new Vector3(x, 0f, z), Quaternion.identity);
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
}
