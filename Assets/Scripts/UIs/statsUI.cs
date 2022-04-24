using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class statsUI : MonoBehaviour
{
    //UI text
    public TMP_Text Stats;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Display total time lasted, total score, and instruction for restart
        Stats.text = "You Lasted: " + Mathf.Round(scoreManager.Instance.playerTime) +
                     "s\n\nTotal Kills: " + (scoreManager.Instance.meleeEnemyKills + scoreManager.Instance.meleeEnemyKills) +
                     "\n\nTotal Damage Received: " + (scoreManager.Instance.meleeHit * 2 + scoreManager.Instance.rangeHit * 1) +
                     "\n\n\n\nTotal Seeds Eaten: " + scoreManager.Instance.seedEaten;
    }
}
