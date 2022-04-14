using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedSpawner : MonoBehaviour
{
    //Bullet stats
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private float spawnPerSecond = 10f;
    [SerializeField]
    private float spawnTimer;
    [SerializeField]
    private List<GameObject> seeds = new List<GameObject>();
    public List<GameObject> Seeds
    {
        get
        {
            return seeds;
        }
    }

    //SFX for bullet
    public AudioSource audioSource;
    public AudioClip bulletSound;

    private void Start()
    {
        //Spawn a pool of bullets at top of the screen
        for (int i = 0; i < 3; i++)
        {
            GameObject newBullet = Instantiate(prefabToSpawn, new Vector2(Random.Range(-10, 10), Random.Range(-5, 5)), Quaternion.identity);
            seeds.Add(newBullet);
            //seeds[i].SetActive(false);
           
            bool canSpawn = false;

            //Debug.Log(grounedRay.collider.gameObject);

            while (!canSpawn)
            {
                seeds[i].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
                RaycastHit2D grounedRay = Physics2D.Raycast(seeds[i].transform.position, Vector2.down, 0.5f, 1 << 6);

                if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<jumpObject>() != null)
                {
                    Debug.Log(grounedRay.collider.gameObject);
                    canSpawn = true;
                }
                else
                {
                    //seeds[i].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
                    canSpawn = false;
                }
            }
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

    public void spawnSeed()
    {
        //let one of the waiting bullets to be active
        for (int i = 0; i < seeds.Count; i++)
        {
            if (!seeds[i].activeInHierarchy)
            {
                //bulletSFX();
                seeds[i].SetActive(true);
                seeds[i].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
                bool canSpawn = false;
 
                //Debug.Log(grounedRay.collider.gameObject);

                while (!canSpawn)
                {
                    seeds[i].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
                    RaycastHit2D grounedRay = Physics2D.Raycast(seeds[i].transform.position, Vector2.down, 0.5f, 1 << 6);

                    if (grounedRay.collider != null && grounedRay.collider.gameObject.GetComponent<jumpObject>() != null)
                    {
                        Debug.Log(grounedRay.collider.gameObject);
                        canSpawn = true;
                    }
                    else
                    {
                        //seeds[i].transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-5, 5));
                        canSpawn = false;
                    }
                }

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
            spawnSeed();
        }

        //Let bullet "spawn" around the player's chest
        //transform.position = new Vector2(GameObject.Find("Player").transform.position.x, GameObject.Find("Player").transform.position.y);
    }
}
