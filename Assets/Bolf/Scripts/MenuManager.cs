using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("Scene2", LoadSceneMode.Single);
    }

    public void LoadScene4()
    {
        SceneManager.LoadScene("Scene4", LoadSceneMode.Single);
    }

    public void UnloadMenu()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
