using UnityEngine;

public class PowerPickup : MonoBehaviour
{

    public PowerUps power;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            power.Activate(p);
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = power.icon;
            sr.color = power.color;
        }
        else
        {
            Debug.Log("Failed to find sr");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
