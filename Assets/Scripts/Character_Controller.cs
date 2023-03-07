using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this class controlls the player movement and animations
/// </summary>
 

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Character_Controller : MonoBehaviour
{
    
    [Header("Parameters")]
    public float speed = 5f;//movement speed parameter

    [Header("Components")]
    public Player_Data inventory;//scriptable object with the inventory data
    public SpriteRenderer hatSprite;//object to change sprite
    public SpriteRenderer clothesSprite;//object to change sprite
    public SpriteRenderer LeftBootSprite;//object to change sprite
    public SpriteRenderer rightBootSprite;//object to change sprite

    private Rigidbody2D rb;//control movement
    private Animator animator;//to control animations
    private Vector2 movement;//to store the horizontal and vertical inputs
    private AudioSource audiosource;//players audiosource
    //to check if the audio is playng
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();//locates the rigidbody
        animator = GetComponent<Animator>();//locates the Animator
        audiosource = GetComponent<AudioSource>();//locates the AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        //reads the input values and stores them
        movement.x= Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //sets the animator parameters to trigger the animations
        animator.SetFloat("Horizontal",movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.magnitude);
        //plays the audio if needed
        playAudio();
    }

    void FixedUpdate()
    {
        //moves the player 
        rb.MovePosition(rb.position+movement*speed*Time.fixedDeltaTime);
    }

    //plays the step sound when the player is walking
    void playAudio()
    {
        // stops the audio when the player is not moving
        if (audiosource.isPlaying)
        {
            if (movement.magnitude < 0.01f)
            {
                audiosource.Stop();
            }
                
        }
        // plays the audio when the player has started to move
        else if (movement.magnitude > 0.01f)
        {
            audiosource.Play();
        }
    }

    //receives a item and wears it
    void wearItem(Item_Data item)
    {
        //finds the hat item in the inventory list and moves it to the wearing object
        if (item.category==cat.Hat)
        {
            for(int i = 0; i < inventory.hatInventory.Count; i++)
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

    //gets what side is the player looking at and sets the appropiate sprite (its triggered from the animation clips)
    public void setSideFace(int side)//side: 0=back, 1=front,2=left or righ
    {
        //changes the clothes sprite to the side the player is looking at
        if (inventory.wearingHat != null) hatSprite.sprite = inventory.wearingHat.sprites[side];//hat
        else hatSprite.sprite = null;

        if (inventory.wearingClothes != null) clothesSprite.sprite = inventory.wearingClothes.sprites[side];//clothes
        else clothesSprite.sprite = null;

        if (inventory.wearingBoots != null)//boots
        {
            LeftBootSprite.sprite = inventory.wearingBoots.sprites[side];
            rightBootSprite.sprite = inventory.wearingBoots.sprites[side + 3];
        }
        else
        {
            LeftBootSprite.sprite = null;
            rightBootSprite.sprite = null;
        }
    }
}
