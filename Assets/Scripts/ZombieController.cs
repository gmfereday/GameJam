using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private float _health = 100.0f;

    private Animator _animator;

    [SerializeField]
    private CapsuleCollider _hitBox;

    // Start is called before the first frame update
    void Start()
    {
        DisableRagdoll();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DisableRagdoll()
    {
        Rigidbody[] rigidBodies = this.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
        }

        //Collider[] colliders = this.GetComponentsInChildren<Collider>();

        //foreach(Collider collider in colliders)
        //{
        //    collider.enabled = false;
        //}

        //_hitBox.enabled = true;
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

        //Collider[] colliders = this.GetComponentsInChildren<Collider>();

        //foreach (Collider collider in colliders)
        //{
        //    collider.enabled = true;
        //}

        //_hitBox.enabled = false;
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
