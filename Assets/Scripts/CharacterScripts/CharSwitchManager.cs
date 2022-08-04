using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSwitchManager : Singleton<CharSwitchManager>
{
    public enum MainCharacter
    {
       BRAWLER = 0,    
       SCIENTIST = 1,
       SPRAY_PAINTER = 2,
       MUSICIAN = 3
    }
    public bool[] selectable;
    public bool[] inStage;

    public MainCharacter startingCharacter;

    public MainCharacter charInPlay;
    public GameObject[] MainCharacterReferences;

    public GameObject[] spawnChars;
    //public bool[] initialSelectable;

    protected override void Awake()
    {
        print("does it go here?");
        base.Awake();
        charInPlay = startingCharacter;
        for (int i = 0; i < MainCharacterReferences.Length; i++)
        {
            if (MainCharacterReferences[i] != null)
            {
                MainCharacterReferences[i].transform.position = transform.position;
            }
        }
        
        //Destroy(gameObject);



    }

    private void Start()
    {
            
            inStage = new bool[4];
            selectable = new bool[4];

            //SetCharacters(initialSelectable);
            BeginSpawnCharacters();
    }




    public void BeginSpawnCharacters()
    {
        MainCharacterReferences = new GameObject[4];       
        for(int i = 0; i < spawnChars.Length; i++)
        {
           
            GameObject g = Instantiate(spawnChars[i], transform.position, Quaternion.identity);
            DontDestroyOnLoad(g);
            int nRole = (int)g.GetComponent<PlayableCharacter>().role;
            MainCharacterReferences[nRole] = g;
            selectable[(int)MainCharacterReferences[nRole].GetComponent<PlayableCharacter>().role] = true;
            if (g.GetComponent<PlayableCharacter>().role != startingCharacter)
            {
                g.SetActive(false);
                g.GetComponent<PlayableCharacter>().controllable = false;
                inStage[nRole] = false;
            }
            else
            {
                g.SetActive(true);
                g.GetComponent<PlayableCharacter>().controllable = true;
                inStage[nRole] = true;
            }
        }
    }

    public void SetCharacters(bool[] b)
    {
        for (int i = 0; i < b.Length; i++)
        {
            selectable[i] = b[i];
        }
    }

    public bool TrySwapCharacter(MainCharacter p, Vector3 location)
    {
        if (p == charInPlay)
        {
            print("Switching to character failed! Already out!");
            return false;
        }
        if (selectable[(int)p])
        {
            if (!inStage[(int)p])
            {
                MainCharacterReferences[(int)p].transform.position = location;
                MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().TryLeave();
                MainCharacterReferences[(int)p].GetComponent<PlayableCharacter>().Summon();

                

                charInPlay = p;
                StaticEvents.OnPlayerDamage.Invoke();
                return true;
            }
            else
            {
                MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().TryLeave();
                MainCharacterReferences[(int)p].GetComponent<PlayableCharacter>().controllable = true;
                

                charInPlay = p;
                StaticEvents.OnPlayerDamage.Invoke();
                return true;
            }
        }
        print("Character is not selectable right now!");
        return false;
    }

    public void SetSelectable(MainCharacter p, bool b)
    {

        selectable[(int)p] = b;

        int numLeft = 0;
        for (int i = 0; i < selectable.Length; i++)
        {
            if (selectable[i])
            {
                numLeft += 1;
            }
        }

       
        if(numLeft == 0)
        {
            //Replace this
            SceneManager.LoadScene("Title");
            MainCharacterReferences[0].GetComponent<Character>().currentHealth = MainCharacterReferences[0].GetComponent<Character>().maxHealth;
            selectable[0] = true;
            MainCharacterReferences[2].GetComponent<Character>().currentHealth = MainCharacterReferences[2].GetComponent<Character>().maxHealth;
            selectable[2] = true;

            //Game Over Shit goes here!!!
        }
        else
        {
            RotateCharacters(MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().GetShadowPosition() +
                   new Vector3(0f, MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().bodyCollider.bounds.extents.y, 0f));
        }
    }

    public void SetInStage(MainCharacter p, bool b)
    {
        inStage[(int)p] = b;
    }

    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.Tab))
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                print("Trying to switch to Spray Painter");
                TrySwapCharacter(MainCharacter.SPRAY_PAINTER, 
                    MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().GetShadowPosition() + 
                    new Vector3(0f, MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().bodyCollider.bounds.extents.y, 0f));

            }

            else if (Input.GetKeyDown(KeyCode.L))
            {
                TrySwapCharacter(MainCharacter.BRAWLER, 
                    MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().GetShadowPosition() + 
                    new Vector3(0f, MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().bodyCollider.bounds.extents.y, 0f));
            }


        }
        */

        if (!TimeSlowdown.instance.timeFrozen)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                print("Switching to next character in rotation");
                RotateCharacters(MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().GetShadowPosition() +
                        new Vector3(0f, MainCharacterReferences[(int)charInPlay].GetComponent<PlayableCharacter>().bodyCollider.bounds.extents.y, 0f));
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    private void RotateCharacters(Vector3 loc)
    {
        int numSelect = 0;
        for (int i = 0; i < selectable.Length; i++)
        {
            if (selectable[i])
            {
                numSelect += 1;
            }
        }

        if (numSelect < 2)
        {
            return;
        }
        bool validChar = false;
        int a = (int)charInPlay;
        while (!validChar)
        {
            a += 1;
            if (a >= MainCharacterReferences.Length)
            {
                a = 0;
            }
            validChar = selectable[a];
        }
        TrySwapCharacter((MainCharacter)a, loc);
        
    }

    public override void OnAlreadyLoaded()
    {
        print("deleting self. Reference to instance = " + instance.name);
        CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].transform.position = this.transform.position;
        for (int i = 0; i < MainCharacterReferences.Length; i++)
        {
            if (MainCharacterReferences[i] != null)
            {
                MainCharacterReferences[i].transform.position = transform.position;
            }
        }
        base.OnAlreadyLoaded();
    }
}

