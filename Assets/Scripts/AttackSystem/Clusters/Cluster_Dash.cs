using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActionCreator/Cluster/Dash")]
public class Cluster_Dash : HitboxCluster
{
    public float dashDuration;
    public float dashSpeed;
    public bool forced = false;

    // Start is called before the first frame update
    public override void OnStart(GameObject self)
    {
        base.OnStart(self);
        self.GetComponent<Character>().Dash(forced);
    }
}
