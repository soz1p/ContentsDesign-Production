using UnityEngine;

public class Compass : MonoBehaviour
{
    public GameObject escapeGatePrefab; // 탈출 게이트 프리팹
    public Vector3 escapeGateSpawnOffset = new Vector3(0.93f, 6.2f, 5.1f); // 나침반에서 탈출 게이트까지의 오프셋

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnEscapeGate();
            Destroy(gameObject);
        }
    }

    void SpawnEscapeGate()
    {
        if (escapeGatePrefab != null)
        {
            Instantiate(escapeGatePrefab, escapeGateSpawnOffset, Quaternion.identity);
            Debug.Log("Escape gate spawned at: " + escapeGateSpawnOffset);
        }
        else
        {
            Debug.LogError("Escape gate prefab is not assigned in the Compass script.");
        }
    }
}
