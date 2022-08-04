using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxReciever_Player : HitboxReceiver
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    public override void TakeHit(bool enFaceRight, bool playerFacingRight, Hitbox h, float originX, out bool x)
    {
        x = false;
        base.TakeHit(enFaceRight,playerFacingRight,h,originX, out bool based);
        StaticEvents.OnPlayerDamage.Invoke();
        
    }

    public override void Die()
    {
        CharSwitchManager.SetSelectable(gameObject.GetComponent<PlayableCharacter>().role, false);
        //base.Die();
    }
    */
}
