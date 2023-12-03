using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio instance;
    public AudioSource audioSource;
    public AudioSource audioSource2;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Ensures only one AudioManager exists
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeMusic()
    {
        audioSource.Stop(); // Stop the first audio source
        audioSource2.Play(); // Start playing the second audio source
    }
    
    public void ChangeMusic2()
    {
        audioSource.Play(); // Stop the first audio source
        audioSource2.Stop(); // Start playing the second audio source
    }
}