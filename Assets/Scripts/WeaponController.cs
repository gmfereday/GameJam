using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private float _damage = 50.0f;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _range = 50.0f;
    [SerializeField]
    private float _impactForce = 100.0f;
    [SerializeField]
    private Transform _shotLocation;

    [SerializeField]
    private float _idleBodyHorizontal = 0.6f;
    [SerializeField]
    private float _idleBodyVertical = 0.0f;
    [SerializeField]
    private float _idleHeadHorizontal = -0.8f;
    [SerializeField]
    private float _idleHeadVertical = 0.0f;

    [SerializeField]
    private float _walkBodyHorizontal = 0.6f;
    [SerializeField]
    private float _walkBodyVertical = 0.0f;
    [SerializeField]
    private float _walkHeadHorizontal = -0.8f;
    [SerializeField]
    private float _walkHeadVertical = 0.0f;

    [SerializeField]
    private float _runBodyHorizontal = 0.6f;
    [SerializeField]
    private float _runBodyVertical = 0.3f;
    [SerializeField]
    private float _runHeadHorizontal = -0.8f;
    [SerializeField]
    private float _runHeadVertical = 0.0f;

    private float _nextShotTime;
    [SerializeField]
    private float _shotAnimDelayTime = 0.09f;

    [SerializeField]
    private GameObject _muzzleFlash;
    private Renderer _flashRenderer;
    [SerializeField]
    private Light _flashLight;
    [SerializeField]
    private Light _muzzleFlashLight;

    [SerializeField]
    private GameObject _bloodSplatter;
    [SerializeField]
    private GameObject _impactSpark;

    [SerializeField]
    private AudioSource _gunShot;

    [SerializeField]
    private int _weaponAnim = 1;
    [SerializeField]
    private bool _fullAuto = false;
    private Animator _animator;

    [SerializeField]
    private LineRenderer _lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _flashRenderer = _muzzleFlash.GetComponent<Renderer>();
        _flashRenderer.enabled = false;
        _nextShotTime = Time.time;
        _muzzleFlashLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Fire(Vector3 direction)
    {
        //direction = this.transform.forward;
        if (Time.time < _nextShotTime) yield break;

        _nextShotTime = Time.time + _fireRate;

        _animator.SetBool("Shoot_b", true);

        yield return new WaitForSeconds(_shotAnimDelayTime);

        _flashRenderer.enabled = true;
        _muzzleFlashLight.enabled = true;
        _gunShot.PlayOneShot(_gunShot.clip, 0.5f);

        RaycastHit hit;

        Vector3 angles = _shotLocation.eulerAngles;
        angles.x = 0;
        angles.z = 0;

        _shotLocation.eulerAngles = angles;

        if (Physics.Raycast(_shotLocation.position, _shotLocation.forward, out hit, _range))
        {
            GameObject effect;

            if (hit.transform.CompareTag("zombie"))
            {
                hit.transform.gameObject.GetComponentInParent<ZombieController>().TakeDamage(_damage);

                hit.rigidbody.AddForce(-hit.normal * _impactForce, ForceMode.Impulse);

                effect = Instantiate(_bloodSplatter, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else effect = Instantiate(_impactSpark, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(effect, 1.0f);
        }

        //Draw bullet trace so player can easily see shot direction
        float dist = _range;

        if (hit.point != null)
        {
            dist = Vector3.Distance(_shotLocation.position, hit.point);
        }

        dist = Mathf.Min(_range, dist);

        //Max trace length is 1/2 the distance, minimum trace length is 1/5 the distance
        float pos1OffsetDistance = Random.Range(0, dist - dist / 5.0f);
        Vector3 pos1 = _shotLocation.position + direction * pos1OffsetDistance;

        float remainingDistance = dist - pos1OffsetDistance;
        float pos2OffsetDistance = Random.Range(dist / 5.0f, Mathf.Min(dist / 2.0f, remainingDistance));
        Vector3 pos2 = pos1 + direction * pos2OffsetDistance;

        _lineRenderer.SetPosition(0, pos1);
        _lineRenderer.SetPosition(1, pos2);

        _lineRenderer.enabled = true;
        yield return new WaitForEndOfFrame();
        _lineRenderer.enabled = false;
        
        _flashRenderer.enabled = false;
        _muzzleFlashLight.enabled = false;
    }

    public void CeaseFire()
    {
        _animator.SetBool("Shoot_b", false);
    }

    public void SetAnimator(Animator animator)
    {
        _animator = animator;
    }

    public void SetIdleValues()
    {
        _animator.SetFloat("Body_Vertical_f", _idleBodyVertical);
        _animator.SetFloat("Body_Horizontal_f", _idleBodyHorizontal);
        _animator.SetFloat("Head_Horizontal_f", _idleHeadHorizontal);
        _animator.SetFloat("Head_Vertical_f", _idleHeadVertical);
    }

    public void SetWalkValues()
    {
        _animator.SetFloat("Body_Vertical_f", _walkBodyVertical);
        _animator.SetFloat("Body_Horizontal_f", _walkBodyHorizontal);
        _animator.SetFloat("Head_Horizontal_f", _walkHeadHorizontal);
        _animator.SetFloat("Head_Vertical_f", _walkHeadVertical);
    }

    public void SetRunValues()
    {
        _animator.SetFloat("Body_Vertical_f", _runBodyVertical);
        _animator.SetFloat("Body_Horizontal_f", _runBodyHorizontal);
        _animator.SetFloat("Head_Horizontal_f", _runHeadHorizontal);
        _animator.SetFloat("Head_Vertical_f", _runHeadVertical);
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            this.enabled = true;
            
            foreach(Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = true;
            }

            _flashLight.enabled = true;

            if(_flashRenderer != null) _flashRenderer.enabled = false;
            _lineRenderer.enabled = false;

            _animator.SetInteger("WeaponType_int", _weaponAnim);
            _animator.SetBool("FullAuto_b", _fullAuto);
        }
        else
        {
            this.enabled = false;
            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
            _flashLight.enabled = false;
            _muzzleFlashLight.enabled = false;
        }
    }    
}