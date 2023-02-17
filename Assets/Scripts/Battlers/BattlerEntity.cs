using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerEntity : MonoBehaviour
{
    protected Stats stats;
    [SerializeField] StatBlock statBlock;

    protected List<Stat> regenStats = new List<Stat>();

    public virtual void Start()
    {
        //import stats
        stats = new Stats(statBlock.stats);

        foreach(Stat stat in stats.GetAll())
        {
            stat.CurrentValue = stat.defaultValue;
            if (stat.regenRate != 0f) regenStats.Add(stat);
        }
        //hook up events to stats
    }

    public bool TryUseMana(float amount)
    {
        if(stats.mana.CurrentValue >= amount)
        {
            stats.mana.CurrentValue -= amount;
            return true;
        }
        return false;
    }

    public void TakeDamage(float amount)
    {
        stats.hp.CurrentValue -= amount;

        if (stats.hp.CurrentValue <= 0f) Die();
    }
    public void Die()
    {
        this.enabled = false;
        //play death animation
    }

    public void Heal()
    {

    }

    //update stats
    void Update()
    {
        //only update stats with non 0 regen
        foreach(Stat stat in regenStats)
        {
            stat.Regen(Time.deltaTime);
        }
    }
}
