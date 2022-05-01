using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class gameManager : Singleton<gameManager>
{
    //GameObjects
    [SerializeField]
    private GameObject seedSpawner;
    [SerializeField]
    private GameObject enemySpawner;

    //Player and pet stats
    public int playerHealth;
    public int petHealth;
    public int petDemand;
    public int seedMax;
    public int spawnWave;

    //Player and pet bools
    public bool playerDeath;
    public bool playerInvinsible;
    public bool petDeath;
    public bool petInvinsible;
    public bool levelCleared;

    //Player and pet invincible timer
    public float playerInvinsibleTime;
    public float petInvinsibleTime;
    public float seedCarried;

    //Coroutines
    Coroutine endingCoroutine;

    void Awake()
    {
        name = "GameManager"; // Set name of object
        
        //Assign variables
        seedSpawner = GameObject.Find("seedSpawner");
        enemySpawner = GameObject.Find("enemySpawner");
        spawnWave = 1;
    }

    private void Start()
    {
        //Reset all the stats and bools
        resetStats();
    }

    public void resetStats()
    {
        //Reset all the stats and bools
        playerHealth = 100;
        petHealth = 50;
        playerInvinsibleTime = 0;
        petInvinsibleTime = 0;
        petDemand = 5;
        seedMax = 8;
        spawnWave = 1;
        seedCarried = 0;

        playerDeath = false;
        playerInvinsible = false;
        petDeath = false;
        petInvinsible = false;
    }

    private IEnumerator endingScreen(float wait)
    {
        //Ends the game in 3s
        yield return new WaitForSeconds(wait);
        if ("gameScreen" == SceneManager.GetActiveScene().name)
        {
            SceneManager.LoadScene("endingScreen");
            resetStats();
        }
    }

    void Update()
    {
        //Assign variables
        if ("gameScreen" == SceneManager.GetActiveScene().name)
        {
            seedSpawner = GameObject.Find("seedSpawner");
            enemySpawner = GameObject.Find("enemySpawner");
        }

        //Kill LL ENEMIES, Spawn more enemies (in waves), spawn some seeds, increase the maximum seeds amount, heal the player and increases the pet demand
        if (levelCleared)
        {
            GameObject.Find("Pet").GetComponent<Animator>().SetTrigger("Attack");
            Camera.main.transform.DOShakePosition(2f, new Vector3(0.75f, 0.75f, 0));
            petDemand += 5;
            seedMax += 5;
            spawnWave += 1;
            seedSpawner.GetComponent<seedSpawner>().spawnSome();
            enemySpawner.GetComponent<enemySpawner>().killAllEnemies();
            StartCoroutine(GameObject.Find("countDownUI").GetComponent<countDownUI>().countDownCoroutineLoop());
            playerHealth += 3;
            levelCleared = false;
        }
        
        //The player has 1s of invincible time if gets hurt
        if (playerInvinsible == true)
        {
            playerInvinsibleTime += Time.deltaTime;
            if (playerInvinsibleTime >= 1)
            {
                playerInvinsible = false;
            }
        }
        //The pet has 1s of invincible time if gets hurt
        if (petInvinsible == true)
        {
            petInvinsibleTime += Time.deltaTime;
            if (petInvinsibleTime >= 1)
            {
                petInvinsible = false;
            }
        }

        //Player death
        if (playerHealth <= 0)
        {
            playerDeath = true;
        }
        
        //Pet death
        if (petHealth <= 0)
        {
            petDeath = true;
        }

        //Player's health can't be over 100
        if (playerHealth > 100)
        {
            playerHealth = 100;
        }

        //PET's health can't be over 50
        if (petHealth > 50)
        {
            petHealth = 50;
        }

        //Either player's death or pet's death will end the game
        if("gameScreen" == SceneManager.GetActiveScene().name)
        {
            if ((playerDeath || petDeath))
            {
                endingCoroutine = StartCoroutine(endingScreen(3f));
                GameObject.Find("Player").GetComponent<Animator>().SetTrigger("Death");
                GameObject.Find("Pet").GetComponent<Animator>().SetTrigger("Death");
            }
        }

        //Reset all the stats and bools
        if ("titleScreen" == SceneManager.GetActiveScene().name)
        {
            resetStats();
        }
    }
}
