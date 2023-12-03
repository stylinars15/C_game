using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    private Audio audioManager; // Reference to the Audio script

    
    void Start()
    {
        // Attempt to find the existing AudioManager in the scene
        audioManager = FindObjectOfType<Audio>();

        // If the AudioManager doesn't exist, create a new one
        if (audioManager == null)
        {
            GameObject audioManagerGO = new GameObject("AudioManager");
            audioManager = audioManagerGO.AddComponent<Audio>();
        }
    }
    
    public void Setup()
    {
        gameObject.SetActive(true);
    }
    
    public void MainMenuButton()
    {
        print("hello");
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            print("hell22222o");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            audioManager.ChangeMusic2();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
        }
    }
    
    public void ReplayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.ChangeMusic();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
