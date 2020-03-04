using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolController : MonoBehaviour
{
    [SerializeField]
    private float _damage = 50.0f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextShotTime;
    [SerializeField]
    private float _shotAnimTime = 0.17f;

    [SerializeField]
    private GameObject _muzzleFlash;
    private Renderer _flashRenderer;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _flashRenderer = _muzzleFlash.GetComponent<Renderer>();
        _flashRenderer.enabled = false;
        _nextShotTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleFire(Vector3 direction)
    {
        if (Time.time < _nextShotTime) return;

        _nextShotTime = Time.time + _fireRate;

        _animator.SetBool("Shoot_b", true);
        _flashRenderer.enabled = true;

        StartCoroutine(EndShot());
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    IEnumerator EndShot()
    {
        yield return new WaitForSeconds(_shotAnimTime);

        _animator.SetBool("Shoot_b", false);
        _flashRenderer.enabled = false;
    }
}