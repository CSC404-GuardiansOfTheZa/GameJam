using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	public int playerScore = 0;
	public ScoreUpdate ScoreUpdate;
    // Start is called before the first frame update
    void Start()
    {
        ScoreUpdate = FindObjectOfType<ScoreUpdate>();
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void UpdateScore(){
    	playerScore += 1;
    	ScoreUpdate.UpdateScoreText(playerScore);
    }
}
