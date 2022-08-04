using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*********
 * 
 * Make Enemies not group up so closely
 * Make enemy fluck better
 * 
 * Make enemies be attracted to the player's Y axis
 * 
 * 
 * 
 * 
 * 
 * 
 */
public class EnemyController : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject target;
    public List<GameObject> enemyList;
    public List<GameObject> toAttack;//Queue of enemy attacks
    public Action enemyDeath;
    //public float AttackPercent = .25f;
    //public float MinAttackers = 2;
    public float AttackDelay = 1f;
    public float atkStart = 10;

    public int maxAtks = 1;

    [SerializeField] public GameObject[] waves; //num of enemies, delay
    [SerializeField] public int[] waveSpawns;
    int waveNum = 0;
    int spawned = 0;
    int killedEnemis = 0;
    int atks = 0;
    public Vector3 pointA;
    //public Vector2Int pointB;
    bool Spawn = true;

    public bool EnemiesMove = true;
    public bool EnemiesAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        StaticEvents.OnEnemyDeath.AddListener(EnemyKill);
        enemyList = new List<GameObject>();
        atkStart = Time.time + AttackDelay + 1;
        for (int i = 0; i < enemies.Length; i++) {
            enemyList.Add(enemies[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checkDead();
        if (spawned < waveSpawns.Length && spawned <= killedEnemis)
        {
            SpawnEnemies();
        }
        if (EnemiesMove)
        {
            SendMoveInstructions();
        }
        if (EnemiesAttack && atkStart < Time.time)
        {
            
            List<GameObject> attackable_melee = GetAttackers();
            for (int i = 0; i < attackable_melee.Count; i++)
            {
                if (atks <= maxAtks)
                {
                    //toAttack.Add(attackable_melee[Mathf.RoundToInt(Random.Range(0, attackable_melee.Count - 1))]);
                    if (attackable_melee[Mathf.RoundToInt(Random.Range(0, attackable_melee.Count))].GetComponent<EnemyScript>().Attack())
                    {
                        atks += 1;
                    }
                }
            }
            if (atks > 0)
            {
                atkStart = Time.time + AttackDelay;
                atks = 0;
            }
        }
        

    }
    public void SpawnEnemies()
    {
        int a = (int)Random.Range(0, enemies.Length - 1);
        int i = 0;
        for (i = 0; i < waveSpawns[waveNum] && i+spawned < waveSpawns.Length; i++) {
            
            enemyList.Add(Instantiate(waves[spawned+i], new Vector3(pointA.x, pointA.y, pointA.z), Quaternion.identity));
        }
        spawned += i;
        waveNum += 1;
    }

    public void SpawnEnemy(GameObject e, Vector3 vec)
    {
        //Instantiate(e, new Vector3(Random.Range(pointA.x, pointB.x) + transform.position.x, waves[waveNum][i].transform.position.y, Random.Range(pointA.y, pointB.y) + transform.position.y), Quaternion.identity);
        spawned += 1;
        enemyList.Add(Instantiate(e, vec, Quaternion.identity));

    }

    public void EnemyKill()
    {
        checkDead();
    }

    public void KillEnemy()//Kills oldest enemy
    {
        if(enemyList.Count > 0)
        {
            GameObject e = enemyList[0];

            enemyList.Remove(enemyList[0]);

            Destroy(e);
        }
    }
    private void checkDead()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].GetComponent<Character>().currentHealth == 0)
            {
                enemyList[i].GetComponent<BoxCollider>().enabled = true;
                //Remove when we have death animations \/ \/ \/
                //enemyList[i].GetComponentInChildren<SpriteRenderer>().enabled = false;
                //Remove when we have death animations /\ /\ /\

                //enemyList[i].GetComponentInChildren<BoxCollider>().enabled = false;
                killedEnemis += 1;
                //enemyList[i].SetActive(false);
                enemyList.Remove(enemyList[i]);
                
            }
        }
    }
    /*
    Enemy spawning (Waves)
    Area
    Timeing of enemines: Array of enemies
    
     */

    private List<GameObject> GetAttackers()
    {
        List<GameObject> attackable_melee = new List<GameObject>();
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].GetComponent<EnemyScript>().InMeleeRange())
            {
                attackable_melee.Add(enemyList[i]);
            }
        }
        return attackable_melee;
    }



    public void SendMoveInstructions()
    {
        //print("move");
        /*List<Vector2> pos = new List<Vector2>();
        for (int i = 0; i < CharSwitchManager.instance.MainCharacterReferences.Length; i++)
        {
            if (CharSwitchManager.instance.inStage[i])
            {
                pos.Add( new Vector2(CharSwitchManager.instance.MainCharacterReferences[i].transform.position.x, CharSwitchManager.instance.MainCharacterReferences[i].transform.position.z));
            }
        }
            
        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector2 enemyPos = new Vector2(enemyList[i].transform.position.x, enemyList[i].transform.position.z);
            if (pos.Count > 0)
            {
                Vector2 closestPos = new Vector2(pos[0].x, pos[0].y);
                float distance = Vector2.Distance(enemyPos,closestPos);
                for (int j = 1; j < pos.Count; j++)
                {
                    Vector2 testPos = new Vector2(pos[j].x, pos[j].y);
                    if (Vector2.Distance(enemyPos, testPos) < distance)
                    {
                        distance = Vector2.Distance(enemyPos, closestPos);
                        closestPos = testPos;
                    }
                }
                enemyList[i].GetComponent<EnemyScript>().Move(closestPos.x, closestPos.y);
            } else
            {
                enemyList[i].GetComponent<EnemyScript>().Move(enemyList[i].transform.position.x, enemyList[i].transform.position.z);
            }
        }*/
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<EnemyScript>().Move();
        }
    }
}
