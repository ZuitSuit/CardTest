using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public Stat hp;// = new Stat(100f, 0f, 100f, 0f);
    public Stat mana;// = new Stat(0f, 0f, 100f, 1f);
    public Stat speed;// = new Stat(1f, 0f, 5f, 1f);

    public List<Stat> GetAll()
    {
        return new List<Stat>(){hp, mana, speed};
    }

    public Stats()
    {
        hp = new Stat(100f, 0f, 100f, 0f);
        mana = new Stat(0f, 0f, 100f, 1f);
        speed = new Stat(1f, 0f, 5f, 1f);
    }

    public Stats(Stats otherStats)
    {
        List<Stat> otherList = new List<Stat>(otherStats.GetAll());

        //TODO redo this so it scales?
        hp = otherList[0];
        mana = otherList[1];
        speed = otherList[2];
    }
}
