using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField]
    private MusicManagerSO musicManager;

    private Playground playground;
    private PlayerBody body;
    private AudioSource audioSource;
    private GameManager gameManager;

    private void Awake() {
        playground = FindObjectOfType<Playground>();
        body = GetComponent<PlayerBody>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable() {
        Objective.OnPowerUpStart += PlayPowerUpSFX;
        OptionUI.OnVolumeChange += ChangeSFXVolume;
        GameManager.OnGameOver += GameOver;
    }
    
    private void OnDisable() {
        Objective.OnPowerUpStart -= PlayPowerUpSFX;
        OptionUI.OnVolumeChange -= ChangeSFXVolume;
        GameManager.OnGameOver -= GameOver;
    }

    private void Start() {
        audioSource.volume = PlayerPrefs.GetFloat(OptionKey.SFX_VOLUME, 1f);
    }

    public bool DeadlyBoundaryChecker() {
        if (Utils.InsideBoundary(transform.position, playground)) return false;

        if (playground.deadlyBoundary) return true;

        return false;
    }

    private void PlayPowerUpSFX(Objective obj) => PlaySFX(musicManager.powerUpSFX);

    private void PlaySFX(AudioClip sfx) {
        audioSource.clip = sfx;
        audioSource.Play();
    }

    private void GameOver() {
        PlaySFX(musicManager.gameOverSFX);

        // Should add effect
    }

    private void ChangeSFXVolume() => audioSource.volume = musicManager.SFXVolume;

   private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            gameManager.TriggerGameOver();
        } 
        else {
            PlaySFX(musicManager.objectiveSFX);
        }
   }
}
