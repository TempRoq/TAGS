using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMeterOnHit : MonoBehaviour
{
    public Character me;
    public ComboMeter comboMeter;
    public void comboOnHit()
    {
        comboMeter.addCombo(1);
    }

}
