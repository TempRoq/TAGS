using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHandler : MonoBehaviour
{
    // Start is called before the first frame update
  
    public GameObject shadow;
    public LayerMask LayerToHit;
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rc, 20f, LayerToHit))
        {
            shadow.SetActive(true);
            shadow.transform.position = rc.point + new Vector3(0, .01f, 0);
        }
        else
        {
            shadow.SetActive(false);
        }

    }

    public Vector3 GetShadowPosition()
    {
        return shadow.transform.position;
    }
}
