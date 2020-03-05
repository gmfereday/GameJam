using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]
    private Slider _slider;

    private PlayerController _playerScript;

    // Start is called before the first frame update
    void Start()
    {
        _playerScript = PlayerManager.instance.player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _playerScript.GetHealth() / 100.0f;
    }
}
