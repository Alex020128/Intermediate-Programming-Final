using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceCannonSpawner : MonoBehaviour
{
    //Bullet stats
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private float spawnPerSecond = 1f;
    [SerializeField]
    private float spawnTimer;
    [SerializeField]
    private List<GameObject> bullets = new List<GameObject>();
    public List<GameObject> Bullets
    {
        get
        {
            return bullets;
        }
    }

    //SFX for bullet
    public AudioSource audioSource;
    public AudioClip bulletSound;

    private void Start()
    {
        //Spawn a pool of bullets at top of the screen
        for (int i = 0; i < 1; i++)
        {
            GameObject newBullet = Instantiate(prefabToSpawn, transform.position, Quaternion.identity, this.gameObject.transform);
            bullets.Add(newBullet);
            bullets[i].SetActive(false);
        }

        //audioSource = GetComponent<AudioSource>();
    }

    //public void bulletSFX()
    //{
    //Play the bullet SFX
    //audioSource.Stop();
    //audioSource.clip = bulletSound;
    //audioSource.Play();
    //}

    public void shootBullet()
    {
        //let one of the waiting bullets to be active
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                //bulletSFX();
                bullets[i].SetActive(true);
                bullets[i].GetComponent<iceCannon>().lifeTimer = 3.0f;
                bullets[i].transform.position = transform.position;
                bullets[i].transform.parent = GameObject.Find("rmbBullet").transform;
                break;
            }
        }
    }

    void Update()
    {
        //Shoot bullet if there's still bullet not shot
        spawnTimer -= Time.deltaTime;
        while (spawnTimer < 0.0f) // && gameManager.Instance.death == false
        {
            spawnTimer += spawnPerSecond;

            GameObject.Find("Player").GetComponent<playerMovement>().shootCannon = false;
        }

        //Let bullet "spawn" around the player's chest
        transform.position = new Vector2(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y);
    }
}
