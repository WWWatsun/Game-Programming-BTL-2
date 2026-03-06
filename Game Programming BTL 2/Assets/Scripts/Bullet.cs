using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletVisual;
    [SerializeField] private GameObject bulletTrail;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Sprite player1Bullet;
    [SerializeField] private Sprite player2Bullet;
    [SerializeField] private int playerNumber;

    [Header("Bullet Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int bounces = 3;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 5f; // Time before auto-release

    private IObjectPool<Bullet> pool;
    private bool isReleased = false;

    private Rigidbody2D rb;
    private Vector2 direction;

    private float timeLived =  0f;
    private int bounceCount = 0;

    private CircleCollider2D circleCollider;
    private Animator explosionAnim;
    private float defaultRadius = 0.15f;
    private float explosionRadius = 0.5f;
    private const float EXPLOSION_WIND_UP = 0.15f;
    private const float EXPLOSION_WIND_DOWN = 0.5f;
    private const float EXPLOSION_DURATION = 0.1f;
    private const string EXPLODE = "Explode";

    private SpriteRenderer sr;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        explosionAnim = explosion.GetComponent<Animator>();
        sr = bulletVisual.GetComponent<SpriteRenderer>();
        trailRenderer = bulletTrail.GetComponent<TrailRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLived += Time.deltaTime;
        if (timeLived >= lifeTime && !isReleased)
        {
            StartExplosion();
        }
    }

    private void OnEnable()
    {
        // Reset state when enabled
        bulletVisual.SetActive(true);
        bounceCount = 0;
        timeLived = 0f;
        isReleased = false;
        circleCollider.enabled = true;
        circleCollider.radius = defaultRadius;
        rb.simulated = true;
        bulletTrail.SetActive(true);
    }

    private void OnDisable()
    {
        bulletVisual.SetActive(false);
        bounceCount = 0;
        timeLived = 0f;
        isReleased = false;
        rb.linearVelocity = Vector2.zero;
        bulletTrail.SetActive(false);
    }

    public void SetPool(IObjectPool<Bullet> pool)
    {
        this.pool = pool;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
        rb.linearVelocity = this.direction * speed;
        float angle = Mathf.Atan2(this.direction.y, this.direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    public void SetPlayer(int playerNumber)
    {
        //if (playerNumber == 1)
        //{
        //    sr.sprite = player1Bullet;
        //}
        //else
        //{
        //    sr.sprite = player2Bullet;
        //}
        this.playerNumber = playerNumber;
    }

    public void ClearTrail()     {
        trailRenderer.Clear();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If already released by another collision event, ignore
        if (isReleased) return;

        if (bounceCount >= bounces)
        {
            StartExplosion();
            return;
        }

        if (collision.gameObject.TryGetComponent(out Player p) ||
            collision.gameObject.TryGetComponent(out Bullet b))
        {
            StartExplosion();
            return;
        }
        else
        {
            // Manual reflection (no Vector2.Reflect)
            Vector2 normal = collision.contacts[0].normal;
            Vector2 incoming = rb.linearVelocity;

            //// Protect against zero velocity
            //if (incoming.sqrMagnitude <= 0f)
            //{
            //    // in case velocity is zero, push it away from the surface normal
            //    incoming = -normal * speed;
            //}

            Debug.Log(normal);

            float dot = Vector2.Dot(incoming, normal);
            Vector2 reflected = incoming - 2f * dot * normal;

            if (Vector3.Dot(rb.linearVelocity, normal) < 0)
            {
                rb.linearVelocity = reflected;
                float angle = Mathf.Atan2(reflected.y, reflected.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
            }

            bounceCount++;
        }
    }

    private void StartExplosion()
    {
        if (isReleased) return;
        isReleased = true;

        ClearTrail();
        AudioManager.Instance.PlayExplosionSound();

        // stop movement and disable collider to avoid extra collisions while winding up
        if (rb != null) rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        if (circleCollider != null) circleCollider.enabled = false;

        // start coroutine that handles animation, damage, and release
        StartCoroutine(ExplosionCoroutine());
    }

    private IEnumerator ExplosionCoroutine()
    {
        // Play explosion animation
        bulletVisual.SetActive(false);
        explosionAnim.SetTrigger(EXPLODE);

        yield return new WaitForSeconds(EXPLOSION_WIND_UP);
        // Destroy walls
        MapGenerator.Instance.DestroyWallAt(transform.position);

        // Check for damageable objects in the explosion radiu
        circleCollider.radius = explosionRadius;
        circleCollider.enabled = true; // enable collider to detect overlaps for damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.TryGetComponent(out Player p))
            {
                p.TakeDamage(damage * Player.multipliers[playerNumber]);
            }
        }

        yield return new WaitForSeconds(EXPLOSION_DURATION);

        circleCollider.radius = defaultRadius;

        yield return new WaitForSeconds(EXPLOSION_WIND_DOWN);

        isReleased = true;

        if (pool != null)
        {
            pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
