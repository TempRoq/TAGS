using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummye : EnemyScript
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

        if (inHitstun)
        {
            m_Animator.SetTrigger("Hurt");
        }
        if(!inHitstun)
            m_Animator.SetTrigger("EndHurt");

        
        if(CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].transform.position.x > transform.position.x)
        {
            gameObject.transform.localScale = new Vector3(-1,1,1);
        } else
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
              
    }



}
