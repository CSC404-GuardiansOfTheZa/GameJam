using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    private static ScoreManager _instance;
    public static ScoreManager Instance => _instance;

    public int Score { get; private set; }
    public int Deaths { get; private set; }
    private int savedScore = 0;

    public delegate void IntDelegate(int num);
    public event IntDelegate onScoreUpdate;
    public event IntDelegate onDeathsUpdate;

    [SerializeField] private bool enableScoring;

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        this.Score = 0;
        this.Deaths = 0;
    }

    void Start() {
        PizzaMan.Instance.OnKilled += this.AddDeath;
    }

    public void AddToScore(int amt) {
        Score += amt;
        Score = Score < 0 ? 0 : Score;
        Debug.Log(Score);
        onScoreUpdate?.Invoke(Score);
    }

    public void AddDeath() {
        this.AddDeaths(1);
    }

    public void AddDeaths(int n) {
        Deaths += n;
        Deaths = Deaths < 0 ? 0 : Deaths;
        Debug.Log(Deaths);
        onDeathsUpdate?.Invoke(Deaths);
    }

    public void SaveScore() {
        savedScore = Score;
    }

    public void LoadScore() {
        Score = savedScore;
        onScoreUpdate?.Invoke(Score);
    }
}
