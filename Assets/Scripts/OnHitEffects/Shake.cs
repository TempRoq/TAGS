using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public GameObject rotateable;
    private bool shaking = false;
    private bool inAni = false;

    public void shake()
    {
        StartCoroutine("shook");
    }


    IEnumerator shook()
    {
        if (!inAni)
        {
            inAni = true;
            Quaternion q = rotateable.transform.rotation;
            int mult = 1;
            for (float ft = 0.3f; ft >= 0; ft -= 0.1f)
            {
                if (shaking)
                {
                    shaking = false;
                    ft = 0.3f;
                }
                var rot = rotateable.transform.rotation;
                rot.w = q.w * 1f * mult;
                transform.rotation = rot;
                mult = mult * -1;
                yield return new WaitForSeconds(.1f);
            }
            rotateable.transform.rotation = q;
            shaking = false;
            inAni = false;
        }
        else
        {
            shaking = true;
        }
    }
}
