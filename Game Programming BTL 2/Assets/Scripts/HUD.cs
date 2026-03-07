using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [Header("Scene Indexes")]
    [SerializeField] private int mainMenuSceneIndex = 0;
    [SerializeField] private int gameSceneIndex = 1;

    [Header("References")]
    [SerializeField] private Slider player1Health;
    [SerializeField] private Slider player2Health;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private GameObject tutorialScreen;
    [SerializeField] private TMP_Text player1PowerUp;
    [SerializeField] private TMP_Text player2PowerUp;

    // Singleton instance
    public static HUD Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (tutorialScreen != null)
        {
            tutorialScreen.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }

        if (winScreen != null) winScreen.SetActive(false);

        if (player1PowerUp != null) player1PowerUp.text = "";
        if (player2PowerUp != null) player2PowerUp.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerHealth(int playerNumber, int health)
    {
        if (player1Health != null && playerNumber == 1)
        {
            // Update player 1 health UI
            player1Health.value = health;
        }
        else if (player2Health != null && playerNumber == 2)
        {
            // Update player 2 health UI
            player2Health.value = health;
        }
    }

    public void UpdatePlayerPowerUp(int playerNumber, string powerUp)
    {
        if (playerNumber == 1 && player1PowerUp != null)
        {
            player1PowerUp.text = powerUp;
        }
        else if (playerNumber == 2 && player2PowerUp != null)
        {
            player2PowerUp.text = powerUp;
        }
    }

    public void ShowWinScreen(int winningPlayer)
    {
        winScreen.SetActive(true);
        winText.text = $"Player {winningPlayer} Wins!";
    }

    public void HideTutorial()
    {
        tutorialScreen.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void LoadMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void LoadGameScene()
    {
        // Reload the main game scene
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}
