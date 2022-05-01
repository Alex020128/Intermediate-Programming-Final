using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleSeedSpawner : MonoBehaviour
{
    //Seed stats
    [SerializeField]
    private GameObject prefabToSpawn = null;
    [SerializeField]
    private float spawnPerSecond = 5f;
    [SerializeField]
    private float spawnTimer;

    // Update is called once per frame
    void Update()
    {
        //Spawn a seed every 5s
        spawnTimer -= Time.deltaTime;
        while (spawnTimer < 0.0f)
        {
            spawnTimer += spawnPerSecond;
            Instantiate(prefabToSpawn, new Vector2(Random.Range(-7, 7), 5), Quaternion.Euler(0, 0, Random.Range(-180, 180)));
        }
    }
}
