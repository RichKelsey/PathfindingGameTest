using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableBaseUnit : ScriptableObject
{
    
    public UnitType unitType;
    public Stats Stats;
    
}

public struct Stats
{
    
    public int Health;
    public float Speed;
    public int AttackModifier;
    public int WalkAcceleration;
    public int WalkDeceleration;
    public int DashPower;
    public float DashCooldown;
    public float AttackCooldown;

}

public enum UnitType
    {
        Player,
        Enemy,
        Boss,
        Npc,
        Other
    }