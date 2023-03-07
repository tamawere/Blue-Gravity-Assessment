using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Item data constructor
/// </summary>

public enum cat {Hat,Clothes,Boots};
[CreateAssetMenu]
public class Item_Data : ScriptableObject
{
    public string ItemName;
    public cat category;
    public int index;
    public int cost;
    public List<Sprite> sprites;
}
