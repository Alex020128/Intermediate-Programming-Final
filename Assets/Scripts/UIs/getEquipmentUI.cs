using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class getEquipmentUI : MonoBehaviour
{
    public TMP_Text playerEquipment;
    private Animator animator;
    private GameObject player;

    public bool faded;

    // Start is called before the first frame update
    void Start()
    {
        playerEquipment = GetComponent<TMP_Text>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");

        faded = false;
    }

    void resetAnimationBool()
    {
        playerEquipment.text = "";
        faded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            if(player.GetComponent<playerMovement>().equipment == "frostCircle" && faded == false)
            {
                playerEquipment.text = "The special seed inspired your ability to cast Frost Field!";
                animator.SetTrigger("fadeOut");
            }
            else if(player.GetComponent<playerMovement>().equipment == "Trap" && faded == false)
            {
                playerEquipment.text = "The special seed inspired your ability to summon Vine Traps!";
                animator.SetTrigger("fadeOut");
            }
            else
            {
                playerEquipment.text = "";
            }
        }
        else
        {
            playerEquipment.enabled = false;
        }
    }
}
