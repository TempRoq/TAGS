using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASTrigger : MonoBehaviour
{
    bool hitThisFrame;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitThisFrame = false;
    }

    public void GetHit()
    {
        hitThisFrame = true;
    }

    public bool JustHit()
    {
        return hitThisFrame;
    }

}
