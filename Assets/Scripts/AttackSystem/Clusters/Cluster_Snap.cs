using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActionCreator/Cluster/SnapInvoker")]
public class Cluster_Snap : HitboxCluster
{
    // Start is called before the first frame update
    public override void OnStart(GameObject self)
    {
        base.OnStart(self);
        StaticEvents.SprayPaintSnap.Invoke();
    }
}
