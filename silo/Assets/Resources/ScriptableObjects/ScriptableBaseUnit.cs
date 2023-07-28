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

}

public enum UnitType
    {
        Player,
        Enemy,
        Boss,
        Npc,
        Other
    }