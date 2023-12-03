using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public Controls Controls;
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
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
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
        if (collision.gameObject.CompareTag("Player") && enemySpawner.CountActiveEnemies() == 0)
        {
            audioManager.ChangeMusic();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            print("else");
            Controls.active_deactivate();
        }
    }

}
