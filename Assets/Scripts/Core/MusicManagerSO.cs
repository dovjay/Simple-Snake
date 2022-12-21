using UnityEngine;
using System;

[CreateAssetMenu(fileName = "MusicManager", menuName = "Music Manager/Create Music Manager", order = 0)]
public class MusicManagerSO : ScriptableObject
{
    [Header("BGM")]
    [Range(0f, 1f)]
    public float BGMVolume;
    public AudioClip menuBGM;
    public AudioClip powerUpBGM;
    public AudioClip mainBGM;
    public AudioClip gameOverBGM;

    [Header("SFX")]
    [Range(0f, 1f)]
    public float SFXVolume;
    public AudioClip objectiveSFX;
    public AudioClip powerUpSFX;
    public AudioClip gameOverSFX;

    private void OnEnable() {
        BGMVolume = PlayerPrefs.GetFloat(OptionKey.BGM_VOLUME, 1f);
        SFXVolume = PlayerPrefs.GetFloat(OptionKey.SFX_VOLUME, 1f);
    }
}
