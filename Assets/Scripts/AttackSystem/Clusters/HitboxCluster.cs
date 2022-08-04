using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActionCreator/Cluster/Basic", order = 0)]
public class HitboxCluster : ScriptableObject
{
    // Start is called before the first frame update
    public int durationInFrames; //Assume 60fps
    public Hitbox[] hitboxes; //all hitboxes that will be active while this cluster is active. Hitbox priority is based on how low the index of a hitbox is.
    public bool refreshHitList; //Will this cluster clear "alreadyHit" in the attackHandler? TRUE = yes (good for multihits), False = no (good for lingeringHitboxes)
    public float xOriginPointOffset; //how far in the X direction is the "core" of the move? The player must be blocking in this direction in order 
    public bool playInvokeSFX;
    public string sfxFileName;
   // FMOD.Studio.EventInstance invokeSFX;
    public virtual void OnStart(GameObject self) //Called on the first frame a cluster is active
    {
        if (playInvokeSFX)
        {
           // invokeSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/" + sfxFileName);
           // invokeSFX.start();
           // invokeSFX.release();
        }
    }

    public virtual void OnUpdate(GameObject self) //Called every frame a cluster is active
    {

    }
}
