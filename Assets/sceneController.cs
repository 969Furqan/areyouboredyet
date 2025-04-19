using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour
{
    // This method will be called when the button is pressed
    public void LoadScene()
    {
        SceneManager.LoadScene("level");
    }
}