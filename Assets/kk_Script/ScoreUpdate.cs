using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUpdate : MonoBehaviour
{
	// private Text playerScoreText;
	public GameObject playerScoreObj;
    // Start is called before the first frame update
    void Start()
    {
        playerScoreObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: 0"; 
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void UpdateScoreText(int score){
    	playerScoreObj.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + score;

    }
}
