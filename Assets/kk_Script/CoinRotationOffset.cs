using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotationOffset : MonoBehaviour
{
	private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay(){
    	yield return new WaitForSeconds(Random.Range(0, 1f));
    	_animator.enabled = true;
    }
}
