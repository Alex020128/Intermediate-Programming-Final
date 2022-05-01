using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class timeUI : MonoBehaviour
{
    //UI text
    public TMP_Text time;

    void Awake()
    {
        //Assign variables
        time = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Display play time
        if(gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            time.text = "Time: " + Mathf.Round(timeManager.Instance.playerTime)+ "s";
        } else
        {
            //Hide this when the game ends
            time.enabled = false;
        }
    }
}
