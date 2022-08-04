using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.SceneManagement;

public class TrainingModeMenu : MonoBehaviour
{
    public CharSwitchManager cc;
    public EnemyController ec;
    public EventSystem ev;
    public StandaloneInputModule im;
    public GameObject defaultGO;


    public GameObject[] enemyArray;
    public int enemyArraySelected = 0;

    public GameObject[] entityArray;
    public int entityArraySelected = 0;

    public bool enemiesAttack = false;
    public bool enemiesMove = false;

    public int numOfEnemies = 1;

    public bool menuSwitch = false;
    public Canvas TrainingMenu;

    public SpriteRenderer bg;
    public Sprite[] backgrounds;
    public int selectedBackground;

    public bool enemyImortality = true;
    public bool playerImortality = true;

    public List<GameObject> breakableObjects;

    public bool playerControlToggle = true;

    // Start is called before the first frame update
    void Start()
    {
        //Add event listener for move button
        /*StaticEvents_Training.EnemiesMoveSwitch.AddListener(SwitchMove);
        //Add event listener for attack button
        StaticEvents_Training.EnemiesAttackSwitch.AddListener(SwitchAttack);
        //Add event listener for Enemy number change
        StaticEvents_Training.EnemiesNumberChange.AddListener(EnemyCount);
        //Add event listener for spawn breakables
        StaticEvents_Training.SpawnBreakable.AddListener(SpawnBreakable);
        //Add event listener for slowdown full
        StaticEvents_Training.PlayerFillMeter.AddListener(FillMeter);
        //Add event listener for scene change
        StaticEvents_Training.SceneChange.AddListener(SceneChange);*/
        defaultGO = ev.currentSelectedGameObject;
        breakableObjects = new List<GameObject>();
        ec.EnemiesMove = enemiesMove;
        ec.EnemiesAttack = enemiesAttack;
        playerControlToggle = false;
        playerControlToggler();
        menuToggle();

    }

    void Awake()
    {
        /*defaultGO = ev.currentSelectedGameObject;
        breakableObjects = new List<GameObject>();
        ec.EnemiesMove = enemiesMove;
        ec.EnemiesAttack = enemiesAttack;
        menuSwitch = true;
        menuToggle();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemy();
        }
        if (Input.GetButtonDown("menu"))
        {
            playerControlToggler();
            menuToggle();
        }
        //Create all enemies are imortal button (Static event probably)
    }

    public void SwitchMove()
    {
        enemiesMove = !enemiesMove;
        ec.EnemiesMove = enemiesMove;
        setAllIdle();
    }

    public void SwitchAttack()
    {
        enemiesAttack = !enemiesAttack;
        ec.EnemiesAttack = enemiesAttack;
    }

    public void SpawnEnemy()
    {
        ec.SpawnEnemy(enemyArray[enemyArraySelected], CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].transform.position);
        EnemyCount(numOfEnemies);
    }

    public void SpawnEnemyChoice(int n)
    {
        ec.SpawnEnemy(enemyArray[n], CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].transform.position);
        ec.enemyList[ec.enemyList.Count - 1].GetComponent<HitboxReceiver>().infiniteHealth = enemyImortality;
        EnemyCount(numOfEnemies);
    }

    public void EnemyCount(int num)
    {
        numOfEnemies = num;
        while(ec.enemyList.Count > num)
        {
            ec.KillEnemy();
        }
        //Get value of UI player num, then message enemy script to remove or
        //add slots based on need (deleting oldest one first)
    }

    public void SpawnBreakable()
    {
        breakableObjects.Add(SpawnEntity(entityArray[entityArraySelected], CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].transform.position));
        //Make generic spawning script for this
    }

    public GameObject SpawnEntity(GameObject e, Vector3 vec)
    {
        //Instantiate(e, new Vector3(Random.Range(pointA.x, pointB.x) + transform.position.x, waves[waveNum][i].transform.position.y, Random.Range(pointA.y, pointB.y) + transform.position.y), Quaternion.identity);
        return Instantiate(e, vec, Quaternion.identity);
    }

    public void SpawnBreakableChoice(int n)
    {
        breakableObjects.Add(SpawnEntity(entityArray[n], CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].transform.position));
        
        //Make generic spawning script for this
    }

    public void ClearBreakable()
    {
        for(int i = 0; i < breakableObjects.Count;){
            GameObject b = breakableObjects[i];
            breakableObjects.RemoveAt(i);
            Destroy(b);
        }
    }

    public void FillMeter()
    {
        //talk to cc to fill the meter
        TimeSlowdown.instance.GiveMeter(100);
    }

    public void BackgroundChange()
    {
        //Get scene selected from UI [scene] *go button*
        selectedBackground += 1;
        if (selectedBackground >= backgrounds.Length)
        {
            selectedBackground = 0;
        }
        bg.sprite = backgrounds[selectedBackground];
    }

    public void menuToggle()
    {
        menuSwitch = !menuSwitch;
        TrainingMenu.enabled = menuSwitch;
 
        //ev.firstSelectedGameObject = defaultGO;
        im.enabled = menuSwitch;
        ev.enabled = menuSwitch;
        if (menuSwitch)
        {
            ev.SetSelectedGameObject(defaultGO);
        }
        setAllIdle();
    }

    public void enemyImortalityToggle()
    {
        enemyImortality = !enemyImortality;
        for(int i = 0; i < ec.enemyList.Count; i++)
        {
            ec.enemyList[i].GetComponent<HitboxReceiver>().infiniteHealth = enemyImortality;
        }
    }

    public void playerImortalityToggle()
    {
        playerImortality = !playerImortality;
        CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].GetComponent<HitboxReceiver>().infiniteHealth = playerImortality;
    }

    public void playerControlToggler()
    {
        playerControlToggle = !playerControlToggle;
        //cc.canSwitch = playerControlToggle;
        CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].GetComponent<Character>().enabled = playerControlToggle;
    }

    public void setAllIdle()
    {
        for (int i = 0; i < ec.enemyList.Count; i++)
        {
            ec.enemyList[i].GetComponentInChildren<Animator>().SetBool("Walk", false);
        }
        //CharSwitchManager.MainCharacterReferences[(int)CharSwitchManager.charInPlay].GetComponentInChildren<Animator>().SetBool("Moving", false);
    }

}
