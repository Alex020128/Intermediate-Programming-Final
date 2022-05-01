using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class finalScoreUI : MonoBehaviour
{
    //UI text
    public TMP_Text finalScore;

    //Total score
    public float Score;

    private void Awake()
    {
        //Assign variables
        finalScore = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
       //Calculate the final score
        Score = Mathf.Floor(scoreManager.Instance.meleeEnemyKills * 100 +
                                scoreManager.Instance.rangeEnemyKills * 150 +
                                scoreManager.Instance.playerTime * 10 -
                                scoreManager.Instance.meleeHit * 10 -
                                scoreManager.Instance.rangeHit * 5);


        //Display the score
        finalScore.text = "Score: " + Score;

        //Score can not be below 0
        if (Score < 0)
        {
            Score = 0;
        }
    }
}
