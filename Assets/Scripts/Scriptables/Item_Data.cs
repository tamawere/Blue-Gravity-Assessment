using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Item data constructor
/// </summary>

public enum cat {Hat,Clothes,Boots};
[CreateAssetMenu]
public class Item_Data : ScriptableObject
{
    public string ItemName;//name to show in the UI
    public cat category;//what category the item belongs to
    public int index;//index of the probably delete
    public int cost;//how much does it cost
    public List<Sprite> sprites;//sprites of the different perspectives
}
