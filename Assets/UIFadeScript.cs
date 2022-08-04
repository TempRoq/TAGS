using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeScript : MonoBehaviour
{

    public CanvasGroup uiStuff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 6)
        {
            uiStuff.alpha = .35f;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 6)
        {
            uiStuff.alpha = 1f;
        }
    }
}
