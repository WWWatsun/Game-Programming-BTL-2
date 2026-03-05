using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Turret turret;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private GameObject explosionVisual;
    
    [Header("Player settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float health = 100f;

    private Vector2 moveInput;
    private float rotateInput;
    private Rigidbody2D rigidBody;

    private int playerNumber = 1;
    private Animator explosionAnim;
    private const string EXPLOSION_TRIGGER = "Explode";
    private const float EXPLOSION_DURATION = 0.35f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        explosionAnim = explosionVisual.GetComponent<Animator>();
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
        AudioManager.Instance.PlayHurtSound();
        if (health <= 0f)
        {
            StartCoroutine(Death());
        }
    }

    public void SetPlayerNumber(int number)
    {
        playerNumber = number;
    }

    private IEnumerator Death()
    {
        Debug.Log($"{gameObject.name} has been defeated!");
        health = 0f;
        HUD.Instance.ShowWinScreen(playerNumber == 1 ? 2 : 1);
        playerVisual.SetActive(false);

        explosionAnim.SetTrigger(EXPLOSION_TRIGGER);
        AudioManager.Instance.PlayExplosionSound();
        Music.Instance.PlayVictoryMusic();
        yield return new WaitForSeconds(EXPLOSION_DURATION);

        Destroy(gameObject);
    }
}
