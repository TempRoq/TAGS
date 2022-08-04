using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Character
{
    bool inAction = false;
    public float idealDistance = 5f;
    private float speed = 2f;
    public Character character;
    public Animator m_Animator;
    //public Rigidbody rb2;

    [SerializeField] public LayerMask m_layerMask;
    // Start is called before the first frame update
     protected override void Start()
    {
        base.Start();
        //m = GetComponent<Movement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.Update();
        if(!inAction)
        {
            //Code to decide on attack vs Moving

        }
    }

    public virtual bool InMeleeRange()
    {
        return (false);
    }

    public virtual bool Attack()
    {
        return false;
    }

    public override void TakeHit()
    {
        base.TakeHit();
        if(currentHealth <= 0)
        {
            StaticEvents.OnEnemyDeath.Invoke();
            if (disappearOnDeath)
            {
                Invoke("dieHard", 1f);
                print("Die");
                m_Animator.SetBool("Die", true);
                GetComponent<CustomGravity>().gravityScale = 0;
                foreach (Collider c in GetComponents<Collider>())
                {
                    c.enabled = false;
                }
                
                GetComponent<Character>().enabled = false;
                GetComponent<Movement>().enabled = false;
                //Destroy(gameObject);
            }
            else
            {

                foreach (Collider c in GetComponents<Collider>())
                {
                    c.enabled = false;
                }
            }
        }
    }
    

    public virtual void Move()
    {
        List<Vector2> pos = new List<Vector2>();
        for (int i = 0; i < CharSwitchManager.instance.MainCharacterReferences.Length; i++)
        {
            if (CharSwitchManager.instance.inStage[i])
            {
                pos.Add(new Vector2(CharSwitchManager.instance.MainCharacterReferences[i].transform.position.x, CharSwitchManager.instance.MainCharacterReferences[i].transform.position.z));
            }
        }
        if (pos.Count > 0)
        {
            Vector2 closestPos = new Vector2(pos[0].x, pos[0].y);
            float distance = Vector2.Distance(transform.position, closestPos);
            for (int j = 1; j < pos.Count; j++)
            {
                Vector2 testPos = new Vector2(pos[j].x, pos[j].y);
                if (Vector2.Distance(transform.position, testPos) < distance)
                {
                    distance = Vector2.Distance(transform.position, closestPos);
                    closestPos = testPos;
                }
            }
            MoveAction(closestPos.x, closestPos.y);
        }
        else
        {
            MoveAction(transform.position.x, transform.position.z);
        }
        
        /*Vector2 tempXY = t.normalized * speed;
        Vector3 newMove = new Vector3(tempXY.x, rb.velocity.y, 2 * tempXY.y);
        rb.velocity = newMove;*/
    }

    public virtual void MoveAction(float x, float z)
    {
        if (!ac.performingAction && !inLag && !inHitstun)
        {
            Vector2 t = new Vector2(x, z) - new Vector2(transform.position.x, transform.position.z);
            m.MoveCharacter(t.x, t.y, ref facingRight);
        }
    }

}