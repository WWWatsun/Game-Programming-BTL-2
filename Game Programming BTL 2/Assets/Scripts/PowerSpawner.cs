using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerSpawner : MonoBehaviour
{
    [SerializeField] GameObject powerBall;
    [SerializeField] PowerUps[] powerUps;

    [SerializeField] float spawnIntervals = 10f;
    [SerializeField] float spawningLoop = 0f;

    [SerializeField] float upperLimit = 3f;
    [SerializeField] float lowerLimit = -4.2f;
    [SerializeField] float leftLimit = -8.5f;
    [SerializeField] float rightLimit = 8.0f;

    [SerializeField] float radius = 0.3f;
    [SerializeField] LayerMask layer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector3 pos = GeneratePosition();
            Debug.Log($"{pos.x}, {pos.y}");
            GameObject obj = Instantiate(powerBall, pos, Quaternion.identity);
            PowerPickup pickup = obj.GetComponent<PowerPickup>();
            pickup.power = powerUps[Random.Range(0, powerUps.Length)];
            yield return new WaitForSeconds(spawnIntervals);

        }
    }

    Vector3 GeneratePosition()
    {
        int i = 0; 
        while(i<10)
        {
            Debug.Log($"Generate number {i}");
            Vector3 vec = new Vector3(Random.Range(leftLimit, rightLimit), Random.Range(lowerLimit, upperLimit), 0);
            Debug.Log($"{vec.x}, {vec.y}");
            if (IsValidSpawn(vec)) return vec;
            i++;
        }
        return Vector3.zero;
    }

    bool IsValidSpawn(Vector3 vec)
    {
        Collider2D collider = Physics2D.OverlapCircle(vec, radius);
        Debug.Log($"Valid spawn: {collider == null}");
        return collider == null;
    }
    
}
