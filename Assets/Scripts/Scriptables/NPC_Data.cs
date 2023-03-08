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
    public string NPCName;//name to show in the UI
    public role job;//can we trade with the NPC or only talk
    public int money;//how much money the NPC have
    public List<string> dialogues;//a list or predefined quotes to say
    public List<Item_Data> inventory;//items the NPC has

}
