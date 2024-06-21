using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class multiEscapeGate : MonoBehaviourPun
{
    public TMP_Text survivalText;
    public GameObject lobbyButton;
    public Slider healthBar; // 체력바 Slider
    public string escapeGatePrefabName = "EscapeGate"; // 문 프리팹 이름
    private static GameObject escapeGateInstance; // 생성된 문 인스턴스

    void Awake()
    {
        if (survivalText != null)
        {
            survivalText.gameObject.SetActive(false);
        }
        if (lobbyButton != null)
        {
            lobbyButton.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the escape gate trigger.");
            photonView.RPC("ActivateSurvivalUI", RpcTarget.All);
        }
    }

    [PunRPC]
    void ActivateSurvivalUI()
    {
        Debug.Log("ActivateSurvivalUI called.");

        if (survivalText != null)
        {
            survivalText.gameObject.SetActive(true);
            Debug.Log("Survival text activated.");
        }
        else
        {
            Debug.LogError("Survival text is not assigned.");
        }

        if (lobbyButton != null)
        {
            lobbyButton.SetActive(true);
            Debug.Log("Lobby button activated.");
        }
        else
        {
            Debug.LogError("Lobby button is not assigned.");
        }

        DisableHealthBar();
    }

    void DisableHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
            Debug.Log("Health bar deactivated.");
        }
        else
        {
            Debug.LogError("Health bar is not assigned.");
        }
    }

    public void EnableEscapeGate()
    {
        if (escapeGateInstance == null)
        {
            Vector3 spawnPosition = new Vector3(0.93f, 6.2f, 5.1f); // 문 생성 위치를 적절히 설정
            escapeGateInstance = PhotonNetwork.Instantiate(escapeGatePrefabName, spawnPosition, Quaternion.identity);
            // Escape gate에 Collider 추가
            Collider escapeGateCollider = escapeGateInstance.GetComponent<Collider>();
            if (escapeGateCollider == null)
            {
                escapeGateCollider = escapeGateInstance.AddComponent<BoxCollider>();
                escapeGateCollider.isTrigger = true; // 트리거로 설정
            }
            Debug.Log("Escape gate object instantiated.");
        }
    }
}
