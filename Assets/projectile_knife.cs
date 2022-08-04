using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActionHandler))]
[RequireComponent(typeof(Rigidbody))]
public class projectile_knife : MonoBehaviour
{
    public ActionHandler ac;
    private Movement m;
    private Rigidbody rb;
    public Action[] moves;
    public Vector3 target;
    public float speed = 1f;
    public Vector2 v;

    private bool die = false;
    // Start is called before the first frame update
    void Start()
    {
        target = CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].transform.position;
        ac = GetComponent<ActionHandler>();
        //m = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        v = new Vector2(target.x, target.z) - new Vector2(transform.position.x, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
        //m.MoveCharacter(target.x, target.y, ref ac.facingRight);
    }

    public void SetTarget(Vector2 v)
    {
        target = v;
    }

    public void Move()
    {
        
        Vector2 tempXY = new Vector2(v.x, v.y).normalized * speed;
        Vector3 newMove = new Vector3(tempXY.x, rb.velocity.y, tempXY.y);
        //print(newMove);
        rb.velocity = newMove;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            if (moves.Length > 0)
            {
                ac.PerformAction(moves[0], rb.velocity.x > 0);
                //print("attack");
            }
            Invoke("Die", Time.deltaTime);
            
        }
        if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
