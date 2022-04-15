using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class petHealthUI : MonoBehaviour
{
    public TMP_Text petHealth;

    // Start is called before the first frame update
    void Start()
    {
        petHealth = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        petHealth.text = "Health: " + gameManager.Instance.petHealth;
    }
}
