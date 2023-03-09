
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Controlls the trade UI and functions
/// </summary>
public class Trader_controller : MonoBehaviour
{
    [SerializeField] private Inventory_Controller inventorytrade;//scriptable object with the inventory data
    [SerializeField] private Dialogue_Controller dialogs;//npcs dialogs controller
    [SerializeField] private Image previewImage;//UI image
    [SerializeField] private TMP_Text textNpcName, textMoney, textName,textCost;//UI info texts
    [SerializeField] private GameObject panelHats, panelClothes, panelBoots;//UI item panels
    [SerializeField] private GameObject itemPrefab;//prefab of item panel
    [SerializeField] private Button buyButton, sellButton;//UI buttons to sell and buy

    public AudioClip openSound, selectSound, tradeSound;//UI Sound effects
    public GameObject modal;//modal to show messages
    public TMP_Text modalText;//message of the modal

    private Panel_animation myAnimation;//the component with the animated entrance
    private NPC_Data inventory;//scriptable object with the inventory data
    private Item_Data selectedItem;//item that the player has selected
    private AudioSource audiosource;//audiosource
    // Start is called before the first frame update
    void Start()
    {
        myAnimation = transform.parent.GetComponent<Panel_animation>();//locates the animation
        audiosource = GetComponent<AudioSource>();//locates the AudioSource
        buyButton.onClick.AddListener(buy);//initializes buy button listener
        sellButton.onClick.AddListener(sell);//initializes sell button listener
    }

    void initTrade()
    {
        selectedItem = null;//clears the selected item in case it has trash
        inventory = dialogs.npc;//gets the NPC inventory throught the dialog component
        textNpcName.text = inventory.NPCName;//initializes UI text
        textMoney.text = "$" + inventory.money.ToString();//sets the money text

        //clears all the items from item containers
        foreach (Transform child in panelHats.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in panelClothes.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in panelBoots.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (inventory.inventory.Count>0)//if the NPC has items we fill the containers
        {
            selectItem(inventory.inventory[0]);//selects the first item of the list
            fillItemContainer();//fills the containers with the item buttons
            buyButton.interactable = true;//enables buy button
        }
        else //if the NPC has not items to sell we clear all the UI
        {
            buyButton.interactable = false;//disables the buy button
            textName.text = "";
            textCost.text = "";
            previewImage.sprite =inventorytrade.invisibleImage;//puts an invisible image in the preview
        }
    }

    void fillItemContainer()//fills the containers with the players inventory
    {
        if (inventory.inventory != null)//if the NPC has items we instantiate them in the containers
        {
            foreach (Item_Data myItem in inventory.inventory)
            {
                GameObject item = Instantiate(itemPrefab) as GameObject;//instantiates the item prefab
                item.transform.GetChild(0).GetComponent<Image>().sprite = myItem.sprites[1];//sets the item image
                item.GetComponent<Item>().thisItem = myItem;//sets the item scriptable
                item.GetComponent<Item>().trader = gameObject.GetComponent<Trader_controller>();//thells the item this class is the inventory

                //sets the parent according to the items category
                if (myItem.category.Equals(cat.Hat))
                {
                    item.transform.SetParent(panelHats.transform);
                }
                else if (myItem.category.Equals(cat.Clothes))
                {
                    item.transform.SetParent(panelClothes.transform);
                }
                if (myItem.category.Equals(cat.Boots))
                {
                    item.transform.SetParent(panelBoots.transform);
                }
                item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//adjust the scale so it dont gets distorted
            }
        }

    }

    public void buttonSound()//gets called from the buttons click events to make a "click" sound
    {
        audiosource.clip = selectSound;
        audiosource.Play();
    }

    public void selectItem(Item_Data item)//selects the item to trade
    {
        
        selectedItem = item;//sets the item that will be traded if we press buy
        previewImage.sprite = selectedItem.sprites[1];//actualizes the image
        textName.text = selectedItem.ItemName;//sets the item name text
        textCost.text = "Cost: $"+selectedItem.cost.ToString();//shows the item cost in the UI
    }
  
    
    public void openTrade()//displays the trading panel an initializes it
    {
        Inventory_Controller.UIisOpen = true;//if this variable is true al the player inputs besides the UI wont work
        initTrade();//initializes the NPC inventory trading panel
        inventorytrade.initInventory();//initializes the player inventory panel
        myAnimation.ToggleMenu();//plays the entrance animation
        audiosource.clip = openSound;//makes a folding pergamin sound
        audiosource.Play();//plays the audio
    }
    public void closeTrade()//closes the trading window
    {
        StartCoroutine(waitfoAnimation());//it waits for the panel to leave the window to enable the player inputs
        myAnimation.ToggleMenu();//plays the exit animation
        audiosource.clip = openSound;//makes a folding pergamin sound
        audiosource.Play();//plays the audio

    }
    public void sell()//function to sell from the player to the NPC
    {
        //checks if the player is not wearing the item is trying to sell
        if (inventorytrade.selectedItem.Equals(inventorytrade.inventory.wearingHat) || inventorytrade.selectedItem.Equals(inventorytrade.inventory.wearingClothes) ||
            inventorytrade.selectedItem.Equals(inventorytrade.inventory.wearingBoots)
           )
        {
            audiosource.clip = openSound;
            audiosource.Play();
            modalText.text="You can't sell something you are wearing";//opens a modal with the message
            modal.SetActive(true);
        }
        else//if we are not wearing the item we can sell it
        {
            if (inventory.money >= inventorytrade.selectedItem.cost)//checks  if we have enought money
            {
                
                inventory.money -= inventorytrade.selectedItem.cost;//takes the item cost from our inventory
                inventorytrade.inventory.money += inventorytrade.selectedItem.cost;//ads the cost to the NPCs inventory

                //checks what category the item belongs to and removes from the user inventory and ads it to the NPC inventory
                if (inventorytrade.selectedItem.category.Equals(cat.Hat))
                {
                    for(int i=0; i<inventorytrade.inventory.hatInventory.Count;i++)
                    {
                        if (inventorytrade.inventory.hatInventory[i].Equals(inventorytrade.selectedItem))
                        {
                            inventory.inventory.Add(inventorytrade.inventory.hatInventory[i]);
                            inventorytrade.inventory.hatInventory.RemoveAt(i);
                        }
                    }
                }
                if (inventorytrade.selectedItem.category.Equals(cat.Clothes))
                {
                    for (int i = 0; i < inventorytrade.inventory.clothesInventory.Count; i++)
                    {
                        if (inventorytrade.inventory.clothesInventory[i].Equals(inventorytrade.selectedItem))
                        {
                            inventory.inventory.Add(inventorytrade.inventory.clothesInventory[i]);
                            inventorytrade.inventory.clothesInventory.RemoveAt(i);
                        }
                    }
                }
                if (inventorytrade.selectedItem.category.Equals(cat.Boots))
                {
                    for (int i = 0; i < inventorytrade.inventory.bootsInventory.Count; i++)
                    {
                        if (inventorytrade.inventory.bootsInventory[i].Equals(inventorytrade.selectedItem))
                        {
                            inventory.inventory.Add(inventorytrade.inventory.bootsInventory[i]);
                            inventorytrade.inventory.bootsInventory.RemoveAt(i);
                        }
                    }
                }


                initTrade();//resets the trade so the changes got actualized in the screen
                inventorytrade.initInventory();//resets the inventory so the changes got actualized in the screen
                audiosource.clip = tradeSound;
                audiosource.Play();
            }
            else//if we have not enough money shows a message
            {
                audiosource.clip = openSound;
                audiosource.Play();
                modalText.text = inventory.NPCName + " does not have enought money";
                modal.SetActive(true);
            }
            
        }
    }
    public void buy()//function where the player buys from the NPC
    {
        if (selectedItem != null)//if we have items
        {
            if (inventorytrade.inventory.money >= selectedItem.cost)//if we have enought money
            {

                inventorytrade.inventory.money -= selectedItem.cost;//substracts the item cost from the players inventory
                inventory.money += selectedItem.cost;//ads the cost to the NPCs inventory

                for(int i=0;i<inventory.inventory.Count; i++)//finds the item in the NPCs inventory
                {
                    if (selectedItem.Equals(inventory.inventory[i]))//when we found the item checks the category and adds it the the player correspondent category
                    {
                        if (selectedItem.category.Equals(cat.Hat))
                        {
                            inventorytrade.inventory.hatInventory.Add(selectedItem);
                        }
                        else if (selectedItem.category.Equals(cat.Clothes))
                        {
                            inventorytrade.inventory.clothesInventory.Add(selectedItem);
                        }
                        else if (selectedItem.category.Equals(cat.Boots))
                        {
                            inventorytrade.inventory.bootsInventory.Add(selectedItem);
                        }
                        inventory.inventory.RemoveAt(i);//removes the item from the players inventory
                    }
                }
                initTrade();//resets the trade panel so the canges can be shown
                inventorytrade.initInventory();//resets the inventory panel so the canges can be shown
                audiosource.clip = tradeSound;
                audiosource.Play();
            }
            else//if we have not enought money it shows a message
            {
                audiosource.clip = openSound;
                audiosource.Play();
                modalText.text = "You don't have enought money!";
                modal.SetActive(true);
            }

        }
        
    }
    IEnumerator waitfoAnimation() //waits for the exit animation to end to enable the player inputs
    {
        yield return new WaitForSeconds(0.8f);
        Inventory_Controller.UIisOpen = false;
    }
}