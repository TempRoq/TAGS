using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StaticEvents_Training : MonoBehaviour
{

    public static UnityEvent EnemiesMoveSwitch;
    public static UnityEvent EnemiesAttackSwitch;
    public static UnityEvent EnemiesNumberChange;
    public static UnityEvent SpawnBreakable;
    public static UnityEvent PlayerFillMeter;
    public static UnityEvent SceneChange;
    private void Awake()
    {

    }

    private void Start()
    {

        if (EnemiesMoveSwitch == null)
        {
            EnemiesMoveSwitch = new UnityEvent();
        }
        if (EnemiesAttackSwitch == null)
        {
            EnemiesAttackSwitch = new UnityEvent();
        }
        if (EnemiesNumberChange == null)
        {
            EnemiesNumberChange = new UnityEvent();
        }
        if (SpawnBreakable == null)
        {
            SpawnBreakable = new UnityEvent();
        }
        if (PlayerFillMeter == null)
        {
            PlayerFillMeter = new UnityEvent();
        }
        if (SceneChange == null)
        {
            SceneChange = new UnityEvent();
        }

    }

   
}
