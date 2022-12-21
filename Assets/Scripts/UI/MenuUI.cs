using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private Canvas optionCanvas;

    [SerializeField]
    private Canvas modeSelectionCanvas;

    [SerializeField]
    private Canvas leaderboardCanvas;

    [SerializeField]
    private Canvas quitPrompt;

    public void PlayClassicMode() => SceneManager.LoadScene(1);

    public void PlayAllNewMode() => SceneManager.LoadScene(2);

    public void PlayTimeTrialMode() => SceneManager.LoadScene(3);

    public void Quit() => Application.Quit();

    public void TriggerQuitPrompt() => quitPrompt.gameObject.SetActive(!quitPrompt.gameObject.activeSelf);

    public void OpenOption() => optionCanvas.gameObject.SetActive(true);

    public void CloseOption() => optionCanvas.gameObject.SetActive(false);

    public void OpenSelectionMode() => modeSelectionCanvas.gameObject.SetActive(true);

    public void CloseSelectionMode() => modeSelectionCanvas.gameObject.SetActive(false);

    public void TriggerLeaderboard() => leaderboardCanvas.gameObject.SetActive(!leaderboardCanvas.gameObject.activeSelf);
}
