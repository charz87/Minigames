using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    private int score;

    public Text scoreText;

    private void Awake()
    {
        score = 0;
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }


    public void AddScore()
    {
        score += 100;
    }
 

}
