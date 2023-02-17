using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : BattlerEntity
{
    [SerializeField] Image hpFill, manaFill;
    [SerializeField] TextMeshProUGUI hpText, manaText;
    public override void Start()
    {
        base.Start();

        //hook player UI

        stats.hp.OnStatChanged += UpdateHealthUI;
        stats.mana.OnStatChanged += UpdateManaUI;
    }

    public void UpdateHealthUI()
    {
        hpFill.fillAmount = stats.hp.StatPercentage();
        hpText.text = stats.hp.CurrentValue.ToString("0");
    }

    public void UpdateManaUI()
    {
        manaFill.fillAmount = stats.mana.StatPercentage();
        manaText.text = stats.mana.CurrentValue.ToString("0");
    }
}
