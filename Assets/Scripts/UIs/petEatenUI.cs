using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class petEatenUI : MonoBehaviour
{
    //Components
    public TMP_Text petEaten;
    public GameObject pet;

    // Start is called before the first frame update
    void Start()
    {
        //Assign variables
        petEaten = GetComponent<TMP_Text>();
        pet = GameObject.Find("Pet");
    }

    // Update is called once per frame
    void Update()
    {
        //Display the seeds pet eaten
        if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            petEaten.text = "Seeds Eaten: " + pet.GetComponent<petMovement>().seedEaten;
        }
        else
        {
            //Hide when the game ends
            petEaten.enabled = false;
        }
    }
}
