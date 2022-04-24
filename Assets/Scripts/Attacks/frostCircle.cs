using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frostCircle : MonoBehaviour
{
    public float frostTimer;

    [SerializeField]
    private float coolDownPerSecond = 30f;
    [SerializeField]
    private float coolDownTimer;

    public SpriteRenderer sr;

    public bool casting;

    public void castCircle()
    {
        casting = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot bullet if there's still bullet not shot
        while (coolDownTimer < 0.0f) // && gameManager.Instance.death == false
        {
            coolDownTimer += coolDownPerSecond;

            GameObject.Find("Player").GetComponent<playerMovement>().castCircle = false;
        }

        if (casting)
        {
            sr.enabled = true;
            frostTimer += Time.deltaTime;
            coolDownTimer = 30;
        }
        else
        {
            frostTimer = 0;
            sr.enabled = false;
            coolDownTimer -= Time.deltaTime;
        }

        if (frostTimer > 10)
        {
            casting = false;
        }

        this.gameObject.transform.position = GameObject.Find("Player").transform.position;
    }
}
