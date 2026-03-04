using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint1, spawnPoint2;
    [SerializeField] private GameObject player1, player2;

    private const string PLAYER_1_TAG = "Player 1";
    private const string PLAYER_2_TAG = "Player 2";

    private void Awake()
    {
        // Instantiate player 1 and capture the created instance
        var instance1 = PlayerInput.Instantiate(
            player1,
            controlScheme: PLAYER_1_TAG,
            pairWithDevice: Keyboard.current
        );
        if (instance1 != null)
        {
            instance1.transform.position = spawnPoint1.position;
            instance1.transform.rotation = spawnPoint1.rotation;
            var playerComp = instance1.GetComponent<Player>();
            if (playerComp != null) playerComp.SetPlayerNumber(1);
        }

        // Instantiate player 2 and capture the created instance (uncomment if using second player)
        InputDevice player2Device = Gamepad.current != null ? (InputDevice)Gamepad.current : (InputDevice)Keyboard.current;
        var instance2 = PlayerInput.Instantiate(
            player2,
            controlScheme: PLAYER_2_TAG,
            pairWithDevice: player2Device
        );
        if (instance2 != null)
        {
            instance2.transform.position = spawnPoint2.position;
            instance2.transform.rotation = spawnPoint2.rotation;
            var playerComp2 = instance2.GetComponent<Player>();
            if (playerComp2 != null) playerComp2.SetPlayerNumber(2);
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
}
