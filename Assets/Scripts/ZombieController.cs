using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private float _health = 100.0f;
    [SerializeField]
    private float _deadDespawnRange = 60.0f;
    [SerializeField]
    private float _despawnRange = 150.0f;

    private GameObject _player;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        HandleDespawn();
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

        Rigidbody[] rigidBodies = this.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;
        }
    }

    private void HandleDespawn()
    {
        float dist = Vector3.Distance(this.transform.position, _player.transform.position);

        if (_health <= 0 && dist >= _deadDespawnRange) Destroy(this.gameObject);

        else if (dist >= _despawnRange) Destroy(this.gameObject);
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
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
