using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : BattlerEntity
{
    [SerializeField] Image hpFill, manaFill;
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
    }

    public void UpdateManaUI()
    {
        manaFill.fillAmount = stats.mana.StatPercentage();
    }
}
