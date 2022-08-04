using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlowdown : Singleton<TimeSlowdown>
{
    public bool infiniteMeter;
    public float timeStopDelay;


    public float currSlowdownMeter;
    public float maxSlowdownMeter;

   

    public bool timeFrozen;

    public bool canFreezeTime;

    public GameObject Vignette;
    Animator VignetteAnim;

    public GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {

        //VignetteAnim = Vignette.GetComponent<Animator>();

        StaticEvents.OnTimeStop.AddListener(TimeFreeze);
        StaticEvents.OnTimeResume.AddListener(RevertTimeInvoked);

        

    }

    // Update is called once per frame
    void Update()
    {
        

       // Canvas.GetComponent<UIScript>().PowerMeter.fillAmount = currSlowdownMeter / 100;

        if (Input.GetKeyDown(KeyCode.Tab))
        {        
            if (timeFrozen)
            {
                TryNormalSpeed(out bool b);
                print("Returned to normal speed: " + b);


                //VignetteAnim.SetTrigger("End");
            }
            else
            {
                TryFreezeTime(out bool b);
                print("Slowed down: " + b);


               // VignetteAnim.SetTrigger("Start");
            }
        }
    }

    public void TimeFreeze()
    {
        StartCoroutine(FreezeTime());

        //VignetteAnim.SetTrigger("Start");
    }


    public void TimeResume()
    {
        RevertTimeInvoked();
    }

    public IEnumerator FreezeTime()
    {
        print("ZA WARUDO");
        timeFrozen = true;
        yield return new WaitForSecondsRealtime(.26f);
        //Spawn thing on controllable character

    }

    public void RevertTimeInvoked()
    {
        timeFrozen = false;
        print("Time Resumed!");
        //VignetteAnim.SetTrigger("End");

    }


    public  void TryFreezeTime(out bool successful)
    {
        if (!timeFrozen && currSlowdownMeter == maxSlowdownMeter)
        {
            successful = true;
            StaticEvents.OnTimeStop.Invoke();


        }

        successful = false;
    }


    public static void TryNormalSpeed(out bool successful)
    {

            successful = true;
            StaticEvents.OnTimeResume.Invoke();

    }

    public  void GiveMeter(float meter)
    {
        currSlowdownMeter = Mathf.Clamp(currSlowdownMeter + meter, 0, maxSlowdownMeter);
    }
}
