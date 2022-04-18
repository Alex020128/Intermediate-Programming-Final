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

    // Start is called before the first frame update
    void Start()
    {
        Score = Mathf.Floor(scoreManager.Instance.meleeEnemyKills * 100 +
                                scoreManager.Instance.rangeEnemyKills * 150 +
                                scoreManager.Instance.playerTime * 10 -
                                scoreManager.Instance.Hit * 100);
    }

    // Update is called once per frame
    void Update()
    {
        finalScore.text = "Score: " + Score;
    }
}
