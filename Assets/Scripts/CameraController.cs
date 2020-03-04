using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = _player.transform.position;
        Vector3 cameraPos = this.transform.position;
        cameraPos.x = playerPos.x;
        cameraPos.z = playerPos.z;

        this.transform.position = cameraPos;

        this.transform.LookAt(_player.transform);
    }
}
