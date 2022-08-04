using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Melee : EnemyScript
{
    private float yDistanceDiff = 2;
    public Animator m_Animator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override bool Attack()
    {
        if (moves.Length > 0) {
            if (!ac.performingAction && !inLag && !inHitstun)
            {
                m_Animator.SetTrigger("Attack");
                ac.PerformAction(moves[Mathf.RoundToInt(Random.Range(0, moves.Length - 1))], facingRight);
                return true;
            }
            return false;
        }
        return false;
    }

    public override bool InMeleeRange()
    {
        Collider[] a = Physics.OverlapSphere(transform.position, idealDistance,m_layerMask);
        Vector2 closestPos = new Vector2(transform.position.x, transform.position.z);
        if (a.Length > 0)
        {
            closestPos = new Vector2(a[0].gameObject.transform.position.x, a[0].gameObject.transform.position.z);
            float distance = Vector2.Distance(transform.position, closestPos);
            for (int j = 1; j < a.Length; j++)
            {
                Vector2 testPos = new Vector2(a[j].gameObject.transform.position.x, a[j].gameObject.transform.position.z);
                if (Vector2.Distance(transform.position, testPos) < distance)
                {
                    distance = Vector2.Distance(transform.position, closestPos);
                    closestPos = testPos;
                }
            }
        }
        if (yDistanceDiff > Mathf.Abs(closestPos.y - transform.position.z)) {
            return a.Length > 0;
        } else
        {
            return false;
        }
    }


    private bool AgainstWall(float x, float z)//This script tells the enemy to move around walls
    {
        LayerMask mask = LayerMask.GetMask("Environment(No Hit)");
        if(transform.position.z < z)
        {
            return (Physics.OverlapBox(transform.position + new Vector3(0, 1, 0.5f), new Vector3(0.5f, 0.2f, 0.5f), Quaternion.identity, mask).Length > 0);
        } else if (transform.position.z > z)
            {
                return (Physics.OverlapBox(transform.position + new Vector3(0, 1, -0.5f), new Vector3(0.5f, 0.2f, 0.5f), Quaternion.identity, mask).Length > 0);
            }
        return false;
        //return Physics.Raycast(transform.position + new Vector3(-1,1,0), new Vector3(x, transform.position.y+1, z), Vector2.Distance(transform.position, new Vector2(x, z)), mask) || Physics.Raycast(transform.position + new Vector3(1, 1, 0), new Vector3(x, transform.position.y + 1, z), Vector2.Distance(transform.position, new Vector2(x, z)), mask);
        //Collider[] a = Physics.OverlapBox(transform.position + new Vector3(0, 1, 0), new Vector3(1,0.1f,1), Quaternion.identity, mask);
        //return (a.Length > 0);
    }
    public override void MoveAction(float x, float z)
    {
        if (inHitstun)
        {
            m_Animator.SetTrigger("Hurt");
        }
        if (!ac.performingAction && !inLag && !inHitstun)
        {
            
            Vector2 t = new Vector2(x, z) - new Vector2(transform.position.x, transform.position.z);
            if (Mathf.Abs(x - transform.position.x) < 1f && Mathf.Abs(z - transform.position.z) < 0.5f)
            {
                //m_Animator.SetTrigger("Idle");
                m.MoveCharacter(0, 0, ref facingRight);
            }
            else
            {
                m_Animator.SetBool("Walk", true);
                //print("walk");
                if (!AgainstWall(x, z))
                    m.MoveCharacter(t.x, t.y * 5, ref facingRight);
                else
                    m.MoveCharacter(t.x, 0, ref facingRight);
            }
        } else
        {
            m_Animator.SetBool("Walk",false);
        }

        /*Vector2 tempXY = t.normalized * speed;
        Vector3 newMove = new Vector3(tempXY.x, rb.velocity.y, 2 * tempXY.y);
        rb.velocity = newMove;*/
    }



}
