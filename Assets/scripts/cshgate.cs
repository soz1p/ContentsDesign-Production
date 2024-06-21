using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cshgate : MonoBehaviour
{
    public TMP_Text survivalText;
    public GameObject lobbyButton;
    public Slider healthBar; // 체력바 Slider
    public TMP_Text mission;

    void Start()
    {
        survivalText.gameObject.SetActive(false);
        lobbyButton.SetActive(false);
        mission.gameObject.SetActive(false);

        StartCoroutine(ShowMissionMessage());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateSurvivalUI();
            DisableHealthBar();
        }
    }

    void ActivateSurvivalUI()
    {
        if (survivalText != null)
        {
            survivalText.gameObject.SetActive(true);
        }
        if (lobbyButton != null)
        {
            lobbyButton.SetActive(true);
        }
        Debug.Log("You Survived! Lobby button activated.");
    }

    void DisableHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowMissionMessage()
    {
        if (mission != null)
        {
            mission.gameObject.SetActive(true);
            Debug.Log("Mission complete message activated.");
            yield return new WaitForSeconds(3); // 3초 대기
            mission.gameObject.SetActive(false);
            Debug.Log("Mission complete message deactivated.");
        }
        else
        {
            Debug.LogError("Mission text is not assigned.");
        }
    }
}
