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

    protected void SetStats(int health, float speed, int attackModifier, int walkAcceleration, int walkDeceleration, int dashPower, float dashCooldown, float attackCooldown)
    {
        Stats.Health = health;
        Stats.Speed = speed;
        Stats.AttackModifier = attackModifier;
        Stats.WalkAcceleration = walkAcceleration;
        Stats.WalkDeceleration = walkDeceleration;
        Stats.DashPower = dashPower;
        Stats.DashCooldown = dashCooldown;
        Stats.AttackCooldown = attackCooldown;
    }

    public abstract void TakeDamage(int damage);


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
