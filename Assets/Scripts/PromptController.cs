using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptController : MonoBehaviour
{
    [SerializeField]
    private float _promptTime = 10.0f;

    [SerializeField]
    private Text _promptText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PromptTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PromptTimer()
    {
        yield return new WaitForSeconds(_promptTime);
        _promptText.enabled = false;
    }
}
