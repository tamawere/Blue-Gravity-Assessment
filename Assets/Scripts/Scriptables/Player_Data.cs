using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Player data constructor
/// </summary>
[CreateAssetMenu]
public class Player_Data : ScriptableObject
{
    public int money;//the money the player has
    public Item_Data wearingHat;//the hat the player is wearing
    public Item_Data wearingClothes;//the clothes the player is wearing
    public Item_Data wearingBoots;//the boots the player is wearing

    public List<Item_Data> hatInventory;//hats the player owns
    public List<Item_Data> clothesInventory; //clothes the player owns
    public List<Item_Data> bootsInventory; //boots the player owns
}
