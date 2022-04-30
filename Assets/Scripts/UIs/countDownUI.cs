using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class countDownUI : MonoBehaviour
{
    public TMP_Text countDown;
    public bool countDownStart;

    // Start is called before the first frame update
    void Start()
    {
        countDown = GetComponent<TMP_Text>();
        countDownStart = false;
    }
    private IEnumerator countDown2(float wait)
    {
        //Destroy all the active enemies
        yield return new WaitForSeconds(wait);
        countDown.text = "2";
    }
    private IEnumerator countDown1(float wait)
    {
        //Destroy all the active enemies
        yield return new WaitForSeconds(wait);
        countDown.text = "1";
    }
    private IEnumerator countDownText(float wait)
    {
        yield return new WaitForSeconds(wait);
        countDown.text = "More Monsters Appeared!";
        yield return new WaitForSeconds(wait);
        GameObject.Find("enemySpawner").GetComponent<enemySpawner>().spawnWave();
        countDownStart = false;
    }

    public IEnumerator countDownCoroutineLoop()
    {
        countDownStart = true;
        countDown.text = "3";
        yield return StartCoroutine(countDown2(1f));
        yield return StartCoroutine(countDown1(1f));
        yield return StartCoroutine(countDownText(1f));
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.Instance.playerDeath == false && gameManager.Instance.petDeath == false)
        {
            if(countDownStart == false)
            {
                countDown.text = "";
            }
        }
        else
        {
            countDown.enabled = false;
        }

    }
}
