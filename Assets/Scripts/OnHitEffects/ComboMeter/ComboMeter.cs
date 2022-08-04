using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboMeter : MonoBehaviour
{
    public float comboNum = 0;
    public Text num;
    bool Combo = false;
    float comboTime = 3f;
    float comboTimeIn = 0;
    public void addCombo(int plus)
    {
        if (!Combo) {
            Combo = true;
            comboNum = plus;
            comboTimeIn = Time.time + comboTime;
        } else if(comboTimeIn > Time.time)
        {
            comboTimeIn = Time.time + comboTime;
            comboNum += plus;
        } else
        {
            Combo = false;
            addCombo(plus);
        }
        num.text = comboNum.ToString();
    }
}
