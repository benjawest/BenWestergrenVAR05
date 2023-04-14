using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}