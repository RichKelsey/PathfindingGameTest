using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour
{
    private UnitType _unitType;
    protected Stats Stats;
    
    //constructor
    protected BaseUnit(UnitType unitType, Stats stats)
    {
        this._unitType = unitType;
        this.Stats = stats;
    }
    
    public void TakeDamage(int damage)
    {
        Stats.Health -= damage;
    }

    public abstract void Move();

    protected void SetUnitType(UnitType unitType)
    {
        this._unitType = unitType;
    }
    
    public UnitType GetUnitType()
    {
        return _unitType;
    }


}
