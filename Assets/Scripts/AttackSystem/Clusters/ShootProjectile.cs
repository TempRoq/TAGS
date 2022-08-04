using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActionCreator/Cluster/SnapInvoker")]
public class ShootProjectile : HitboxCluster
{
    // Start is called before the first frame update
    public GameObject projectile;
    public override void OnStart(GameObject self)
    {
        //Instantiate(projectile, );
    }
}
