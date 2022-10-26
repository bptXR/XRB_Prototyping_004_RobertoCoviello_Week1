using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Enemies;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TimerRestart timer;
    [SerializeField] private Image gameOverImage;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject waveSpawner;
    [SerializeField] private Image startFade;

    [SerializeField] private AudioClip[] getHitSounds;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioSource audioSource;

    private Enemy[] _enemies;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Start()
    {
        startFade.DOFade(0, 5);
    }

    public void TakeDamage(int playerHealth)
    {
        currentHealth = playerHealth;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(GameOver());
        }
        else
        {
            Sounds(getHitSounds);
        }
    }

    private IEnumerator GameOver()
    {
        waveSpawner.SetActive(false);
        audioSource.PlayOneShot(dieSound);
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.PlayOneShot(gameOverSound);
        timerText.DOFade(1, 5);
        gameOverImage.DOFade(1, 5).OnComplete(() => timer.enabled = true);
    }

    private void Sounds(AudioClip[] clips)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
}