using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endingSeedSpawner : MonoBehaviour
{
    //Seed stats
    [SerializeField]
    private GameObject prefabToSpawn = null;

    // Start is called before the first frame update
    void Start()
    {
        //Spawn the number of seeds the pet eaten in the game
        for (int i = 0; i < scoreManager.Instance.seedEaten; i++)
        {
            Instantiate(prefabToSpawn, new Vector2(Random.Range(1, 7), 5), Quaternion.Euler(0, 0, Random.Range(-180, 180)));
        }
    }
}
