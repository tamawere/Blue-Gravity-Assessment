using UnityEngine;
/// <summary>
/// class of the UI Item element and its functions 
/// </summary>
public class Item : MonoBehaviour
{
    public Item_Data thisItem;//scriptable of the item
    public Inventory_Controller inventory;//inventory class
    public Trader_controller trader;//trader class
    public bool isInventory;//if true we call to the inventory, if false we call to the trader
    public void selectItem()//selects this item in the inventory or the trader
    {
        if (isInventory)
        {
            inventoryItem();
        }
        else
        {
            tradeItem();
        }
    }
    void inventoryItem()//if is a item from the users invetory
    {
        inventory.selectItem(thisItem);
        inventory.buttonSound();
    }
    void tradeItem()//if is a item from the NPCs invetory
    {
        trader.selectItem(thisItem);
        trader.buttonSound();
    }
}
