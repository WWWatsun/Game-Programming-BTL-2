using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player1, player2;

    private const string PLAYER_1_TAG = "Player 1";
    private const string PLAYER_2_TAG = "Player 2";

    // Store the Player 2 input reference so we can switch its devices later
    private PlayerInput player2Input;

    private void OnEnable()
    {
        // Subscribe to device change events
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        // Always unsubscribe to prevent memory leaks
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void Start()
    {
        Vector2 spawnPoint1 = MapGenerator.Instance.SpawnPoint1;
        Vector2 spawnPoint2 = MapGenerator.Instance.SpawnPoint2;

        // Instantiate player 1
        var instance1 = PlayerInput.Instantiate(
            player1,
            controlScheme: PLAYER_1_TAG,
            pairWithDevice: Keyboard.current
        );
        if (instance1 != null)
        {
            instance1.transform.position = spawnPoint1;
            var playerComp = instance1.GetComponent<Player>();
            if (playerComp != null) playerComp.SetPlayerNumber(1);
        }

        // Instantiate player 2
        InputDevice player2Device = GetPlayer2Device();
        player2Input = PlayerInput.Instantiate(
            player2,
            controlScheme: PLAYER_2_TAG,
            pairWithDevice: player2Device
        );
        if (player2Input != null)
        {
            player2Input.transform.position = spawnPoint2;
            var playerComp2 = player2Input.GetComponent<Player>();
            if (playerComp2 != null) playerComp2.SetPlayerNumber(2);
        }
    }

    private InputDevice GetPlayer2Device()
    {
        if (Gamepad.current != null) return Gamepad.current;

        if (Keyboard.current != null)
        {
            Debug.LogWarning("Gamepad not available at start. Player 2 will use keyboard.");
            return Keyboard.current;
        }

        Debug.LogError("No input devices available for Player 2!");
        return null;
    }

    // This method fires whenever a device is plugged in or unplugged
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        // Make sure Player 2 actually exists before trying to change its controls
        if (player2Input == null) return;

        switch (change)
        {
            case InputDeviceChange.Disconnected:
                // If the disconnected device was the one Player 2 was using
                if (device == player2Input.devices[0] && device is Gamepad)
                {
                    Debug.LogWarning("Gamepad disconnected! Switching Player 2 to Keyboard.");
                    player2Input.SwitchCurrentControlScheme(PLAYER_2_TAG, Keyboard.current);
                }
                break;

            case InputDeviceChange.Added:
            case InputDeviceChange.Reconnected:
                // If a gamepad is plugged back in, give it back to Player 2
                if (device is Gamepad)
                {
                    Debug.Log("Gamepad connected! Switching Player 2 back to Gamepad.");
                    player2Input.SwitchCurrentControlScheme(PLAYER_2_TAG, device);
                }
                break;
        }
    }
}