using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(HitboxReceiver))]
[RequireComponent(typeof(ActionHandler))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ShadowHandler))]
[RequireComponent(typeof(SpriteChanger))]
[RequireComponent(typeof(CustomGravity))]
public class Character : MonoBehaviour
{


    protected HitboxReceiver hbr;
    protected ActionHandler ac;
    protected Movement m;
    protected Rigidbody rb;
    protected ShadowHandler sh;
    protected SpriteChanger sc;
    protected CustomGravity cg;


    [Header("Stats")]
    public int currentHealth;
    public int maxHealth;
    public float jumpSpeed;
    public float dashDuration;
    public float dashMultiplier;
    public bool facingRight;

    [Header("Colliders")]
    public Collider floorCheck;
    public Collider bodyCollider;

    [Header("Actions")]
    public Action[] moves;
    public Action jumpAction;
    public Action dashAction;


    [Header("States")]
    public bool timeStopped;
    public bool inLag;
    public bool inHitstun;
    public bool isGrounded;
    public bool canDash;
    public bool dashing;
    public bool blocking;
    protected float hitstunTimer;
    protected float offHitstunTimer;
    protected float lagTimer;
    protected float offLagTimer;
    protected float dashTimer;
    protected float offDashTimer;


    [Header("Blocking")]
    [Range(0, 1)]
    public float blockDMGMult;
    [Range(0, 1)]
    public float blockKBMult, blockLagMult = 0f;


    [Header("Miscellaneous")]
    public LayerMask groundedLayerMask;
    public PhysicMaterial basicMaterial;
    public PhysicMaterial stunMaterial;
    public bool disappearOnDeath;
    protected bool changesSprites = false;
    protected Vector3 lastVelocity;

    [Header("Gravity")]
    public float attackGravity;
    public float HitstunGravScale;
    public float HitstunGravThresh;

  


    // Start is called before the first frame update
    protected virtual void Start()
    {
        hbr = GetComponent<HitboxReceiver>();
        ac = GetComponent<ActionHandler>();
        m = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        sh = GetComponent<ShadowHandler>();
        bodyCollider.material = basicMaterial;
        cg = GetComponent<CustomGravity>();
        if (GetComponent<SpriteChanger>() != null)
        {
            sc = GetComponent<SpriteChanger>();
            changesSprites = sc.changeSprites;

        }

        hbr.OnHit.AddListener(TakeHit);
        StaticEvents.OnTimeStop.AddListener(OnTimeStop);
        StaticEvents.OnTimeResume.AddListener(OnTimeResume);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (timeStopped)
        {
            print("time is stopped!");
            return;
        }

        if (!ac.performingAction && !inHitstun && !inLag)//&& Input.GetKeyDown(KeyCode.G)
        {
            cg.ReturnBaseValues();
        }
        


        if (inLag)
        {
            lagTimer += Time.deltaTime;
            if ( lagTimer >= offLagTimer)
            {
                inLag = false;
                bodyCollider.material = basicMaterial;
            }
        }


        if (inHitstun)
        {

            hitstunTimer += Time.deltaTime;
            if (hitstunTimer >= offHitstunTimer)
            {
                inHitstun = false;
                bodyCollider.material = basicMaterial;
                if (cg != null)
                {
                    cg.ReturnBaseValues();
                }
            }

        }

        if (dashing)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= offDashTimer)
            {
                dashing = false;
                if (!isGrounded)
                {
                    canDash = false;
                }
                rb.velocity = Vector3.zero; //What happens if this is deleted?
            }
        }   

        if (dashing)
        {
            m.Dash();
        }

        if (sc)
        {
            sc.FlipSpriteHorizontal(facingRight);
        }

    }

    private void FixedUpdate()
    {
        isGrounded = (Physics.OverlapBox(floorCheck.bounds.center, floorCheck.bounds.extents, floorCheck.transform.rotation, groundedLayerMask).Length > 0);
        if (!isGrounded)
        {
            blocking = false;
        }
        if (isGrounded && !canDash)
        {
            canDash = true;
        }
    }

    public bool GetFacingRight()
    {
        return facingRight;
    }

    public virtual void PerformAction(Action a)
    {
        //print("Performing action " + a.name);

        cg.ChangeGravity(attackGravity, cg.gravityScale, cg.gravityThreshold);

        if (!inLag && !inHitstun)
        {
            if (a.stopUser)
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                CancelDash();
            }
            ac.PerformAction(a, facingRight);
            TakeLag(a.attackLagFrames);
            if (sc)
            {
                sc.SetTrigger(a.trigger);
            }
            
        }

    }

    public void CancelAction()
    {
        ac.CancelAction();
        inLag = false;
    }

    public virtual void TakeHit()
    { //originOfAttack refers to the point where the hitbox cluster calls the "center" of the attack
        HitboxReceiver.AttackerInfo currentInfo = hbr.HitInfoReceived;
        bool successfulBlock = CheckIfBlocking(currentInfo.attackCoreX);
        if (!successfulBlock)
        {         
            blocking = false;
            CancelAction();
            CancelDash();
            cg.ChangeGravity(Physics.gravity.y, HitstunGravScale, HitstunGravThresh);
        }
        else
        {
            OnBlock();
        }
       
        
        if (!hbr.infiniteHealth)
        {
            currentHealth = Mathf.Clamp(currentHealth - (int)(currentInfo.justHitBy.damage * (successfulBlock ? blockDMGMult : 1)), 0, maxHealth);
        }

        if (hbr.takesKnockback)
        {
            Vector3 mult = currentInfo.justHitBy.knockbackDirection;
            if (!currentInfo.attackerFaceRight)
            {
                mult.x *= -1;        
            }
            if (successfulBlock)
            {
                mult.y = 0f;
                print("mult in successful block before multiplication: " + mult);
                mult *= blockKBMult;
            }
            rb.velocity = Vector3.zero;
            rb.velocity = mult * currentInfo.justHitBy.knockbackPower;
            print("mult = " + mult);
        }

        if (hbr.takesHitstun)
        {
            TakeHitstun((int)(currentInfo.justHitBy.framesHitstun * (successfulBlock ? blockLagMult : 1)), successfulBlock);
        }



        if (changesSprites)
        {
            int a = 1 + (int)(currentHealth / (maxHealth / sc.allSprites.Length - 1));
            sc.ChangeSprite(Mathf.Clamp(a, 1, sc.allSprites.Length - 1));
            if (currentHealth <= 0)
            {
                sc.ChangeSprite(0);
            }
        }


    }

    public void TakeLag(int lagAmountFrames)
    {
        inLag = true;

        lagTimer = 0f;
        offLagTimer = (lagAmountFrames / 60f);
    }

    public void TakeHitstun(int stunAmountFrames, bool successfulBlock)
    {
        if (!successfulBlock)
        {
            bodyCollider.material = stunMaterial;
        }
        inLag = false;
        lagTimer = 0f;
        offLagTimer = 0f;

        inHitstun = true;
        hitstunTimer = 0;
        offHitstunTimer = (stunAmountFrames / 60f);
    }
    public virtual bool CheckIfBlocking(float originX)
    {
        print("CHECK IF BLOCKING:\nfacingRight = " + facingRight + "\noriginX = " + originX + "\ntransform.position.x = " + transform.position.x + "\n should return " + (blocking && ((facingRight && transform.position.x <= originX) || (!facingRight && transform.position.x >= originX))));
        return blocking && ((facingRight && transform.position.x <= originX) || (!facingRight && transform.position.x >= originX));
    }

    public virtual void Die()
    {
        if (disappearOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            foreach (Collider c in GetComponents<Collider>())
            {
                c.enabled = false;
            }
        }

    }

    public Vector3 GetShadowPosition()
    {
        return sh.GetShadowPosition();
        
    }

    public void CancelDash()
    {
        dashing = false;
    }

    public void Dash(bool force)
    {

        if (canDash || force)
        {
            m.BeginDash(dashMultiplier, facingRight);
            canDash = false;
            
        }

        else if (!canDash && !isGrounded)
        {
            m.BeginDash(dashMultiplier * 5, Vector3.down, ref facingRight);

        }

        dashing = true;
        dashTimer = 0;
        offDashTimer = dashDuration;
        if (sc)
        {
            sc.SetTrigger("Dash");
        }

    }

    public bool TryJump()
    {
        if (isGrounded && !inHitstun && !dashing)
        {
            Jump();
            return true;
        }
        return false;
    }

    public void Jump()
    {
        m.Jump(jumpSpeed);
        if (sc)
        {
            sc.SetTrigger("Jump");
        }
        
    }

     private void OnDrawGizmos()
    {
        if (inLag)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(transform.position, Vector3.one / 2);
        }

        else if (inHitstun)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, Vector3.one / 2);
        }

        else if (blocking)
        {
            if (facingRight)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(transform.position, Vector3.one / 2);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(transform.position, Vector3.one / 2);
            }
        }

    }

    public virtual void OnBlock()
    {
        print("You blocked!");
    }

    public virtual void OnTimeStop()
    {
        timeStopped = true;
        lastVelocity = rb.velocity;
        sc.PauseAnimator(true);
        cg.ReturnBaseValues();
        CancelAction();
        CancelDash();
        rb.isKinematic = true;
    }

    public virtual void OnTimeResume()
    {
        timeStopped = false;
        cg.ReturnBaseValues();
        rb.velocity = lastVelocity;
        sc.PauseAnimator(false);
        rb.isKinematic = false;
    }


}
