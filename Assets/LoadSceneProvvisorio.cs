using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneProvvisorio: MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void playGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void playAle()
    {
        SceneManager.LoadScene(2);
    }
    
    public void playMarco()
    {
        SceneManager.LoadScene(3);
    }
}
