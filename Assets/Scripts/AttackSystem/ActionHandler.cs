using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActionHandler : MonoBehaviour
{
    //ROUGH RUNDOWN:
    //If there are still frames left in the given cluster, instantiate the hitboxes in the cluster.
    //Otherwise, if there are clusters remaining, check if there is a cluster that starts on this frame (and set appropriate variables if there is).
    //If there are no clusters remaining, the player is no longer performing an action.
    //Note that not being in an action does not mean that the player is no longer in lag. DrawGizmo is solely there for debugging purposes.

    public LayerMask targetsToHit; //What layers can this attacker hit
    public bool currMoveFacingRight;
    Dictionary<int, HitboxCluster> frameToAction;
    Action currAction;
    HitboxCluster currCluster;
    public int currentFrame;
    public int framesRemaining;
    public int numClustersRemaining = 0;
    public bool performingAction = false;

    public bool drawGizmo = false;

    List<GameObject> alreadyHit;

    // Start is called before the first frame update
    void Start()
    {
        alreadyHit = new List<GameObject>();
        frameToAction = new Dictionary<int, HitboxCluster>();
    }

    private void FixedUpdate()
    {
        //If the player is performing an action, if a new cluster is hit, pe
        if (performingAction) 
        {
            UpdateInfoFrame(currentFrame);

            if (framesRemaining <= 0) { //if, even after that, the number of frames remaining is still less than or equal to 0, do not show gizmos. if there are clusters remaining, currCluster is null. 
                drawGizmo = false;
                currCluster = null;
                if (numClustersRemaining == 0)
                {        
                    performingAction = false;
                }
            }
            else //if there are frames remaining, just build the damn hitboxes.
            {   
                BuildHitboxes();
            }
            currentFrame += 1;
        }
    }

    public void PerformAction(Action a, bool newfacingRight)
    {
        currMoveFacingRight = newfacingRight;
        frameToAction.Clear();
        alreadyHit.Clear();
        for (int i = 0; i < a.clusters.Length; i++) {
            frameToAction.Add(a.clusterFrames[i], a.clusters[i]);
        }
        currAction = a;
        numClustersRemaining = a.clusters.Length;
        
        performingAction = true;
        currentFrame = 0;
        currAction.OnInvoked(gameObject);
        framesRemaining = 0;

    }

    public void UpdateInfoFrame(int frame)
    {
        //print("updateInfoFrameCalled on frame " + frame);
        if (frameToAction.TryGetValue(frame, out HitboxCluster hbc))
        {
            currCluster = hbc;
            framesRemaining = hbc.durationInFrames;
            numClustersRemaining -= 1;
            if (hbc.refreshHitList)
            {
                alreadyHit.Clear();
            }
            currCluster.OnStart(gameObject);
        }
    }


    public void BuildHitboxes() //Place the hitboxes into the scene and see if they interact with anything 
    {
        drawGizmo = true;
        currCluster.OnUpdate(gameObject);
        foreach (Hitbox h in currCluster.hitboxes)
        {
            Vector3 additional = h.offsetFromAnchor;

            Collider[] cols = Physics.OverlapBox(transform.position + new Vector3(h.offsetFromAnchor.x * (currMoveFacingRight ? 1 : -1), h.offsetFromAnchor.y), h.dimensions / 2, Quaternion.identity, targetsToHit, QueryTriggerInteraction.Collide);
            foreach (Collider c in cols)
            {
                GameObject g = c.gameObject;
                try
                {
                    if (!alreadyHit.Contains(g))
                    {
                        g.GetComponent<HitboxReceiver>().TakeHit(gameObject, currMoveFacingRight, h, transform.position.x + (currCluster.xOriginPointOffset * (currMoveFacingRight ? 1 : -1)));
                        alreadyHit.Add(g);
                        h.OnHit(gameObject, g);

                    }
                }
                catch (Exception)
                {

                    Debug.LogError("GameObject " + g.name + " has the wrong Layer!");
                }
            }
        }
        framesRemaining -= 1;
    }


    public void CancelAction()
    {
        performingAction = false;
    }
    private void OnDrawGizmos()
    {
        if (drawGizmo)
        {
            foreach(Hitbox h in currCluster.hitboxes)
            {
                Gizmos.DrawCube(transform.position + new Vector3(h.offsetFromAnchor.x * (currMoveFacingRight ? 1 : -1), h.offsetFromAnchor.y), h.dimensions);

            }
        }
    }
}
