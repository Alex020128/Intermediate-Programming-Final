using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerHealthUI : MonoBehaviour
{
    public TMP_Text playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            playerHealth.text = "Player Health: " + gameManager.Instance.playerHealth;
        }
        else
        {
            playerHealth.enabled = false;
        }
    }
}
