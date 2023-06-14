using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Slider health_Slider;
    [SerializeField] private GameObject menuGame_Panel;
    [SerializeField] private GameObject menuOptions_Panel;
    [SerializeField] private GameObject loadingScrenn_Panel;
    [SerializeField] private GameObject finalScrenn_Panel;
    [SerializeField] private Slider loading_Slider;
    [SerializeField] private TextMeshProUGUI loading_Text;
    [SerializeField] private GameObject loadingConfirmed_Text;
    [SerializeField] private Slider musicVolume_Slider;
    [SerializeField] private Slider audioFxVolume_Slider;
    
    private float defaultMusicVolume = 1.0f;
    private float defaultAudioFxVolume = 1.0f;

    [SerializeField] private InputController inputs;
    void Start()
    {
        menuGame_Panel.SetActive(false);
        menuOptions_Panel.SetActive(false);
        loadingScrenn_Panel.SetActive(false);
        loadingConfirmed_Text.SetActive(false);
        finalScrenn_Panel.SetActive(false);

        health_Slider.maxValue = health.StartHealth;
        musicVolume_Slider.value = defaultMusicVolume;
        audioFxVolume_Slider.value = defaultAudioFxVolume;
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
        ToggleMenuInGame();
    }

    private void ToggleMenuInGame() 
    {
        if (menuGame_Panel.activeInHierarchy) 
        {
            menuGame_Panel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
        }
        else 
        {
            menuGame_Panel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        health_Slider.value = health.CurrentHealth;
        AudioManager.Instance.SetVolumeMusic(musicVolume_Slider.value);
        AudioManager.Instance.SetVolumeAudioFX(audioFxVolume_Slider.value);

        if(GameManager.instance.EnemyRemain <= 0) 
        {
            finalScrenn_Panel.SetActive(true);
        }
        else 
        {
            finalScrenn_Panel.SetActive(false);
        }
    }

    public void ResumeGame() 
    {
        ToggleMenuInGame();
    }

    public void BeginGame()
    {
        StartCoroutine(LoadAsyncScene(0));
    }

    public void OpenOptions() 
    {
        if (!menuOptions_Panel.activeInHierarchy)
        {
            menuGame_Panel.SetActive(false);
            menuOptions_Panel.SetActive(true);
        }
    }

    public void BackOptions()
    {
        if (!menuGame_Panel.activeInHierarchy)
        {
            menuGame_Panel.SetActive(true);
            menuOptions_Panel.SetActive(false);
        }
    }

    public void QuitGame() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    IEnumerator LoadAsyncScene(int sceneId) 
    {
        loadingScrenn_Panel.SetActive(true);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone) 
        {
            loading_Text.text = " " + (asyncLoad.progress * 110) + "%";
            loading_Slider.value =  Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (asyncLoad.progress >= 0.9f) 
            {
                loadingConfirmed_Text.SetActive(true);

                if (Input.anyKey)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}
