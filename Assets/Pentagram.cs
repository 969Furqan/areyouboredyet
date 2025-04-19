using UnityEngine;
using UnityEngine.SceneManagement;

public class Pentagram : MonoBehaviour
{
    private bool playerInRange = false;  

    void Update()
    {
        
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ReloadCurrentScene();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;  
        }
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
