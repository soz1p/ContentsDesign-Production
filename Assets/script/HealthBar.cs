using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient gradient;
    public Image fillImage;
    public float decreaseRate = 1f; // Adjust this to control how fast the health decreases

    private float currentHealth;

    void Start()
    {
        currentHealth = healthSlider.maxValue;
        UpdateHealthBar();
    }

    void Update()
    {
        if (currentHealth > 0)
        {
            currentHealth -= decreaseRate * Time.deltaTime;
            UpdateHealthBar();
        }
    }

    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
        fillImage.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
}
