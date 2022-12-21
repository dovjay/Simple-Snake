using UnityEngine;
using UnityEngine.UI;

public class GameOptionUI : MonoBehaviour {
    [SerializeField] private Toggle deadlyBoundaryToggle;

    private void Awake() {
        deadlyBoundaryToggle.isOn = (PlayerPrefs.GetInt(OptionKey.DEADLY_BOUNDARY, 0) == 1) ? true : false;
    }

    private void OnEnable() {
        deadlyBoundaryToggle.onValueChanged.AddListener(ChangeDeadlyBoundary);
    }

    private void OnDisable() {
        deadlyBoundaryToggle.onValueChanged.RemoveListener(ChangeDeadlyBoundary);
    }

    public void ChangeDeadlyBoundary(bool toggle) {
        PlayerPrefs.SetInt(OptionKey.DEADLY_BOUNDARY, deadlyBoundaryToggle.isOn ? 1 : 0);
    }
}