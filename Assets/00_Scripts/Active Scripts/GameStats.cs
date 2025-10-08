using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public enum GameDifficulty {
    Beginner,
    Easy,
    Normal,
    Hard
}
public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }
    public float playerGold = 0;
    [HideInInspector] public int waveCounter = 1;
    private int playerLives;
    // UI References
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI waveText;
    [HideInInspector] public int maxWaves;
    public GameObject coinIcon;
    public GameObject heartIcon;
    
    

    private GameDifficulty currentDifficulty = GameDifficulty.Beginner;

    
    public int PlayerLives {
        // Public property to access playerLives
        get { return playerLives; }
        set { playerLives = value; }
    }
    private void Awake() {
   
        // Singleton pattern
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        UpdateGoldText();
        UpdateLivesText();
        UpdateWaveText();
    }
    private void Start() {
        UpdateGoldText();
        UpdateWaveText();
        int difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        SetDifficulty(difficulty);
    }


    #region Player Stats
    public void AddGold(int amount) {
        playerGold += amount;
        UpdateGoldText();

        coinIcon.GetComponent<PopAnimation>().PlayPop();
        
    }

    public void LoseLife(int amount) {
        playerLives -= amount;
        UpdateLivesText();

        if (playerLives <= 0) {
            GameManager.Instance.GameOver();
        } else {
            heartIcon.GetComponent<PopAnimation>().PlayPop();
        }
    }



    public void UpdateGoldText() {
        if (goldText != null) {
            goldText.text = $"${playerGold}";
        }
    }

    public void UpdateLivesText() {
        if (livesText != null) {
            livesText.text = $"{playerLives}";
        }
    }

    public void UpdateWaveText() {
        if (waveText != null) {
            waveText.text = $"Wave: {waveCounter}/{maxWaves}";
        }
    }
    #endregion
    public int SetDifficulty(int difficultyLevel) {
        //set the difficulty and lives based on the difficulty level
        switch (difficultyLevel) {
            case 1:
                currentDifficulty = GameDifficulty.Beginner;
                playerLives = 250;
                break;
            case 2:
                currentDifficulty = GameDifficulty.Easy;
                playerLives = 150;
                break;
            case 3:
                currentDifficulty = GameDifficulty.Normal;
                playerLives = 100;
                break;
            case 4:
                currentDifficulty = GameDifficulty.Hard;
                playerLives = 50;
                break;
            default:
                currentDifficulty = GameDifficulty.Beginner;
                playerLives = 250;
                break;
        }

        // Update the difficulty UI
        if (difficultyText != null) {
            difficultyText.text = $"Difficulty: {currentDifficulty}";
        }

        UpdateLivesText();



        return playerLives;
    }


    public void RestartGame() {
        SceneManager.LoadScene("GameScene");
    }

    
}

