using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerHealthUI : MonoBehaviour
{
    //Components
    public TMP_Text playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        playerHealth = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Display the player health
        if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            playerHealth.text = "Player Health: " + gameManager.Instance.playerHealth;
        }
        else
        {
            //Hide this when the game ends
            playerHealth.enabled = false;
        }
    }
}
