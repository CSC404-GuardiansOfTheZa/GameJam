using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    // Start is called before the first frame update
    private float _userHorizontalInput;
    private const float ScaleMovment = 0.25f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _userHorizontalInput = Input.GetAxis("Vertical");
        gameObject.GetComponent<Transform>().position += transform.forward * _userHorizontalInput * ScaleMovment;

    }
}
