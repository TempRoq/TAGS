using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharWheel : MonoBehaviour
{

    public List<GameObject> Slices;
    public Color HealthyColor;
    public Color DamagedColor;

    public bool IsBrawler, IsPainter;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in Slices)
        {
            //obj.transform.localScale = new Vector3(3, 3, 3);
            //obj.GetComponent<Image>().color = HealthyColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBrawler)
        {
            foreach(GameObject obj in Slices)
            {
                obj.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            }
            Slices[1].transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);
        }
        if (IsPainter)
        {
            foreach (GameObject obj in Slices)
            {
                obj.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            }
            Slices[3].transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);
        }
    }
}
