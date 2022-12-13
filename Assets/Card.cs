using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    int hp, damage, energy;
    int maxHP;
    public int HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public int MaxHP => maxHP;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int Energy
    {
        get { return energy; }
        set { energy = value; }
    }


    public Card(int _hp, int _damage, int _energy)
    {
        hp = _hp;
        damage = _damage;
        energy = _energy;
    }


}
