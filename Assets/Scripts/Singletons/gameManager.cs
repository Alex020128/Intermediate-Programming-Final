using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class gameManager : Singleton<gameManager>
{
    //Player bullet
    [SerializeField]
    private GameObject prefabToSpawn = null;

    private GameObject seedSpawner;

    //Player stats
    public int playerHealth;
    public int petHealth;

    public int petDemand;
    public int seedMax;
    public int spawnWave;
    public TMP_Text health; //UI text

    //Player bools
    public bool playerDeath = false;
    public bool playerInvinsible = false;
    public bool petDeath = false;
    public bool petInvinsible = false;

    public bool levelCleared = false;

    //Player invincible timer
    public float playerInvinsibleTime;
    public float petInvinsibleTime;
    public float seedCarried;

    //Player stats that can grow as more buffs are collected
    /*public float missileCoolDownTime;
    
    public float bulletDamage;
    public float missileDamage;

    public float bulletDamageEXP;
    public float bulletAmountEXP;

    public float bulletDamageEXPBar;
    public float bulletAmountEXPBar;

    //Background music loop
    public AudioSource audioSource;
    public AudioClip musicLoop;*/

    void Awake()
    {
        name = "GameManager"; // Set name of object

        seedSpawner = GameObject.Find("seedSpawner");
        
        health = GetComponent<TMP_Text>();
        //audioSource = GetComponent<AudioSource>();

        playerHealth = 100;
        petHealth = 50;
        playerInvinsibleTime = 0;
        petInvinsibleTime = 0;

        petDemand = 5;
        seedMax = 8;
        spawnWave = 1;

        seedCarried = 0;
        //missileCoolDownTime = 10;

        //bulletDamageEXP = 0;
        //bulletAmountEXP = 0;

        //bulletDamageEXPBar = 1;
        //bulletAmountEXPBar = 1;


    }

    private void Start()
    {
        //Loops the music
        //audioSource.clip = musicLoop;
        //audioSource.Play();
    }

    void Update()
    {
        if (levelCleared)
        {
            petDemand += 5;
            seedMax += 5;
            spawnWave += 1;
            seedSpawner.GetComponent<seedSpawner>().spawnSome();

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

        //Display health
        if (playerDeath == false)
        {
            health.text = "Health: " + playerHealth;
        } else
        {
            health.enabled = false;
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

        //Increases bullet damage by 1 if the exp bar is filled, increases the exp required for next levelup by 1 as well
        /*if (bulletDamageEXP == bulletDamageEXPBar)
        {
            bulletDamage += 1;
            bulletDamageEXP = 0;
            bulletDamageEXPBar += 1;
        }
        
        //Increases bullet amount by 1 if the exp bar is filled, increases the exp required for next levelup by 1 as well
        if (bulletAmountEXP == bulletAmountEXPBar)
        {
            GameObject newBullet = Instantiate(prefabToSpawn, GameObject.Find("lmbBullet").transform.position, Quaternion.identity, GameObject.Find("lmbBullet").transform);
            GameObject.Find("lmbBullet").GetComponent<bulletSpawner>().Bullets.Add(newBullet);
            newBullet.SetActive(false);
            bulletAmountEXP = 0;
            bulletAmountEXPBar += 1;
            missileCoolDownTime -= 1;
        }*/
    }
}
