using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] charactersSelect;
    [SerializeField] private int indexSelectedCharacter;

    public void PlayGame() 
    {
        PlayerPrefs.SetInt("indexSelectedCharacter", indexSelectedCharacter);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void NextCharacter() 
    {
        charactersSelect[indexSelectedCharacter].SetActive(false);
        indexSelectedCharacter = (indexSelectedCharacter + 1) % charactersSelect.Length; //Quando chegar no final do array irá aproximar para o valor mais próximo.
        charactersSelect[indexSelectedCharacter].SetActive(true);
    }

    public void PreviousCharacter() 
    {
        charactersSelect[indexSelectedCharacter].SetActive(false);
        indexSelectedCharacter--;
        if(indexSelectedCharacter < 0) 
        {
            indexSelectedCharacter += charactersSelect.Length;
        }
        charactersSelect[indexSelectedCharacter].SetActive(true);
    }
}