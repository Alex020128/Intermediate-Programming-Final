using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class musicManager : Singleton<musicManager>
{
    //Background music loops
    public AudioSource audioSource;
    
    public AudioClip currentMusic;
    
    public AudioClip titleLoop;
    public AudioClip gameLoop;
    public AudioClip endingLoop;

    void Awake()
    {
        name = "MusicManager"; // Set name of object
        
        //Assign variables
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Play the music loop according to the scene
        if(currentMusic != titleLoop && "titleScreen" == SceneManager.GetActiveScene().name)
        {
            audioSource.Stop();
            audioSource.clip = titleLoop;
            audioSource.Play();
            currentMusic = titleLoop;
        }
        if (currentMusic != gameLoop && "gameScreen" == SceneManager.GetActiveScene().name)
        {
            audioSource.Stop();
            audioSource.clip = gameLoop;
            audioSource.Play();
            currentMusic = gameLoop;
        }
        if (currentMusic != endingLoop && "endingScreen" == SceneManager.GetActiveScene().name)
        {
            audioSource.Stop();
            audioSource.clip = endingLoop;
            audioSource.Play();
            currentMusic = endingLoop;
        }
    }
}
