using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Image HPBarCurrent;

    public void UpdateHP(int maxHP, int currentHP, int damage)
    {
        currentHP -= damage;
        HPBarCurrent.fillAmount = currentHP / maxHP;
    }
}
