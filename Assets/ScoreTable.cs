using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreTable : MonoBehaviour
{
	private Transform entryContainer;
	private Transform entryTemplate;
	private List<Transform> highscoreEntryTransformList;
    // Start is called before the first frame update
    private void Awake(){
    	entryContainer = transform.Find("ScoreContainer");
    	entryTemplate = entryContainer.Find("ScoreTemplate");
    	string Scene = PlayerPrefs.GetString("prevScene");

    	entryTemplate.gameObject.SetActive(false);
    	if(Scene == "Finish"){
    		AddHighscoreEntry(ScoreManager.Instance.Score,ScoreManager.Instance.Deaths);
    	}

    	string jsonString = PlayerPrefs.GetString("highscoreTable");
    	Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
    	List<HighscoreEntry> highscoreEntryList = highscores.highscoreEntryList;

    	// use for reset table (don't remove it!)
    	// for (int i = 0; i < highscores.highscoreEntryList.Count; i++){
    	// 	highscores.highscoreEntryList[i].score = 0;
    	// 	highscores.highscoreEntryList[i].death = 1000;
    	// }
    	// string resetjson = JsonUtility.ToJson(highscores);
    	// PlayerPrefs.SetString("highscoreTable", resetjson);
    	// PlayerPrefs.Save();

    	for (int i = 0; i < highscores.highscoreEntryList.Count; i++){
    		for( int j = i + 1; j < highscores.highscoreEntryList.Count; j++){
    			if(highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score){
    				HighscoreEntry tmp = highscores.highscoreEntryList[i];
    				highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
    				highscores.highscoreEntryList[j] = tmp;
    			}
    			else if((highscores.highscoreEntryList[j].score == highscores.highscoreEntryList[i].score) && (highscores.highscoreEntryList[j].death < highscores.highscoreEntryList[i].death)){
    				HighscoreEntry tmp = highscores.highscoreEntryList[i];
    				highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
    				highscores.highscoreEntryList[j] = tmp;
    			}
    		}
    	}

    	highscoreEntryTransformList = new List<Transform>();
    	foreach (HighscoreEntry highscoreEntry in highscoreEntryList){
    		CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
    	}
    }

    private void AddHighscoreEntry(int score, int death){
    	// create new entry
    	HighscoreEntry highscoreEntry = new HighscoreEntry{ score = score, death = death};
    	string jsonString = PlayerPrefs.GetString("highscoreTable");
    	Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
    	//sort the score and death
    	for (int i = 0; i < highscores.highscoreEntryList.Count; i++){
    		for( int j = i + 1; j < highscores.highscoreEntryList.Count; j++){
    			if(highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score){
    				HighscoreEntry tmp = highscores.highscoreEntryList[i];
    				highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
    				highscores.highscoreEntryList[j] = tmp;
    			}
    			else if((highscores.highscoreEntryList[j].score == highscores.highscoreEntryList[i].score) && (highscores.highscoreEntryList[j].death < highscores.highscoreEntryList[i].death)){
    				HighscoreEntry tmp = highscores.highscoreEntryList[i];
    				highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
    				highscores.highscoreEntryList[j] = tmp;
    			}
    		}
    	}
    	bool hasSame = false;
    	// for(int i = 0; i < highscores.highscoreEntryList.Count; i++){
    	// 	HighscoreEntry checkEntry = highscores.highscoreEntryList[i];
    	// 	if(checkEntry.score == score && checkEntry.death == death){
    	// 		highscores.highscoreEntryList[i].numPeople += 1;
    	// 		hasSame = true;
    	// 	}
    	// }
    	if(!hasSame){
    		if(highscores.highscoreEntryList.Count < 5){
    			// add new one 
    			highscores.highscoreEntryList.Add(highscoreEntry);
    		}
	    	else{
	    		// replace
	    		HighscoreEntry currentLast = highscores.highscoreEntryList[highscores.highscoreEntryList.Count - 1];
	    		if(currentLast.score < score || (currentLast.score == score && currentLast.death > death)){
	    			highscores.highscoreEntryList[highscores.highscoreEntryList.Count - 1] = highscoreEntry;
	    		}
	    	}
    	}
    	string json = JsonUtility.ToJson(highscores);
    	PlayerPrefs.SetString("highscoreTable", json);
    	PlayerPrefs.Save();
    }


    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList){
    	float templateHeight = 150f;
    	Transform entryTransform = Instantiate(entryTemplate, container);
		RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
		entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
		int score = highscoreEntry.score;
		int death = highscoreEntry.death;
		entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
		entryTransform.Find("deathText").GetComponent<TextMeshProUGUI>().text = death.ToString();
		transformList.Add(entryTransform);
		entryTransform.gameObject.SetActive(true);
    }

    private class Highscores{
    	public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry{
    	public int score;
    	public int death;
    }
}
