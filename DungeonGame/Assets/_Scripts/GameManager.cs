using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputController inputs;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
