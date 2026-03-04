using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Turret turret;
    
    [Header("Player settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float health = 100f;

    private Vector2 moveInput;
    private float rotateInput;
    private Rigidbody2D rigidBody;

    private int playerNumber = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
        HandleRotate();
    }

    private void HandleMove()
    {
        rigidBody.linearVelocity = moveInput.y * transform.up * moveSpeed;

        // If the player is moving backwards, we want to flip the rotation direction
        float rotationDirection = moveInput.y < 0 ? -1f : 1f;

        rigidBody.MoveRotation(rigidBody.rotation - moveInput.x * rotationDirection * rotationSpeed * Time.deltaTime);
    }

    private void HandleRotate()
    {
        turret.Rotate(-rotateInput * rotationSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<float>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            turret.Shoot(playerNumber);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        HUD.Instance.UpdatePlayerHealth(playerNumber, (int)health);
        if (health <= 0f)
        {
            // Death logic
            Debug.Log($"{gameObject.name} has been defeated!");
            Destroy(gameObject);
        }
    }

    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
    }
}
