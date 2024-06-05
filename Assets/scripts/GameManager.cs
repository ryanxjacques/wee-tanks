using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private AudioSource _audioSource;
    public AudioClip playerDeathNoise;
    

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        _audioSource = GetComponent<AudioSource>();
        player.onDeath += HandlePlayerDeath;
    }
    
    private void HandlePlayerDeath()
    {
        Destroy(player.gameObject);
        _audioSource.PlayOneShot(playerDeathNoise,0.7f);
        StartCoroutine(ChangeSceneAfterDelay(3, "level select"));
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private IEnumerator ChangeSceneAfterDelay(float timeWait, string newScene)
    {
        yield return new WaitForSeconds(timeWait);
        SceneManager.LoadScene(newScene);
    }
}
