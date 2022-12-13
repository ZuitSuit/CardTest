using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card
{
    int hp, damage, energy;
    int maxHP;

    public UnityAction HpChangedAction;
    public UnityAction DamageChangedAction;
    public UnityAction EnergyChangedAction;

    public int HP
    {
        get { return hp; }
        set { 
            hp = value;
            HpChangedAction?.Invoke();
        }
    }

    public int MaxHP => maxHP;

    public int Damage
    {
        get { return damage; }
        set {
            DamageChangedAction?.Invoke();
            damage = value;
        }
    }
    public int Energy
    {
        get { return energy; }
        set {
            EnergyChangedAction?.Invoke();
            energy = value;
        }
    }

    public Card(int _hp, int _damage, int _energy)
    {
        hp = _hp;
        damage = _damage;
        energy = _energy;

        maxHP = hp;
    }


}
