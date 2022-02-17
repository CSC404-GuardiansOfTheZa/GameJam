using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
	
{
    // Start is called before the first frame update
    private Image _healthBar;
    private float _maxHealth = 100;
    public float _healthValue;
    private float lerpSpeed;

    void Start()
    {
        _healthValue = _maxHealth;
        _healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    	lerpSpeed = 3f * Time.deltaTime;
        HealthChange();
        ColorChanger();
    }

    void HealthChange(){
    	_healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, _healthValue / _maxHealth, lerpSpeed);
    }

    void ColorChanger(){
    	Color healthColor = Color.Lerp(Color.red, Color.green, (_healthValue / _maxHealth));
    	_healthBar.color = healthColor;
    }

    void Damage(float damagePoint){
    	if(_healthValue - damagePoint > 0){
    		_healthValue -= damagePoint;
    	}
    	else{
    		// game over
    		_healthValue = 0;
    	}
    }

    void Heal(float healingPoints){
    	if(_healthValue + healingPoints <= _maxHealth){
    		_healthValue += healingPoints;
    	}
    	else{
    		_healthValue = _maxHealth;
    	}
    }
}

