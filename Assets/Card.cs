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
            //int previousHP = hp;
            hp = Mathf.Clamp(value, 0, maxHP);
            HpChangedAction?.Invoke();
        }
    }

    public int MaxHP => maxHP;

    public int Damage
    {
        get { return damage; }
        set {
            
            damage = value;
            DamageChangedAction?.Invoke();
        }
    }
    public int Energy
    {
        get { return energy; }
        set {
            energy = value;
            EnergyChangedAction?.Invoke();
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
