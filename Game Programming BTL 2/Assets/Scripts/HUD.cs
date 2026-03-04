using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider player1Health;
    [SerializeField] private Slider player2Health;

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
}
