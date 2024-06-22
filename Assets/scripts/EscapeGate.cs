using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EscapeGate : MonoBehaviour
{
    public TMP_Text survivalText;
    public GameObject lobbyButton;
    public Slider healthBar; // Ã¼·Â¹Ù Slider
    void Start()
    {
        survivalText.gameObject.SetActive(false);
        lobbyButton.SetActive(false);
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
        survivalText.gameObject.SetActive(true);
        lobbyButton.SetActive(true);
        Debug.Log("You Survived! Lobby button activated.");
    }

    void DisableHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }
}
