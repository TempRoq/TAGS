using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxReciever_enemy : HitboxReceiver
{
    // Start is called before the first frame update
    public Animator m_Animator;
    bool dead = false;
    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public override void Die()
    {
        
        StaticEvents.OnEnemyDeath.Invoke();
        if (disappearOnDeath)
        {
            dead = true;
            Invoke("dieHard", 1f);
            print("Die");
            m_Animator.SetBool("Die",true);
            //GetComponent<CustomGravity>().gravityScale = 0;
            /*foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
            }*/
    /*
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

    private void dieHard()
    {
        Destroy(gameObject);
    }

    public override void OnGetHit()
    {
        if (!dead)
        {
            Invoke("Hit", Time.deltaTime);
            base.OnGetHit();
        }
    }

    private void Hit()
    {
        m_Animator.SetTrigger("Damage");
    }
    */
}
