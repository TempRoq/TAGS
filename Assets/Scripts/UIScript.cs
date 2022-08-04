using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Image PowerMeter;
    [SerializeField] public Image HealthBarImage;
    [SerializeField] public Image[] WheelImages;

    // Start is called before the first frame update
    void Start()
    {

        StaticEvents.OnPlayerDamage.AddListener(PlayerDamage);//StaticEvents.OnPlayerDamage.Invoke();
        Invoke("PlayerDamage", Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
        PowerMeter.fillAmount = TimeSlowdown.instance.currSlowdownMeter / TimeSlowdown.instance.maxSlowdownMeter;
    }

    private void PlayerDamage()
    {
        HealthBarImage.fillAmount = ((float)CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].GetComponent<Character>().currentHealth)
                            / ((float)CharSwitchManager.instance.MainCharacterReferences[(int)CharSwitchManager.instance.charInPlay].GetComponent<Character>().maxHealth);



        for (int i = 0; i < WheelImages.Length; i++)
        {
            if (i == 1)//Remove this when all 4 characters exist
            {
                i++;
            }
            if (i == (int)CharSwitchManager.instance.charInPlay)
            {
                WheelImages[i].transform.localScale = new Vector3(2, 2, 1);
            }
            else
            {
                WheelImages[i].transform.localScale = new Vector3(1.6f, 1.6f, 1);
            }
            float hp = ((float)CharSwitchManager.instance.MainCharacterReferences[i].GetComponent<Character>().currentHealth)
                            / ((float)CharSwitchManager.instance.MainCharacterReferences[i].GetComponent<Character>().maxHealth);

            WheelImages[i].color = new Vector4(2 - 2 * hp, 2 - 2 * (1 - hp), 0, 1);

        }
    }



}
