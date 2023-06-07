using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    [SerializeField] private Transform playerSpawn;

    void Start()
    {
        int indexSelectedCharacter = PlayerPrefs.GetInt("indexSelectedCharacter");
        GameObject playerPrefab = characters[indexSelectedCharacter];
        GameObject player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation, playerSpawn);
    }
}