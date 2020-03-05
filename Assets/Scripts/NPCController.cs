using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private int _idleAnim = 0;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _animator.SetInteger("Animation_int", _idleAnim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
