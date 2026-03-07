using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 50;
    [SerializeField] private GameObject bulletPrefab;

    // Singleton instance
    public static BulletPool Instance { get; private set; }

    private IObjectPool<Bullet> pool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pool = new ObjectPool<Bullet>(
            createFunc: CreateItem,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyItem,
            defaultCapacity: this.defaultCapacity,
            maxSize: this.maxSize
        );
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

    private Bullet CreateItem()
    {
        GameObject bulletObj = Instantiate(bulletPrefab);
        bulletObj.name = "Pooled Bullet";
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetPool(pool);
        bullet.gameObject.SetActive(false);
        return bullet;
    }

    private void OnGet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyItem(Bullet bullet) {
        Destroy(bullet.gameObject);
    }

    public void ShootBullet(Vector2 position, Vector2 direction, int playerNumber)
    {
        Bullet bullet = pool.Get();
        bullet.transform.position = position;
        bullet.ClearTrail();
        bullet.SetDirection(direction);
        bullet.SetPlayer(playerNumber);
    }
}
