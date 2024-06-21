using UnityEngine;

public class cshCompassSpawner : MonoBehaviour
{
    public GameObject compassPrefab;
    public Vector3 initialPosition = new Vector3(14f, 5f, 15f);
    public float randomRange = 3f;

    void Start()
    {
        Vector3 randomPosition = initialPosition + new Vector3(
            Random.Range(-randomRange, randomRange),
            0f,
            Random.Range(-randomRange, randomRange)
        );
        Instantiate(compassPrefab, randomPosition, Quaternion.identity);
    }
}
