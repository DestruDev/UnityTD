using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;


public class GameManager : MonoBehaviour {
    #region Singleton
    public static GameManager Instance { get; private set; }
    #endregion

    public GameObject settingsPanel;
    [HideInInspector, SerializeField]public bool autoSkipWave = false;
    private Button autoSkipWaveButton;
    public TextMeshProUGUI autoSkipWaveTextIndicator;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;
    [SerializeField] public List<Image> uiBlockerImages;
    public TMP_Dropdown displayModeDropdown;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        settingsPanel.SetActive(false);
        gameOverScreen.SetActive(false);
        
    }
    private void Start() {
        LoadSavedSettings();
    }
    public void ToMainMenu() {
        // Detect current mode and save it
        if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow) {
            PlayerPrefs.SetInt("Fullscreen", 1);
        } else {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }

        if (autoSkipWave) {
            PlayerPrefs.SetInt("AutoSkipWave", 1);
        } else {
            PlayerPrefs.SetInt("AutoSkipWave", 0);
        }
            PlayerPrefs.Save();

        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            settingsPanel.SetActive(false);
        }
    }



    public void ToggleSettingsPanel() {
        bool isActive = settingsPanel.activeSelf;
        settingsPanel.SetActive(!isActive);
    }

    public void CloseSettingsPanel() {
        settingsPanel.SetActive(false);
    }

    public void ToggleAutoSkipWave(Button button) {
        autoSkipWave = !autoSkipWave;
        float activeV = 0.80f;
        float inactiveV = 1f;

        if (autoSkipWave) {
            SetButtonBrightness(button, activeV);
            EnemySpawner.Instance.autoStart = true;
            autoSkipWaveTextIndicator.text = "TRUE";
            autoSkipWaveTextIndicator.color = Color.green;
            PlayerPrefs.SetInt("AutoSkipWave", 1);
        } else {
            SetButtonBrightness(button, inactiveV);
            EnemySpawner.Instance.autoStart = false;
            autoSkipWaveTextIndicator.text = "FALSE";
            autoSkipWaveTextIndicator.color = Color.red;
            PlayerPrefs.SetInt("AutoSkipWave", 0);
        }
        PlayerPrefs.Save();
    }

    private void SetButtonBrightness(Button tabButton, float targetV) {
        Image img = tabButton.GetComponent<Image>();
        if (img != null) {
            Color originalColor = img.color;
            Color.RGBToHSV(originalColor, out float h, out float s, out float v);
            Color newColor = Color.HSVToRGB(h, s, targetV);
            newColor.a = originalColor.a; // Preserve alpha
            img.color = newColor;
        }
    }

    public void GameOver() {
        Debug.Log("Game Over!");
        // 1. Freeze the game
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        StopAllCoroutines();
    }

    public void GameWon() {
        Debug.Log("Game Won!");
        // 1. Freeze the game
        Time.timeScale = 0f;
        gameWonScreen.SetActive(true);
        StopAllCoroutines();
    }

    public void DisplayDropDown(int _) {
        int index = displayModeDropdown.value;

        ApplyDisplaySetting(index);

        PlayerPrefs.SetInt("Fullscreen", index);
        PlayerPrefs.Save();
    }

    private void ApplyDisplaySetting(int index) {
        switch (index) {
            case 0:
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;
        }
    }

    private void LoadSavedSettings() {
        //AutoSkipWave
        int savedAutoSkip = PlayerPrefs.GetInt("AutoSkipWave", 0);
        if (savedAutoSkip == 1) {
            autoSkipWave = true;
        } else {
            autoSkipWave = false;
        }
        EnemySpawner.Instance.autoStart = autoSkipWave;
        autoSkipWaveTextIndicator.text = autoSkipWave ? "TRUE" : "FALSE";
        autoSkipWaveTextIndicator.color = autoSkipWave ? Color.green : Color.red;

        //Display mode
        int savedIndex = PlayerPrefs.GetInt("Fullscreen", 0);

        displayModeDropdown.value = savedIndex;
        displayModeDropdown.RefreshShownValue();

        DisplayDropDown(savedIndex);
    }
}