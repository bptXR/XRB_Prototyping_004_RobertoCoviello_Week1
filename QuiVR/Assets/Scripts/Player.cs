using UnityEngine;
using Enemies;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private AudioClip[] getHitSounds;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int playerHealth)
    {
        currentHealth = playerHealth;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            Sounds();
        }
    }

    private void GameOver()
    {
        audioSource.PlayOneShot(dieSound);
        print("Game Over");
    }

    private void Sounds()
    {
        AudioClip clip = getHitSounds[Random.Range(0, getHitSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}