using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EscapeGate : MonoBehaviour
{
    public TMP_Text survivalText;
    public GameObject lobbyButton;
    public Slider healthBar; // Ã¼·Â¹Ù Slider
    public EscapeGate gateobject;

    void Start()
    {
        survivalText.gameObject.SetActive(false);
        lobbyButton.SetActive(false);
        gateobject.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gateobject.gameObject.SetActive(true);
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
