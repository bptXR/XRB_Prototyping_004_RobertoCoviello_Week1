using UnityEngine;
using UnityEngine.AI;
using BowArrow;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int currentHealth;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private int damageToPlayer = 20;

        private Player _player;
        private NavMeshAgent _enemy;
        private Transform _playerTransform;
        private Animator _anim;
        private bool _isAttacking;
        private int _damageToEnemy;

        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackIndex = Animator.StringToHash("AttackIndex");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int DieIndex = Animator.StringToHash("DieIndex");
        private static readonly int Walk = Animator.StringToHash("Walk");

        private void Awake()
        {
            _enemy = GetComponent<NavMeshAgent>();
            _player = FindObjectOfType<Player>();
            _playerTransform = _player.transform;
            _anim = GetComponent<Animator>();
        }

        private void Start()
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            _enemy.speed = Random.Range(1.8f, 3.2f);
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
            _anim.SetTrigger(Walk);
        }

        public void Hit(Arrow arrow)
        {
            DisableCollider(arrow);
            TakeDamage(arrow);
        }

        private void DisableCollider(Arrow arrow)
        {
            if (arrow.TryGetComponent(out Collider collider))
                collider.enabled = false;
        }

        private void TakeDamage(Arrow arrow)
        {
            _damageToEnemy = arrow.damageToEnemy;
            currentHealth -= _damageToEnemy;
            healthBar.SetHealth(currentHealth);

            if (currentHealth > 0) return;
            KillEnemy();
        }

        private void KillEnemy()
        {
            _enemy.isStopped = true;
            _anim.SetInteger(DieIndex, Random.Range(0, 7));
            _anim.SetTrigger(Die);

            meshRenderer.materials[0].DOFade(0, 5).OnComplete(() => Destroy(gameObject));
        }

        public void DoDamage()
        {
            if (!_isAttacking) return;
            int currentPlayerHealth = _player.currentHealth - damageToPlayer;
            _player.TakeDamage(currentPlayerHealth);
        }
    }
}