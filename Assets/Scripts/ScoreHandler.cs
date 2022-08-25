using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int scoreMultiplier = 1;

    public const string HighScoreKey = "HighScore";

    private float scoreValue;
    private int scoreValueInt;


    // Update is called once per frame
    void Update()
    {
        scoreValue += Time.deltaTime;
        scoreValueInt = Mathf.FloorToInt(scoreValue) * scoreMultiplier; //this rounds the float to an int.
        scoreText.text = scoreValueInt.ToString();
    }

    private void OnDestroy()
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if(scoreValueInt > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, scoreValueInt);
        }
    }
}
