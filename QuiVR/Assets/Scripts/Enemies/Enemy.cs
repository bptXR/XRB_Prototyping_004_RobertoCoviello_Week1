using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private GameObject _player;
        private NavMeshAgent _enemy;
        private Transform _playerTransform;
        private Animator _anim;
        private bool _isAttacking;

        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackIndex = Animator.StringToHash("AttackIndex");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int DieIndex = Animator.StringToHash("DieIndex");
        private static readonly int Walk = Animator.StringToHash("Walk");

        private void Awake()
        {
            _enemy = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = _player.transform;
            _anim = GetComponent<Animator>();
        }

        private void Update()
        {
            _enemy.SetDestination(_playerTransform.position);

            if (!_isAttacking) return;
            _anim.SetInteger(AttackIndex, Random.Range(0, 7));
            _anim.SetTrigger(Attack);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _enemy.isStopped = true;
            _isAttacking = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            _enemy.isStopped = false;
            _isAttacking = false;
            _anim.SetTrigger(Attack);
        }
    }
}