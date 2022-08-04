using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaticEvents : Singleton<StaticEvents>
{

    public static bool valuesSet = false;
    public static UnityEvent SprayPaintSnap;
    public static UnityEvent OnTimeStop;
    public static UnityEvent OnTimeResume;
    public static UnityEvent OnEnemyDeath;
    public static UnityEvent OnPlayerDamage;


    public static float paintInvokeDelay = .5f;
    public GameObject hitEffect;
    public static bool currentlyInvoking = false;
    public static List<PaintCloud> allPaint;

    protected override void Awake()
    {
        base.Awake();
    }

    public enum PaintColor
    {
        RED = 0,
        BLUE = 1
    }

    private void Start()
    {
        allPaint = new List<PaintCloud>();
        if (SprayPaintSnap == null)
        {
            SprayPaintSnap = new UnityEvent();
            SprayPaintSnap.AddListener(BeginClearPaint);
        }

        if (OnTimeStop == null)
        {
            OnTimeStop = new UnityEvent();
        }

        if (OnTimeResume == null)
        {
            OnTimeResume = new UnityEvent();
        }

        if (OnEnemyDeath == null)
        {
            OnEnemyDeath = new UnityEvent();
        }

        if (OnPlayerDamage == null)
        {
            OnPlayerDamage = new UnityEvent();
        }

    }

    public void SetAllValues()
    {
        allPaint = new List<PaintCloud>();
        if (SprayPaintSnap == null)
        {
            SprayPaintSnap = new UnityEvent();
            SprayPaintSnap.AddListener(BeginClearPaint);
        }

        if (OnTimeStop == null)
        {
            OnTimeStop = new UnityEvent();
        }

        if (OnTimeResume == null)
        {
            OnTimeResume = new UnityEvent();
        }

        if (OnEnemyDeath == null)
        {
            OnEnemyDeath = new UnityEvent();
        }

        if (OnPlayerDamage == null)
        {
            OnPlayerDamage = new UnityEvent();
        }
        valuesSet = true;

    }

    public void BeginClearPaint()
    {
        StartCoroutine(ClearPaint());
    }

    public IEnumerator ClearPaint()
    {
        PaintCloud[] pClouds = new PaintCloud[allPaint.Count];
        for (int i = 0; i < allPaint.Count; i++)
        {
            pClouds[i] = allPaint[i];
        }
        int a = allPaint.Count;
        allPaint.Clear();
        
        for (int i = 0; i < a; i++)
        {
            pClouds[i].Detonate();
            if (i != a - 1)
            {
                yield return new WaitForSeconds(paintInvokeDelay);
            }
        }


        currentlyInvoking = false;
    }

    public static void UnsetValues()
    {
        valuesSet = false;
    }

   
}
