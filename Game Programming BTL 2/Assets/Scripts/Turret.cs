using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretPivotPoint;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private GameObject flash;

    private float lastShootTime = 0f;

    private Animator flashAnim;
    private const string SHOOT_TRIGGER = "Shoot";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashAnim = flash.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        lastShootTime += Time.deltaTime;
    }

    public void Rotate(float rotateValue)
    {
        turretPivotPoint.Rotate(0f, 0f, rotateValue, Space.Self);
    }

    public void Shoot(int playerNumber)
    {
        if (lastShootTime >= shootCooldown)
        {
            BulletPool.Instance.ShootBullet(shootPoint.position, shootPoint.up, playerNumber);
            lastShootTime = 0f;
            flashAnim.SetTrigger(SHOOT_TRIGGER);
            AudioManager.Instance.PlayShootSound();
        }
    }
}
