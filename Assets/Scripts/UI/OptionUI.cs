using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionUI : MonoBehaviour
{
    public static event Action OnVolumeChange;

    [SerializeField]
    private TMP_Dropdown controlOption;
    [SerializeField]
    private Slider BGMVolumeSlider;
    [SerializeField]
    private Slider SFXVolumeSlider;
    [SerializeField]
    private MusicManagerSO musicManager;

    private void Awake() {
        controlOption.value = PlayerPrefs.GetInt(OptionKey.CONTROL_OPTION, 0);
        BGMVolumeSlider.value = PlayerPrefs.GetFloat(OptionKey.BGM_VOLUME, 1f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat(OptionKey.SFX_VOLUME, 1f);
    }

    private void OnEnable() {
        controlOption.onValueChanged.AddListener(ChangeControl);
        BGMVolumeSlider.onValueChanged.AddListener(ChangeBGMVolume);
        SFXVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void OnDisable() {
        controlOption.onValueChanged.RemoveListener(ChangeControl);
        BGMVolumeSlider.onValueChanged.RemoveListener(ChangeBGMVolume);
        SFXVolumeSlider.onValueChanged.RemoveListener(ChangeSFXVolume);
    }

    private void ChangeBGMVolume(float v) {
        PlayerPrefs.SetFloat(OptionKey.BGM_VOLUME, v);
        musicManager.BGMVolume = v;
        OnVolumeChange?.Invoke();
    }

    private void ChangeSFXVolume(float v) {
        PlayerPrefs.SetFloat(OptionKey.SFX_VOLUME, v);
        musicManager.SFXVolume = v;
        OnVolumeChange?.Invoke();
    }

    private void ChangeControl(int option) => PlayerPrefs.SetInt(OptionKey.CONTROL_OPTION, option);
}
