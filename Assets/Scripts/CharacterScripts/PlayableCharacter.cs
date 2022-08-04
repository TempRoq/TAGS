using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : Character
{
    // Start is called before the first frame update

    // Update is called once per frame



    [SerializeField]
    protected List<Action> atkBacklog;
    protected List<int> atkNumBacklog;
    [Header("Playable Character Specifics")]

    public bool controllable;
    public bool takeInputs = true;
    public int backLength;
    public CharSwitchManager.MainCharacter role;
    public float switchOutTimer;
    public float switchOutTimerMax;

    public Color colorTintLeastActions;
    public Color colorTintMaxActions;

    public GameObject ControllableIndicator;

    
    protected bool justSpawned = false;

    protected override void Start()
    {
        base.Start();
        atkBacklog = new List<Action>();
        atkNumBacklog = new List<int>();
    }

    protected override void Update()
    {
        base.Update();

        if (controllable)
        {
            ControllableIndicator.SetActive(true);
        }
        else
        {
            ControllableIndicator.SetActive(false);
        }

        if (justSpawned)
        {
            justSpawned = false;
            if (sc.HasAnim())
            {
                //sc.SetTrigger("Idle");
            }

        }


        sc.SetBool("PerformingAction", ac.performingAction);

        if (!(inHitstun || inLag) && !ac.performingAction && atkBacklog.Count > 0 && !blocking && !timeStopped)
        {
            CancelDash();// ==> The difference between dash jumping and not dash jumping
            if (atkBacklog[0] == jumpAction)
            {
                if ((isGrounded && !(inHitstun || inLag) && !ac.performingAction && !dashing))
                {
                    PerformAction(atkBacklog[0]);                   
                }
            }
            else
            {
                
                PerformAction(atkBacklog[0]);
            }
        }

        if (controllable && takeInputs)
        {
            if (!(inHitstun || inLag) && !ac.performingAction && !dashing && !blocking)
            {
                if (m.MoveCharacter(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), ref facingRight) && isGrounded)
                {
                    sc.SetBool("Moving", true);
                }
                else if (isGrounded)
                {
                    sc.SetBool("Moving", false);
                }
            }
            
         
            if (!Input.GetKey(KeyCode.Tab))
            {
                CheckControls();
            }
            
        }

       

        if (!(inHitstun || inLag) && atkBacklog.Count == 0 && !controllable)
        {
            takeInputs = true;
            sc.SetTrigger("Leave");
            gameObject.SetActive(false);

            CharSwitchManager.instance.SetInStage(role, false);
            rb.velocity = Vector3.zero;
            CancelDash();
            ac.CancelAction();

        }

        sc.SetBool("Dashing", dashing);
        sc.SetBool("Blocking", blocking);
        sc.SetBool("Grounded", isGrounded);
        sc.SetBool("Hitstun", inHitstun);
        sc.SetFloat("yVelocity", rb.velocity.y);
        sc.SetFloat("velocityMagnitude", rb.velocity.magnitude);
        sc.SetBool("InLag", inLag);

    }

    public void TryLeave()
    {
        controllable = false;
    }

    public void Summon()
    {
        //sc.SetTrigger("Summon");
        timeStopped = TimeSlowdown.instance.timeFrozen;
        gameObject.SetActive(true);
        CharSwitchManager.instance.SetInStage(role, true);
       
        controllable = true;
        justSpawned = true;
        switchOutTimer = switchOutTimerMax;
    }

    public void TryAddPerform(Action a, int i)
    {
        //idea: having a move in the queue be changed to a status effect by a spellcaster.
        if (atkBacklog.Count < backLength)
        {
            atkBacklog.Add(a);
            atkNumBacklog.Add(i);
            GetUpdatedColor();

        }
    }

    public override void PerformAction(Action a)
    {
        cg.ChangeGravity(attackGravity, cg.gravityScale, cg.gravityThreshold);

        if (!inLag && !inHitstun)
        {
            if (a.stopUser)
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                CancelDash();
            }
            sc.SetInt("ActionNumber", atkNumBacklog[0]);
            sc.SetTrigger("Action");
            ac.PerformAction(a, facingRight);
            TakeLag(a.attackLagFrames);
        }

        atkNumBacklog.RemoveAt(0);
        atkBacklog.RemoveAt(0);

        if (atkBacklog.Count == 0)
        {
            takeInputs = true;
        }
        GetUpdatedColor();

    }

    public void CheckControls()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inLag && !blocking)
            {
                atkBacklog.Clear();
                atkNumBacklog.Clear();
                GetUpdatedColor();
            }
            if (!blocking && !inHitstun && !inLag && isGrounded)
            {
                blocking = true;
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }

            else
            {
                blocking = false;
            }

        }

        sc.SetBool("Blocking", blocking);
        if (Input.GetKeyDown(KeyCode.L)) //Attack button
        {

            if (Input.GetKey(KeyCode.J))
            {


                if (atkBacklog.Count > 0 && atkBacklog[atkBacklog.Count - 1] == dashAction)
                {
                    atkBacklog[atkBacklog.Count - 1] = moves[2];
                    atkNumBacklog[atkNumBacklog.Count - 1] = 2;
                }
                else
                {
                    if (!ac.performingAction)
                    {
                        TryAddPerform(moves[2], 2); //Dash Attack
                    }
                }


            }

            //Put something here for the air attack;

            else if (Input.GetAxisRaw("Horizontal") != 0 && !ac.performingAction)
            { //Side Attack
                TryAddPerform(moves[1], 1);

            }

            else if (!ac.performingAction)
            {
                TryAddPerform(moves[0], 0); //Basic Attack
            }

        }

        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (isGrounded && !(inHitstun || inLag) && !ac.performingAction && !dashing && !blocking)
            {
                Jump();


            }
            /*
            else if (isGrounded && (dashing || inLag || ac.performingAction || blocking) && !ac.performingAction )
            {
                TryAddPerform(jumpAction, -1);
            }
            */
        }

        else if (Input.GetKeyDown(KeyCode.J) && !ac.performingAction)
        {
            if (canDash && !dashing && !inLag && !inHitstun && !blocking)
            {
                
                Dash(false);
            }
            /*else
            {
                TryAddPerform(dashAction, -2);
            }
            */

        }
    }

    public void GetUpdatedColor()
    {
        Color newColor;
        if (atkBacklog.Count == 0)
        {
            newColor = Color.white;
        }
        else
        {
            float newColorNum = (float)atkBacklog.Count / (float)backLength;
            newColor = Color.Lerp(colorTintLeastActions, colorTintMaxActions, newColorNum);
        }

        sc.ChangeColor(newColor);
    }

    public override void OnBlock()
    {
        base.OnBlock();
        blocking = false;
    }

    public override void OnTimeStop()
    {
        base.OnTimeStop();
        backLength = 100;
        takeInputs = false;
        atkBacklog.Clear();
        atkNumBacklog.Clear();
        blocking = false;

        
    }

    public override void TakeHit()
    {
        base.TakeHit();
        StaticEvents.OnPlayerDamage.Invoke();
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public override void Die()
    {
        CharSwitchManager.instance.SetSelectable(gameObject.GetComponent<PlayableCharacter>().role, false);
        int a = (int)CharSwitchManager.instance.charInPlay;
        

            //print(CharSwitchManager.instance.charInPlay - ((int)CharSwitchManager.instance.charInPlay + i - 4));
        if (a == 2)
        {
            CharSwitchManager.instance.TrySwapCharacter(CharSwitchManager.instance.charInPlay - 2, transform.position);
        }
        else 
        {
            CharSwitchManager.instance.TrySwapCharacter(CharSwitchManager.instance.charInPlay + 2, transform.position);
        }
        
        //base.Die();
    }

    public override void OnTimeResume()
    {
        base.OnTimeResume();
        if (atkBacklog.Count == 0)
        {
            takeInputs = true;
        }
        backLength = 1;
    }
}
