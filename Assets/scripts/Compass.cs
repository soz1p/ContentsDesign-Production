using UnityEngine;

public class Compass : MonoBehaviour
{
    public GameObject escapeGatePrefab; // Ż�� ����Ʈ ������
    public Vector3 escapeGateSpawnOffset = new Vector3(0.93f, 6.2f, 5.1f); // ��ħ�ݿ��� Ż�� ����Ʈ������ ������

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
