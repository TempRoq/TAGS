using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2;
    Vector3 lastMove;
    float dashSpeedMult;
    

    //Should dashing decelerate the player?


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastMove = Vector3.right;
    }

    // Update is called once per frame

    public bool MoveCharacter(float x, float z, ref bool facingRight)
    {
        bool moving = true;
        Vector2 tempXY = new Vector2(x, z).normalized * speed;
        Vector3 newMove = new Vector3(tempXY.x, rb.velocity.y, 2 * tempXY.y);
        Vector3 temp = new Vector3(tempXY.x, 0f, 2 * tempXY.y);
        if (temp != Vector3.zero)
        {
            lastMove = temp;
        }
        else
        {
            lastMove = (Vector3.right * speed * (facingRight ? 1 : -1));
            moving = false;
        }

        if (x > 0)
        {
            facingRight = true;
            //gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }

        if (x < 0)
        {
            facingRight = false;
            //gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        rb.velocity = newMove;
        return moving;
    }

    public void Jump(float jumpSpeed)
    {
        Vector3 newSpeed = rb.velocity;
        newSpeed.y = jumpSpeed;
        rb.velocity = newSpeed;
        //gameObject.GetComponentInChildren<Animator>().SetTrigger("Jump");
       // rb.AddForce(new Vector3(0, 2f * jumpSpeed, 0f), ForceMode.Force);
    }


    public void BeginDash(float newDashSpeedMult, Vector3 newLastDirection, ref bool facingRight)
    {
        dashSpeedMult = newDashSpeedMult;
        
        if (newLastDirection.x > 0)
        {
            facingRight = true;
        }
        else if (newLastDirection.x < 0)
        {
            facingRight = false;
        }

        lastMove = newLastDirection;
    }

    public void BeginDash(float newDashSpeedMult, bool facingRight)
    {
        dashSpeedMult = newDashSpeedMult;
        lastMove = (Vector3.right * speed * (facingRight ? 1 : -1));
    }

    public void Dash()
    {
        rb.velocity = lastMove  * dashSpeedMult;
    }
        
}
