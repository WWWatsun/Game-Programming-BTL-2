using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider player1Health;
    [SerializeField] private Slider player2Health;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text winText;

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
        winScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerHealth(int playerNumber, int health)
    {
        if (playerNumber == 1)
        {
            // Update player 1 health UI
            player1Health.value = health;
        }
        else if (playerNumber == 2)
        {
            // Update player 2 health UI
            player2Health.value = health;
        }
    }

    public void ShowWinScreen(int winningPlayer)
    {
        winScreen.SetActive(true);
        winText.text = $"Player {winningPlayer} Wins!";
    }

    public void Replay()
    {
        // Reload the current scene to restart the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
