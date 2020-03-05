using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private float _health = 100.0f;
    [SerializeField]
    private float _damage = 10.0f;
    [SerializeField]
    private float _deadDespawnRange = 60.0f;
    [SerializeField]
    private float _despawnRange = 150.0f;
    [SerializeField]
    private float _lookRadius = 15.0f;
    [SerializeField]
    private float _rotationSpeed = 5.0f;
    [SerializeField]
    private float _attackDelay = 1.0f;
    private float _nextAttackTime;

    private Transform _target;
    private Transform _player;
    private PlayerController _playerScript;

    private Animator _animator;

    private NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();
        _animator = this.GetComponent<Animator>();
        _agent = this.GetComponent<NavMeshAgent>();
        _target = PlayerManager.instance.player.transform;
        _player = PlayerManager.instance.player.transform;
        _playerScript = _player.gameObject.GetComponent<PlayerController>();
        _nextAttackTime = Time.time;
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
        float dist = Vector3.Distance(this.transform.position, _player.position);

        if(dist <= _lookRadius && _agent.enabled)
        {
            _agent.SetDestination(_player.position);

            if(dist <= _agent.stoppingDistance)
            {
                Vector3 direction = (_player.position - this.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);

                if (Time.time > _nextAttackTime)
                {
                    _nextAttackTime = Time.time + _attackDelay;
                    _playerScript.TakeDamage(_damage);
                }
            }
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
        float dist = Vector3.Distance(this.transform.position, _player.position);

        if (_health <= 0 && dist >= _deadDespawnRange) Destroy(this.gameObject);

        else if (dist >= _despawnRange) Destroy(this.gameObject);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            EnableRagdoll();
        }
    }
}