using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityGateController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private float _openRate = 1.0f;
    [SerializeField]
    private float _openDistance = 3.0f;

    private Vector3 _origin;
    private Vector3 _closeTarget;
    [SerializeField]
    private Vector3 _openTarget;

    private bool _shouldOpen = false;
    private bool _isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        _origin = this.transform.position;
        _closeTarget = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckShouldOpen();
        HandleOpenClose();
    }

    private void CheckShouldOpen()
    {
        float dist = Vector3.Distance(_origin, _player.transform.position);

        if (dist < _openDistance) _shouldOpen = true;

        else _shouldOpen = false;
    }

    private void HandleOpenClose()
    {
        if (_shouldOpen && !_isOpen)
        {
            _isOpen = true;
            StartCoroutine(OpenAnim());
        }
        else if (!_shouldOpen && _isOpen)
        {
            _isOpen = false;
            StartCoroutine(CloseAnim());
        }
    }

    private IEnumerator OpenAnim()
    {
        while(this.transform.localPosition != _openTarget)
        {
            Vector3 newPos = Vector3.MoveTowards(this.transform.localPosition, _openTarget, _openRate * Time.deltaTime);

            this.transform.localPosition = newPos;

            yield return null;
        }
    }

    private IEnumerator CloseAnim()
    {
        while (this.transform.localPosition != _closeTarget)
        {
            Vector3 newPos = Vector3.MoveTowards(this.transform.localPosition, _closeTarget, _openRate * Time.deltaTime);

            this.transform.localPosition = newPos;

            yield return null;
        }
    }
}
