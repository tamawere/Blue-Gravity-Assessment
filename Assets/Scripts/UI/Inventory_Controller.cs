using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory_Controller : MonoBehaviour
{
    [SerializeField] private Player_Data inventory;//scriptable object with the inventory data

    [SerializeField] private Image previewImg;
    [SerializeField] private TMP_Text txtName, txtCost;

    private Panel_animation myAnimation;//the component with the animated entrance

    private Item_Data selectedItem;
    // Start is called before the first frame update
    void Start()
    {
        myAnimation = GetComponent<Panel_animation>();//locates the animation
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initInventory()
    {
        //previewImg.sprite = inventory.sprites[1];
        //txtName.text = item.ItemName;
        //txtCost.text = "$" + item.cost.ToString();
    }

    public void selectItem(Item_Data item)
    {
        previewImg.sprite = item.sprites[1];
        txtName.text = item.ItemName;
        txtCost.text = "$"+item.cost.ToString();
        selectedItem = item;
    }
    public void wearItem()
    {
        wearItem(selectedItem);
    }
    void wearItem(Item_Data item)
    {
        //finds the hat item in the inventory list and moves it to the wearing object
        if (item.category == cat.Hat)
        {
            for (int i = 0; i < inventory.hatInventory.Count; i++)
            {
                if (item.Equals(inventory.hatInventory[i]))
                {
                    inventory.wearingHat = inventory.hatInventory[i];
                    inventory.hatInventory.RemoveAt(i);
                }
            }
        }
        //finds the item in the inventory list and moves it to the wearing object
        else if (item.category == cat.Clothes)
        {
            for (int i = 0; i < inventory.clothesInventory.Count; i++)
            {
                if (item.Equals(inventory.clothesInventory[i]))
                {
                    inventory.wearingClothes = inventory.clothesInventory[i];
                    inventory.clothesInventory.RemoveAt(i);
                }
            }
        }
        //finds the item in the inventory list and moves it to the wearing object
        else if (item.category == cat.Boots)
        {
            for (int i = 0; i < inventory.bootsInventory.Count; i++)
            {
                if (item.Equals(inventory.bootsInventory[i]))
                {
                    inventory.wearingBoots = inventory.bootsInventory[i];
                    inventory.bootsInventory.RemoveAt(i);
                }
            }
        }
    }
    public void OpenInventory()
    {
        initInventory();
        myAnimation.ToggleMenu();
    }
}
