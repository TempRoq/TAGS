using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged: EnemyScript
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
                
                ac.PerformAction(moves[Mathf.RoundToInt(Random.Range(0, moves.Length - 1))], facingRight);
                m_Animator.SetTrigger("Attack");
                return true;
            }
            return false;
        }
        return false;
    }

    public override bool InMeleeRange()
    {
        /*Collider[] a = Physics.OverlapSphere(transform.position, idealDistance, m_layerMask);
        Vector2 closestPos = new Vector2(transform.position.x, transform.position.z);
        if (a.Length > 0)
        {
            return false;
        }*/
        return true;
    }

    private bool RunAway()
    {
        Collider[] a = Physics.OverlapBox(transform.position, new Vector3(idealDistance, idealDistance, idealDistance),Quaternion.identity, m_layerMask);
        Vector2 closestPos = new Vector2(transform.position.x, transform.position.z);
        if (a.Length > 0)
        {
            return true;
        }
        return false;
    }

    public override void MoveAction(float x, float z)
    {
        if (inHitstun)
        {
            m_Animator.SetTrigger("Hurt");
        }
        if (!ac.performingAction && !inLag && !inHitstun)
        {
            if (RunAway())
            {
                m_Animator.SetBool("Walk", true);
                Vector2 t = new Vector2(transform.position.x, transform.position.z) - new Vector2(x, z);
                m.MoveCharacter(t.x, t.y, ref facingRight);
            } else
            {
                if (Mathf.Abs(z - transform.position.z) > 0.5f)
                {
                    m_Animator.SetBool("Walk",true);
                    Vector2 t = new Vector2(0, z) - new Vector2(0, transform.position.z);
                    m.MoveCharacter(-0.1f, t.y, ref facingRight);
                } else
                {
                    //m_Animator.SetTrigger("Idle");
                    m.MoveCharacter(0, 0, ref facingRight);
                }
            }
        } else
        {
            m_Animator.SetBool("Walk", false);
        }

        /*Vector2 tempXY = t.normalized * speed;
        Vector3 newMove = new Vector3(tempXY.x, rb.velocity.y, 2 * tempXY.y);
        rb.velocity = newMove;*/
    }
}
