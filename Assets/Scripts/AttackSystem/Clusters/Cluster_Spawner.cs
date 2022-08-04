using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ActionCreator/Cluster/Spawner")]
public class Cluster_Spawner : HitboxCluster
{

    public GameObject itemToSpawn;
    public Vector3 offsetSpawned;

    public override void OnStart(GameObject self)
    {
        base.OnStart(self);
        Vector3 scale = offsetSpawned;
        if (!self.GetComponent<Character>().GetFacingRight())
        {
            scale.Scale(new Vector3(-1, 1, 1));
        }

        GameObject g = Instantiate(itemToSpawn, self.transform.position + scale, Quaternion.identity);

        if (g.GetComponent<ActionHandler>())
        {
            g.GetComponent<ActionHandler>().currMoveFacingRight = self.GetComponent<Character>().GetFacingRight();
        }
        
    }

}
