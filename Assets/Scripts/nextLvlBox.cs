using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextLvlBox : MonoBehaviour
{
    public string nextScene;
    public EnemyController enem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 && enem.enemyList.Count == 0)
        {
            //CharSwitchManager.TrySwapCharacter(CharSwitchManager.MainCharacter.BRAWLER, other.transform.position + Vector3.up * 2);
            //other.gameObject.GetComponent<Character>().enabled = false;
            Invoke("ChangeScene", 0.0f);
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
    }

}
