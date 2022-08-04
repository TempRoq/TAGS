using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
            Destroy(gameObject);

        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if(SceneManager.GetActiveScene().name == "End")
        {
            Destroy(gameObject);
        }
    }
}
