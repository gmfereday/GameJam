using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private float _minHealth = 55.0f;
    [SerializeField]
    private float _maxHealth = 200.0f;
    private float _health;
    [SerializeField]
    private float _minRunSpeed = 2.0f;
    [SerializeField]
    private float _maxRunSpeed = 8.0f;
    private float _runSpeed;
    [SerializeField]
    private float _walkSpeed = 2.0f;
    [SerializeField]
    private float _minDamage = 5.0f;
    [SerializeField]
    private float _maxDamage = 20.0f;
    private float _damage;
    [SerializeField]
    private float _deadDespawnRange = 60.0f;
    [SerializeField]
    private float _despawnRange = 150.0f;
    [SerializeField]
    private float _lookRadius = 20.0f;
    [SerializeField]
    private float _chaseLookRadius = 35.0f;
    [SerializeField]
    private float _fov = 90.0f;
    [SerializeField]
    private float _rotationSpeed = 5.0f;
    [SerializeField]
    private float _attackDelay = 1.0f;
    private float _nextAttackTime;

    private Vector3 _targetPos;
    private Transform _playerPos;
    private PlayerController _playerScript;

    private Animator _animator;

    private NavMeshAgent _agent;

    private ScoreController _scoreCtrl;

    private bool _dead = false;

    enum ZombieState
    {
        WANDERING,
        CHASING
    }

    ZombieState _currentState = ZombieState.WANDERING;

    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();
        _animator = this.GetComponent<Animator>();
        _agent = this.GetComponent<NavMeshAgent>();
        _playerPos = PlayerManager.instance.player.transform;
        _playerScript = PlayerManager.instance.player.GetComponent<PlayerController>();
        _nextAttackTime = Time.time;

        _health = Random.Range(_minHealth, _maxHealth);
        _runSpeed = Random.Range(_minRunSpeed, _maxRunSpeed);
        _damage = Random.Range(_minDamage, _maxDamage);

        _agent.speed = _walkSpeed;

        //Vector3 initialTarget = this.transform.position + new Vector3(1, 0, 1);
        //_target = this.transform;
        //_target.position = initialTarget;

        _targetPos = this.transform.position;

        if (!_agent.isOnNavMesh) Destroy(this.gameObject);
    }

    private void Update()
    {
        ManageMovement();
    }

    private void FixedUpdate()
    {
        HandleDespawn();
    }

    private void ManageMovement()
    {
        if (!_agent.enabled) return;

        float playerDist = 0.0f;

        switch (_currentState)
        {
            case ZombieState.WANDERING:
                float targetDist = Vector3.Distance(this.transform.position, _targetPos);

                if (targetDist <= _agent.stoppingDistance)
                {
                    float newX = Random.Range(-_lookRadius, _lookRadius);
                    float newZ = Random.Range(-_lookRadius, _lookRadius);

                    _targetPos.x += newX;
                    _targetPos.y = this.transform.position.y;
                    _targetPos.z += newZ;

                    _agent.SetDestination(_targetPos);
                }

                playerDist = Vector3.Distance(this.transform.position, _playerPos.position);

                if (playerDist <= _lookRadius)
                {
                    Vector3 toPlayer = (_playerPos.position - this.transform.position).normalized;
                    float angle = Mathf.Abs(Vector3.Angle(this.transform.forward, toPlayer));

                    if (angle <= _fov / 2.0)
                    {
                        _currentState = ZombieState.CHASING;
                        _agent.SetDestination(_playerPos.position);
                        _agent.speed = _runSpeed;
                    }
                }
                break;

            case ZombieState.CHASING:
                _agent.SetDestination(_playerPos.position);

                playerDist = Vector3.Distance(this.transform.position, _playerPos.position);

                if (playerDist <= _agent.stoppingDistance)
                {
                    Vector3 direction = (_playerPos.position - this.transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);

                    if (Time.time > _nextAttackTime)
                    {
                        _nextAttackTime = Time.time + _attackDelay;
                        StartCoroutine(_playerScript.TakeDamage(_damage));
                    }
                }
                else if (playerDist > _chaseLookRadius)
                {
                    _currentState = ZombieState.WANDERING;
                    _agent.speed = _walkSpeed;

                    float newX = Random.Range(-_lookRadius, _lookRadius);
                    float newZ = Random.Range(-_lookRadius, _lookRadius);

                    _targetPos.x += newX;
                    _targetPos.y = this.transform.position.y;
                    _targetPos.z += newZ;

                    _agent.SetDestination(_targetPos);
                }
                break;
        }        
    }

    private void DisableRagdoll()
    {
        Rigidbody[] rigidBodies = this.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
        }
    }

    private void EnableRagdoll()
    {
        _animator.enabled = false;
        _agent.enabled = false;

        Rigidbody[] rigidBodies = this.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }
    }

    private void HandleDespawn()
    {
        float dist = Vector3.Distance(this.transform.position, _playerPos.position);

        if (_health <= 0 && dist >= _deadDespawnRange) Destroy(this.gameObject);

        else if (dist >= _despawnRange) Destroy(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        if (_dead) return;

        _health -= damage;

        _currentState = ZombieState.CHASING;

        if (_agent.enabled)
        {
            _agent.SetDestination(_playerPos.position);
            _agent.speed = _runSpeed;
        }

        if (_health <= 0)
        {
            _dead = true;
            EnableRagdoll();
            _scoreCtrl.IncrementScore();
        }
    }

    public void SetScoreController(ScoreController ctrl)
    {
        _scoreCtrl = ctrl;
    }
}