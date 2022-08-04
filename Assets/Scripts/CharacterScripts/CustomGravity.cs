using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{

    public float gravityScale = 1.0f;
    public float gravityThreshold;

    float baseGravScale;
    float baseGravThresh;

    public float TerminalVelocity;



    float globalGravity;
    Rigidbody rb;
    // Start is called before the first frame update

    private void OnEnable()
    {
       
    }

    private void Awake()
    {
        
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        globalGravity = Physics.gravity.y;
         rb.useGravity = false;

        baseGravScale = gravityScale;
        baseGravThresh = gravityThreshold;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 gravity = globalGravity * Vector3.up * (Mathf.Abs(rb.velocity.y) > gravityThreshold ? gravityScale : 1f);
        rb.AddForce(gravity, ForceMode.Acceleration);

        if ( Mathf.Abs(rb.velocity.y) > TerminalVelocity)
        {
            Vector3 newVel = rb.velocity;
            newVel.y = Mathf.Clamp(newVel.y, -TerminalVelocity, TerminalVelocity);
        }
    }

    public void ChangeGravity(float globGrav, float gravScal, float gravThresh)
    {
        gravityScale = gravScal;
        gravityThreshold = gravThresh;
        globalGravity = globGrav;
    }
    
    public void ReturnBaseValues()
    {
        gravityScale = baseGravScale;
        gravityThreshold = baseGravThresh;
        globalGravity = Physics.gravity.y;

    }
}
