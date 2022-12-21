using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField]
    private MusicManagerSO musicManager;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        GameManager.OnGameOver += PlayGameOverBGM;
        Objective.OnPowerUpStart += PlayPowerUpBGM;
        PowerUpManager.OnPowerUpEnd += PlayGameBGM;
        OptionUI.OnVolumeChange += ChangeBGMVolume;
    }

    private void OnDisable() {
        GameManager.OnGameOver -= PlayGameOverBGM;
        Objective.OnPowerUpStart -= PlayPowerUpBGM;
        PowerUpManager.OnPowerUpEnd -= PlayGameBGM;
        OptionUI.OnVolumeChange -= ChangeBGMVolume;
    }

    void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat(OptionKey.BGM_VOLUME, 1f);

        SwitchBGM();
    }

    private void SwitchBGM() {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                PlayMenuBGM();
                break;
            case 1:
            case 2:
            case 3:
                PlayGameBGM();
                break;
            default:
                break;
        }
    }

    public void ChangeBGMVolume() => audioSource.volume = musicManager.BGMVolume;

    public void PlayMenuBGM() => PlayBGM(musicManager.menuBGM);

    public void PlayGameBGM() => PlayBGM(musicManager.mainBGM);

    public void PlayPowerUpBGM(Objective obj) => PlayBGM(musicManager.powerUpBGM);

    public void PlayGameOverBGM() {
        audioSource.loop = false;
        PlayBGM(musicManager.gameOverBGM);
    }

    public void PlayBGM(AudioClip bgm) {
        audioSource.clip = bgm;
        audioSource.Play();
    }
}
