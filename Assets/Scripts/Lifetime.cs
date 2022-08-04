using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    float time;
    public float lifespan;
    void Start()
    {
        time = Time.time + lifespan;
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= Time.time)
        {
            Destroy(gameObject);
        }
    }

    public void SetNewLifespan(float f)
    {
        lifespan = f;
        time = Time.time + lifespan;
    }
}
