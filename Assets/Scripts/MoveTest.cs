using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    // Start is called before the first frame update
    private float _userHorizontalInput;
    private float _userVerticalInput;
    private const float ScaleMovment = 0.25f;
    private Transform trans;
    private Vector3 _rot;

    void Start()
    {
        trans = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _userHorizontalInput = Input.GetAxis("Vertical");
        _userVerticalInput = Input.GetAxis("Horizontal");
        _rot = trans.rotation.eulerAngles;
        _rot += new Vector3(0, _userVerticalInput, 0);
        trans.rotation = Quaternion.Euler(_rot);
        trans.position += transform.forward * _userHorizontalInput * ScaleMovment;

    }
}
