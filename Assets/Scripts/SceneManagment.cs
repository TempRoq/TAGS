using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{

    string levelchange;


    public void Awake()
    {

    }

    public void ChangeScene(string level)
    {
        SceneManager.LoadScene(level);
        PlayerPrefs.SetInt("Water", 0);
    }


    public void ChangeSceneCheese(string level)
    {
        StartCoroutine(ChangeSceneInvoke(level));
    }

    public IEnumerator ChangeSceneInvoke(string level)
    {
        //yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level);
        PlayerPrefs.SetInt("Water", 0);
        yield return new WaitForSeconds(1f);
    }
        void Scenery()
    {
        SceneManager.LoadScene(levelchange);

    }

    public void doExitGame()
    {
        Application.Quit();
    }

}
