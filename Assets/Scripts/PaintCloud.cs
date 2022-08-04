using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(ActionHandler))]
[RequireComponent(typeof(Character))]
public class PaintCloud : MonoBehaviour
{
    ActionHandler ac;
    Character ch;
    public Action detonationAction;
   // public Action nonDetonatedAction;

    public float lifeTime;
    public float timer;

    public bool detonated = false;
    // Start is called before the first frame update
    void Start()
    {
        ch = GetComponent<Character>();
        ac = GetComponent<ActionHandler>();
        //StaticEvents.SprayPaintSnap.AddListener(Detonate);
        timer = Time.time + lifeTime;
        StaticEvents.allPaint.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= Time.time && !detonated)
        {
            StaticEvents.allPaint.Remove(this);
            Destroy(gameObject); //Fizzle out because was in play for too long.
        }
        if (!detonated)
        {
            if (!ch.inLag)
            {
               // ch.PerformAction(nonDetonatedAction, nonDetonatedAction.stopUser);
            }
        }

        else
        {
            if (!ch.inLag)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Detonate()
    {
        if (!detonated)
        {
            print("POOF!");
            ch.CancelAction();
            ch.PerformAction(detonationAction);
            gameObject.GetComponent<Animator>().SetTrigger("Snap");
            detonated = true;
        }

        
    }
}
