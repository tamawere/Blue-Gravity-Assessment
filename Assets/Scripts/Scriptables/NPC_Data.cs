using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// NPC data constructor
/// </summary>
/// 
public enum role { Villager, Trader};
[CreateAssetMenu]
public class NPC_Data : ScriptableObject
{
    public string NPCName;
    public role job;
    public int money;
    public List<string> dialogues;
    public List<Item_Data> inventory;

}
