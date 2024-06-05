using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private AudioSource _audioSource;
    private enemy_hit[] enemyList;
    private int enemyCounter = 0;
    [SerializeField] private AudioClip playerDeathNoise;
    private TextMeshProUGUI winText;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        player.onDeath += HandlePlayerDeath;
        enemyList = FindObjectsOfType<enemy_hit>();
        foreach (var enemy in enemyList)
        {
            enemy.onSpawn += HandleEnemySpawn;
            enemy.onDeath += HandleEnemyDeath;
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        GameObject canvasObject = GameObject.Find("CanvasUI");
        winText = canvasObject.transform.Find("WinText").GetComponent<TextMeshProUGUI>();
        winText.enabled = false;
    }
    
    private void HandleEnemySpawn()
    {
        enemyCounter += 1;
    }

    private void HandleEnemyDeath()
    {
        enemyCounter -= 1;
        if (enemyCounter == 0)
        {
            winText.enabled = true; // You Win!
            StartCoroutine(ChangeSceneAfterDelay(3, "level select"));
        }
    }

    private void HandlePlayerDeath()
    {
        _audioSource.PlayOneShot(playerDeathNoise,0.7f);
        StartCoroutine(ChangeSceneAfterDelay(3, "level select"));
    }

    private void DisableWinText()
    {

    }

    private IEnumerator ChangeSceneAfterDelay(float timeWait, string newScene)
    {
        yield return new WaitForSeconds(timeWait);
        SceneManager.LoadScene(newScene);
    }
}
