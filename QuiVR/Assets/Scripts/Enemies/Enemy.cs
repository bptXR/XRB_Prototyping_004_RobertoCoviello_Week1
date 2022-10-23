using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private GameObject _player;
        private NavMeshAgent _enemy;
        private Transform _playerTransform;

        private void Awake()
        {
            _enemy = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = _player.transform;
        }

        private void Update()
        {
            _enemy.SetDestination(_playerTransform.position);
        }
    }
}
