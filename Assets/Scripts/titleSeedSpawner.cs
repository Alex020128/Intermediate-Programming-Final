using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleSeedSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private float spawnPerSecond = 5f;
    [SerializeField]
    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot bullet if there's still bullet not shot
        spawnTimer -= Time.deltaTime;
        while (spawnTimer < 0.0f) // && gameManager.Instance.death == false
        {
            spawnTimer += spawnPerSecond;
            Instantiate(prefabToSpawn, new Vector2(Random.Range(-7, 7), 5), Quaternion.Euler(0, 0, Random.Range(-180, 180)));
        }
    }
}
