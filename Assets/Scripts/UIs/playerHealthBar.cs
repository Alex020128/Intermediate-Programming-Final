using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class playerHealthBar : MonoBehaviour
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Lerps the player health bar
        slider.DOValue(gameManager.Instance.playerHealth / 100f, 0.5f);

        if (gameManager.Instance.playerDeath == true || gameManager.Instance.petDeath == true)
        {
            //Hide this when player is dead
            this.gameObject.SetActive(false);
        }
    }
}
