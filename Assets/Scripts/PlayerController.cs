using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _runningSpeed = 10.0f;
    [SerializeField]
    private float _walkingSpeed = 5.0f;
    [SerializeField]
    private float _health = 100.0f;

    private Vector3 _aimDirection;

    private Animator _animator;

    private CharacterController _characterController;

    private Camera _mainCamera;

    [SerializeField]
    private GameObject _weaponRoot;
    [SerializeField]
    private GameObject[] _weaponPrefabs;

    private List<GameObject> _weapons;
    private int _currentWeapon = 0;
    private WeaponController _weaponCtrl;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _characterController = this.GetComponent<CharacterController>();
        _mainCamera = Camera.main;

        _weapons = new List<GameObject>();
        _weapons.Add(Instantiate(_weaponPrefabs[0], _weaponRoot.transform));
        _weapons.Add(Instantiate(_weaponPrefabs[1], _weaponRoot.transform));

        foreach(GameObject weapon in _weapons)
        {
            WeaponController ctrl = weapon.GetComponent<WeaponController>();
            ctrl.SetAnimator(_animator);
            Debug.Log("SetActive");
            ctrl.SetActive(false);
        }

        _weaponCtrl = _weapons[0].GetComponent<WeaponController>();
        _weaponCtrl.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleWeapon();
        HandleWeaponSwitch();
    }

    private void HandleMovement()
    {
        //Calculate movement direction and speed. Also set movement animation.
        float speed = (Input.GetKey(KeyCode.LeftShift)) ? _runningSpeed : _walkingSpeed;

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if(direction != Vector3.zero) _animator.SetFloat("Speed_f", (speed == _walkingSpeed) ? 0.49f : 1.0f);
        else _animator.SetFloat("Speed_f", 0.0f);

        Vector3 motion = direction;
        motion *= (Mathf.Abs(direction.x) == 1 && Mathf.Abs(direction.z) == 1) ? 0.707f : 1.0f;
        motion *= speed;

        _characterController.Move(motion * Time.deltaTime);

        //Handle facing direction
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            _aimDirection = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(_aimDirection);
            _aimDirection = (_aimDirection - this.transform.position).normalized;
        }
    }

    private void HandleWeapon()
    {
        if (Input.GetKey(KeyCode.Space)) StartCoroutine(_weaponCtrl.HandleFire(this.transform.forward));
    }

    private void HandleWeaponSwitch()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            _weaponCtrl.SetActive(false);

            _currentWeapon = (_currentWeapon + 1) % _weapons.Count;
            _weaponCtrl = _weapons[_currentWeapon].GetComponent<WeaponController>();
            _weaponCtrl.SetActive(true);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if(_health <= 0)
        {
            this.enabled = false;
            _animator.SetBool("Death_b", true);
        }
    }

    public float GetHealth()
    {
        return _health;
    }
}