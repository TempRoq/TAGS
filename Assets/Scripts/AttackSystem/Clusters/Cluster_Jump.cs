using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ActionCreator/Cluster/Jump")]
public class Cluster_Jump : HitboxCluster
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void OnStart(GameObject self)
    {
        base.OnStart(self);
        if (self.GetComponent<Character>().TryJump())
        {
            self.GetComponent<ActionHandler>().CancelAction();
        }
    }
}
