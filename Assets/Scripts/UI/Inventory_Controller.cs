using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(AudioSource))]
public class Inventory_Controller : MonoBehaviour
{
    public static bool UIisOpen=false;//if true all the inputs gets disabled while the UI is on screen

    [SerializeField] private Character_Controller player;//scriptable object with the inventory data
    [SerializeField] private Image previewImage;//UI image
    [SerializeField] private TMP_Text textMoney, textName,textCost;//UI info texts
    [SerializeField] private GameObject panelHats, panelClothes, panelBoots;//UI item panels
    [SerializeField] private GameObject itemPrefab;//prefab of item panel
    [SerializeField] private Button wearButton;//prefab of item panel

    public AudioClip openSound, selectSound, wearSound;//sound effects
    public Sprite invisibleImage;//to show when there's no items
    [HideInInspector] public Item_Data selectedItem;//item that the player has selected
    [HideInInspector] public Player_Data inventory;//scriptable object with the inventory data

    private Panel_animation myAnimation;//the component with the animated entrance
    private AudioSource audiosource;//audiosource
    private bool isOpen = false;//if the inventory window is open

    // Start is called before the first frame update
    void Start()
    {
        myAnimation = GetComponent<Panel_animation>();//locates the animation
        inventory = player.inventory;//locates the players inventory
        audiosource = GetComponent<AudioSource>();//locates the AudioSource
    }

    public void initInventory()//initializes the inventory
    {
        textMoney.text = "$" + inventory.money.ToString();//sets the money text
        if (wearButton != null)
            wearButton.interactable=true;//enables the wear button

        //finds the first item we are wearing that is not null and selects it
        if (inventory.wearingHat!=null)
            selectItem(inventory.wearingHat);
        else if (inventory.wearingClothes != null)
        {
            selectItem(inventory.wearingClothes);
        }
        else if (inventory.wearingBoots != null)
        {
            selectItem(inventory.wearingBoots);
        }
        else if(inventory.hatInventory.Count>0)
        {
            selectItem(inventory.hatInventory[0]);
        }
        else if (inventory.clothesInventory.Count > 0)
        {
            selectItem(inventory.clothesInventory[0]);
        }
        else if (inventory.bootsInventory.Count > 0)
        {
            selectItem(inventory.bootsInventory[0]);
        }
        else //if it couldnt found any item 
        {
            if(wearButton!=null)
                wearButton.interactable=false;//disables wear button
            previewImage.sprite = invisibleImage;//hides image
        }

        //clears all the items from item containers
        foreach (Transform child in panelHats.transform) {
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

        fillItemContainer();//fills the containers with the players inventory

    }

    void fillItemContainer()//fills the containers with the players inventory
    {
        //fills the hat container with the players inventory
        if (inventory.wearingHat != null)
        {
            GameObject item = Instantiate(itemPrefab) as GameObject;//instantiate new item
            item.transform.SetParent(panelHats.transform);//set the container as a parent
            item.transform.GetChild(0).GetComponent<Image>().sprite = inventory.wearingHat.sprites[1];//actualizes the image
            item.GetComponent<Item>().thisItem = inventory.wearingHat;//sets the item class info
            item.GetComponent<Item>().inventory=gameObject.GetComponent<Inventory_Controller>();//pass itself as a parameter so the new item can call its functions
            item.GetComponent<Item>().isInventory = true;///sets the item as a inventory item
            item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);//adjust the scale to 1
        }

        if (inventory.hatInventory != null)//if theres at least one hat
        {
            foreach(Item_Data myItem in inventory.hatInventory)//finds every item of its category and instantiates it in the containers
            {
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.transform.SetParent(panelHats.transform);
                item.transform.GetChild(0).GetComponent<Image>().sprite = myItem.sprites[1];
                item.GetComponent<Item>().thisItem = myItem;
                item.GetComponent<Item>().inventory = gameObject.GetComponent<Inventory_Controller>();
                item.GetComponent<Item>().isInventory = true;
                item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
           
        }
        //fills the clothes container with the players inventory
        if (inventory.wearingClothes != null)
        {
            GameObject item = Instantiate(itemPrefab) as GameObject;
            item.transform.SetParent(panelClothes.transform);
            item.transform.GetChild(0).GetComponent<Image>().sprite = inventory.wearingClothes.sprites[1];
            item.GetComponent<Item>().thisItem = inventory.wearingClothes;
            item.GetComponent<Item>().inventory = gameObject.GetComponent<Inventory_Controller>();
            item.GetComponent<Item>().isInventory = true;
            item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        if (inventory.clothesInventory != null)
        {
            foreach (Item_Data myItem in inventory.clothesInventory)//finds every item of its category and instantiates it in the containers
            {
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.transform.SetParent(panelClothes.transform);
                item.transform.GetChild(0).GetComponent<Image>().sprite = myItem.sprites[1];
                item.GetComponent<Item>().thisItem = myItem;
                item.GetComponent<Item>().inventory = gameObject.GetComponent<Inventory_Controller>();
                item.GetComponent<Item>().isInventory = true;
                item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }

        }
        //fills the boots container with the players inventory
        if (inventory.wearingBoots != null)
        {
            GameObject item = Instantiate(itemPrefab) as GameObject;
            item.transform.SetParent(panelBoots.transform);
            item.transform.GetChild(0).GetComponent<Image>().sprite = inventory.wearingBoots.sprites[1];
            item.GetComponent<Item>().thisItem = inventory.wearingBoots;
            item.GetComponent<Item>().inventory = gameObject.GetComponent<Inventory_Controller>();
            item.GetComponent<Item>().isInventory = true;
            item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        if (inventory.bootsInventory != null)
        {
            foreach (Item_Data myItem in inventory.bootsInventory)
            {
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.transform.SetParent(panelBoots.transform);
                item.transform.GetChild(0).GetComponent<Image>().sprite = myItem.sprites[1];
                item.GetComponent<Item>().thisItem = myItem;
                item.GetComponent<Item>().inventory = gameObject.GetComponent<Inventory_Controller>();
                item.GetComponent<Item>().isInventory = true;
                item.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }

        }

    }

    public void buttonSound()//click sound triggered from the click event
    {
        audiosource.clip = selectSound;
        audiosource.Play();
    }

    public void selectItem(Item_Data item)//selects the item we are going to trade/wear
    {
        
        selectedItem = item;
        previewImage.sprite = selectedItem.sprites[1];//actializes the image
        textName.text = selectedItem.ItemName;//sets the item name text
        textCost.text ="Cost: $" + selectedItem.cost.ToString();//sets the item cost text
    }
    public void wearItem()//so the button can call the weat function
    {
        wearItem(selectedItem);
    }
    void wearItem(Item_Data item)//wears the selected item
    {
        audiosource.clip = wearSound;
        audiosource.Play();
        //finds the hat item in the inventory list and moves it to the wearing object
        if (item.category == cat.Hat)
        {
            for (int i = 0; i < inventory.hatInventory.Count; i++)
            {
                if (item.Equals(inventory.hatInventory[i]))
                {
                    if(inventory.wearingHat!=null)
                        inventory.hatInventory.Add(inventory.wearingHat);
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
                    if (inventory.wearingClothes != null)
                        inventory.clothesInventory.Add(inventory.wearingClothes);
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
                    if (inventory.wearingBoots != null)
                        inventory.bootsInventory.Add(inventory.wearingBoots);
                    inventory.wearingBoots = inventory.bootsInventory[i];
                    inventory.bootsInventory.RemoveAt(i);
                }
            }
        }
    }
    public void OpenInventory()//opens the inventory window
    {
        isOpen = !isOpen;
        if (isOpen)//open the inventory window
        {
            UIisOpen = true;
            initInventory();//initializes the inventory
        }
        else//closes the inventory window
        {
            StartCoroutine(waitfoAnimation());
        }
        myAnimation.ToggleMenu();
        audiosource.clip = openSound;
        audiosource.Play();
    }
    IEnumerator waitfoAnimation()//waits for the panel to leave the screen to enable the inputs
    {
        yield return new WaitForSeconds(0.7f);
        UIisOpen = false;
    }
}
