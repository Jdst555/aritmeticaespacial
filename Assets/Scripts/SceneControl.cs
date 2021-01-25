using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject introManager;
    private Scene currentScene;
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    void Start()
    {

       // Instantiate(introManager, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(this.gameObject); //persistir SceneControl
        //SceneManager.activeSceneChanged += setupScene;
        SceneManager.activeSceneChanged += changeScene;
    }

    private void setupScene(Scene current, Scene next)
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Main")
        {
            Instantiate(gameManager, Vector3.zero, Quaternion.identity);
        }
        if (currentScene.name == "Intro")
        {
            Instantiate(introManager, Vector3.zero, Quaternion.identity);
        }
    }

    private void changeScene(Scene current, Scene next)
    {
        if (current.name == "Main")
        {
            Instantiate(gameManager, Vector3.zero, Quaternion.identity);
        }
        if (current.name == "Intro")
        {
            Instantiate(introManager, Vector3.zero, Quaternion.identity);
        }
    }
    // Update is called once per frame
    
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }
    public void Exit()
    {
        Application.Quit();
    }

}
