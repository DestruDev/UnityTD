using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System;

public class MainMenu : MonoBehaviour {
    public GameObject difficultySelectPanel;
    public GameObject optionsPanel;
    public GameObject changelogPanel;

    public Button changelogButton;
    public Button difficultyScreenButton;
    public Button optionsButton;
    public Button exitButton;

    public Button closeDifficultyButton;
    public Button closeOptionsButton;
    public Button closeChangelogButton;

    public Button beginnerButton;
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;

    public TextMeshProUGUI changelogText;
    public GameObject optionsGameplay;
    public GameObject optionsVideo;
    public Button Tab1;
    public Button Tab2; 
    

    public int isFullscreen = 0;

    public TMP_Dropdown displayModeDropdown;
    void Start() {
        // Load saved fullscreen setting from PlayerPrefs (0 = Windowed by default)
        isFullscreen = PlayerPrefs.GetInt("Fullscreen", 0);
        displayModeDropdown.value = isFullscreen;
        displayModeDropdown.RefreshShownValue();
        // Apply setting using the combined function
        DisplayDropDown(isFullscreen);

        
        

        // Changelog
        UpdateChangelogText();
    }

    public void Awake() {
        //default active states
        optionsPanel.SetActive(false);
        optionsGameplay.SetActive(true);
        optionsVideo.SetActive(false);
        difficultySelectPanel.SetActive(false);
        changelogPanel.SetActive(false);


    }

    

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (difficultySelectPanel.activeSelf) {
                ClosePanel(difficultySelectPanel);
            } else if (optionsPanel.activeSelf) {
                ClosePanel(optionsPanel);
            } else if (changelogPanel.activeSelf) {
                ClosePanel(changelogPanel);
            }
        }
    }

    public void TogglePanel(GameObject panel) {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);

        if (panel == difficultySelectPanel) {
            optionsButton.gameObject.SetActive(isActive);
            changelogButton.gameObject.SetActive(isActive);
        } else if (panel == optionsPanel) {
            difficultyScreenButton.gameObject.SetActive(isActive);
            changelogButton.gameObject.SetActive(isActive);
        } else if (panel == changelogPanel) {
            difficultyScreenButton.gameObject.SetActive(isActive);
            optionsButton.gameObject.SetActive(isActive);
        }
    }
    public void ClosePanel(GameObject panel) {
        panel.SetActive(false);

        if (panel == difficultySelectPanel) {
            optionsButton.gameObject.SetActive(true);
            changelogButton.gameObject.SetActive(true);
        } else if (panel == optionsPanel) {
            difficultyScreenButton.gameObject.SetActive(true);
            changelogButton.gameObject.SetActive(true);

            
            optionsGameplay.SetActive(true);
            optionsVideo.SetActive(false);

            
            SetButtonBrightness(Tab1, 0.40f); // active
            SetButtonBrightness(Tab2, 0.32f); // inactive
        } else if (panel == changelogPanel) {
            difficultyScreenButton.gameObject.SetActive(true);
            optionsButton.gameObject.SetActive(true);
        }
    }

    public void SelectDifficulty(int difficultyLevel) {


        // Set the difficulty and update the player lives
        int livesForDifficulty = GameStats.Instance.SetDifficulty(difficultyLevel);

        // Update the GameManager's playerLives using the property
        GameStats.Instance.PlayerLives = livesForDifficulty;
        // Save to PlayerPrefs
        PlayerPrefs.SetInt("Difficulty", difficultyLevel);
        PlayerPrefs.Save();


    }
    public void SwitchOptionScreen(Button button) {
        // Set screen visibility based on selected tab
        if (button == Tab1) {
            optionsGameplay.SetActive(true);
            optionsVideo.SetActive(false);
        } else if (button == Tab2) {
            optionsVideo.SetActive(true);
            optionsGameplay.SetActive(false);
        }

        // Brightness values
        float activeV = 0.40f;
        float inactiveV = 0.32f;

        // Set brightness of Tab1
        if (button == Tab1) {
            SetButtonBrightness(Tab1, activeV);
        } else {
            SetButtonBrightness(Tab1, inactiveV);
        }

        // Set brightness of Tab2
        if (button == Tab2) {
            SetButtonBrightness(Tab2, activeV);
        } else {
            SetButtonBrightness(Tab2, inactiveV);
        }
    }
    public void DisplayDropDown(int index) {
        isFullscreen = index;

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

        PlayerPrefs.SetInt("Fullscreen", index);
        PlayerPrefs.Save();
    }

    public void ExitGame() {
        Application.Quit();
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
    public void UpdateChangelogText() {
        changelogText.text =

            "v0.1.5 (2025-07-03)\n" +
            "\t\u2022 If there's a tower selected, buying another tower now unselects it.\n" +
            "\t\u2022 Changed the logo of the game executable (.exe).\n" +
            "\t\u2022 Added PlayerPrefs save for the Auto Skip Waves option in Settings.\n" +
            "\t\u2022 Added build restriction areas outside the map boundaries.\n" +
            "\t\u2022 Holding Shift now makes the player run faster and speeds up the run animation.\n" +
            "\t\u2022 Added new tooltips for player controls.\n" +
            "\t\u2022 Fullscreen/windowed setting now saves and persists between scenes.\n" +
            "\t\u2022 Pressing ESC now closes windows and cancels tower placement.\n" +
            "\t\u2022 Closing the Options menu in the main menu resets the active tab.\n" +
            "\t\u2022 Clicking Settings while placing a tower now cancels placement.\n" +
            "\t\u2022 Added build restrictions over UI panels to prevent overlap.\n" +
            "\t\u2022 Fixed sprite collider sizes and placements.\n" +
            "\t\u2022 Speed-up now only affects enemies and towers, not the player.\n" +
            "\t\u2022 Added visual indicator to the Auto Skip Waves button.\n\n" +

            "v0.1.4 (2025-06-11)\n" +
            "\t\u2022 Fixed changelog pages' visible scroll.\n\n" +



            "v0.1.3 (2025-05-30)\n" +
            "\t\u2022 Fullscreen toggle changed to dropdown.\n" +
            "\t\u2022 Main menu buttons hooked via Inspector OnClick instead of code.\n" +
            "\t\u2022 Added loading screen with progress bar.\n" +
            "\t\u2022 Fixed canvas rendering and display issues.\n" +
            "\t\u2022 Animated PNG UI for player stats (gold, lives, etc).\n" +
            "\t\u2022 Adjusted variable access levels for encapsulation.\n" +
            "\t\u2022 Added new font and redesigned UI panels.\n" +
            "\t\u2022 Button press animations added.\n" +
            "\t\u2022 New background image for main menu.\n" +
            "\t\u2022 New map layout using a 32x32 tilemap.\n" +
            "\t\u2022 Tower placement restricted via customizable tilemap grid.\n" +
            "\t\u2022 Enemy waypoint object size now snaps to grid.\n" +
            "\t\u2022 Towers can only be placed within the player�fs interact range.\n" +
            "\t\u2022 Replaced top-right close (X) button with bottom-right back button.\n" +
            "\t\u2022 Improved visuals for options menu tab layout.\n" +
            "\t\u2022 Automatic tile border tiling system added.\n" +
            "\t\u2022 New walking animation for player.\n" +
            "\t\u2022 Camera now follows the player as they move.\n" +
            "\t\u2022 Enemies are now sortable by strength in the Inspector.\n" +
            "\t\u2022 Adjusted length of enemy death animations.\n" +
            "\t\u2022 Added 8 new enemy types with unique stats and walk/death animations.\n" +
            "\t\u2022 Player has a toggle for interact range visibility.\n" +
            "\t\u2022 Player can now walk behind/in front of trees and objects.\n" +
            "\t\u2022 Towers now feature animations.\n" +
            "\t\u2022 Introduced 4 new custom-sprited towers:\n" +
            "\t\t Rock Thrower\n" +
            "\t\t Club Grunt\n" +
            "\t\t Firestarter\n" +
            "\t\t Slingshotter\n" +
            "\t\u2022 Rebalanced tower stats and wave difficulty.\n" +
            "\t\u2022 Win screen added for when all waves are completed.\n" +
            "\t\u2022 Fixed Restart button functionality.\n" +
            "\t\u2022 Fixed Sell Price UI bug - no longer resets or displays incorrect value.\n\n" +

            "v0.1.2 (2025-04-17)\n" +
            "\t\u2022 In-game settings button/panel added.\n" +
            "\t\u2022 Back to main menu button implemented.\n" +
            "\t\u2022 Upgrading towers now increases their sell price.\n" +
            "\t\u2022 Close settings button added.\n" +
            "\t\u2022 Option to autostart waves added in settings.\n" +
            "\t\u2022 Deprecated scripts removed.\n" +
            "\t\u2022 Reorganized folders in the inspector for better structure.\n" +
            "\t\u2022 Enemies and towers now instantiate under respective containers.\n\n" +

            "v0.1.1 (2025-04-15)\n" +
            "\t\u2022 Tower level now starts at 1 by default.\n" +
            "\t\u2022 Sell Tower button now sells for 50% of the tower's cost.\n" +
            "\t\u2022 Changed NextWave button to a play icon.\n" +
            "\t\u2022 Added visual indicator for active option tab.\n\n" +

            "v0.1.0 (2025-04-14)\n" +
            "\t\u2022 <b>First playable prototype of the game!</b>\n" +
            "\t\u2022 A significant amount of bug fixes/code revamping relating to towers, enemies, UI, etc.\n" +
            "\t\u2022 Changes include but are not limited to:\n" +
            "\t\t- Clicking outside the tower unselects it.\n" +
            "\t\t- Fixed restricted area placement logic.\n" +
            "\t\t- Updated tower range colors across prefabs for consistency.\n" +
            "\t\t- Targeting modes 'GetFirst()' and 'GetLast()' replaced with distance traveled.\n" +
            "\t\t- Upgrade panel now closes when clicking outside.\n" +
            "\t\t- Placing a tower now disables the upgrade panel by default.\n" +
            "\t\t- Clicking a tower shows its range indicator.\n" +
            "\t\t- Towers are now upgradable through the panel.\n\n" +

            "v0.0.9 (2025-04-12)\n" +
            "\t\u2022 Restricted areas for placing towers added.\n" +
            "\t\u2022 Revamped tower placing code.\n\n" +

            "v0.0.8 (2025-04-11)\n" +
            "\t\u2022 Towers now face the direction of the enemy they're targeting.\n" +
            "\t\u2022 Added different tower targeting options (First, Last, Strong).\n" +
            "\t\u2022 Updated visual for tower range indicator.\n\n" +

            "v0.0.7 (2025-04-10)\n" +
            "\t\u2022 Set up tabs in the options menu.\n" +
            "\t\u2022 Fullscreen toggle in options menu.\n" +
            "\t\u2022 Exit button added to close out game from main menu.\n" +
            "\t\u2022 Added additional enemy types with varying health and speed.\n" +
            "\t\u2022 Introduced WaveConfig class, fully customizable in inspector.\n" +
            "\t\t- Includes enemy counts per type, wave delay, and spawn interval.\n\n" +

            "v0.0.6 (2025-04-09)\n" +
            "\t\u2022 Tower range shows when dragging tower.\n" +
            "\t\u2022 Enemy faces towards next waypoint when reaching one.\n" +
            "\t\u2022 Enemy model updated to visually display direction facing.\n" +
            "\t\u2022 Updated map PNG.\n" +
            "\t\u2022 Updated waypoint image.\n\n" +

            "v0.0.5 (2025-04-08)\n" +
            "\t\u2022 Added toggle-able upgrade panel for towers.\n" +
            "\t\u2022 Switches to a different tower's panel when clicking on another tower.\n\n" +

            "v0.0.4 (2025-04-07)\n" +
            "\t\u2022 Enemy kills now give gold.\n" +
            "\t\u2022 Wave counter.\n" +
            "\t\u2022 Towers purchasable and placeable with enough gold.\n" +
            "\t\u2022 Game over screen on lives = 0.\n\n" +

            "v0.0.3 (2025-04-06)\n" +
            "\t\u2022 Options button/panel added.\n" +
            "\t\u2022 Difficulty select screen and functionality added.\n" +
            "\t\t- Starting lives determined by difficulty chosen.\n" +
            "\t\u2022 Ability to speed up the game.\n\n" +

            "v0.0.2 (2025-04-05)\n" +
            "\t\u2022 Tower selection UI added.\n" +
            "\t\u2022 Player stats UI added.\n" +
            "\t\u2022 Main Menu added.\n" +
            "\t\t- Changelog added.\n\n" +

            "v0.0.1 (2025-04-04)\n" +
            "\t\u2022 Enemy spawns added.\n" +
            "\t\u2022 Tower targeting added.\n" +
            "\t\u2022 Range handler added.\n";
    }


}
