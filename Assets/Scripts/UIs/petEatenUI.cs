using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class petEatenUI : MonoBehaviour
{
    public TMP_Text petEaten;
    public GameObject pet;

    // Start is called before the first frame update
    void Start()
    {
        petEaten = GetComponent<TMP_Text>();
        pet = GameObject.Find("Pet");
    }

    // Update is called once per frame
    void Update()
    {
        petEaten.text = "Seeds Eaten: " + pet.GetComponent<petMovement>().seedEaten;
    }
}
