using UnityEngine;
using Enemies;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    [SerializeField] private HealthBar healthBar;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int playerHealth)
    {
        currentHealth = playerHealth;
        healthBar.SetHealth(currentHealth);

        if (currentHealth > 0) return;
        GameOver();
    }

    private void GameOver()
    {
        print("Game Over");
    }
}