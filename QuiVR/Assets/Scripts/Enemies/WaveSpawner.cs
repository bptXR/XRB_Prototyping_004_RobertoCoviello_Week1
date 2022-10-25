using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private List<Enemy> enemies = new();
        [SerializeField] private int currWave;
        [SerializeField] private int waveDuration;
        [SerializeField] private Transform[] spawnLocations;
        [SerializeField] private int spawnIndex;
        [SerializeField] private int spendAmount;

        private List<Enemy> _enemiesToSpawn = new();
        private int _waveValue;
        private float _waveTimer;
        private float _spawnInterval;
        private float _spawnTimer;

        public List<Enemy> spawnedEnemies = new();

        [SerializeField] private AudioClip[] waveStartSounds;
        [SerializeField] private AudioClip gameSound;
        [SerializeField] private AudioClip wavePauseSound;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            GenerateWave();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_spawnTimer <= 0)
            {
                if (_enemiesToSpawn.Count > 0)
                {
                    Enemy enemy = Instantiate(_enemiesToSpawn[0], spawnLocations[spawnIndex].position,
                        Quaternion.identity);
                    _enemiesToSpawn.RemoveAt(0);
                    spawnedEnemies.Add(enemy);
                    _spawnTimer = _spawnInterval;

                    if (spawnIndex + 1 <= spawnLocations.Length - 1)
                    {
                        spawnIndex++;
                    }
                    else
                    {
                        spawnIndex = 0;
                    }
                }
                else
                {
                    _waveTimer = 0;
                }
            }
            else
            {
                _spawnTimer -= Time.fixedDeltaTime;
                _waveTimer -= Time.fixedDeltaTime;
            }

            if (!(_waveTimer <= 0) || spawnedEnemies.Count > 0) return;
            currWave++;
            GenerateWave();
        }

        public void GenerateWave()
        {
            StartCoroutine(Sounds());

            _waveValue = currWave * spendAmount;
            GenerateEnemies();

            _spawnInterval = waveDuration / _enemiesToSpawn.Count;

            _waveTimer = waveDuration;
        }

        private void GenerateEnemies()
        {
            List<Enemy> generatedEnemies = new List<Enemy>();
            while (_waveValue > 0 || generatedEnemies.Count < 50)
            {
                int randEnemyId = Random.Range(0, enemies.Count);
                int randEnemyCost = enemies[randEnemyId].cost;

                if (_waveValue - randEnemyCost >= 0)
                {
                    generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                    _waveValue -= randEnemyCost;
                }
                else if (_waveValue <= 0)
                {
                    break;
                }
            }

            _enemiesToSpawn.Clear();
            _enemiesToSpawn = generatedEnemies;
        }

        private IEnumerator Sounds()
        {
            AudioClip clip = waveStartSounds[Random.Range(0, waveStartSounds.Length)];
            audioSource.PlayOneShot(clip);
            yield return new WaitWhile(() => audioSource.isPlaying);
            audioSource.PlayOneShot(gameSound);
        }
    }
}