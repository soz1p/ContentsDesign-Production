using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SceneManager�� ����ϱ� ���� �߰�

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
        else
        {
            SceneManager.LoadScene("tryagain"); // ü���� 0 ���ϰ� �Ǹ� "tryagain" ������ �̵�
        }
    }

    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth;
        fillImage.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
}
