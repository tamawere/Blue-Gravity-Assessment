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
    [Space(10)]
    [SerializeField] private SpriteRenderer hatSprite;//object to change sprite
    [SerializeField] private SpriteRenderer clothesSprite;//object to change sprite
    [SerializeField] private SpriteRenderer LeftBootSprite;//object to change sprite
    [SerializeField] private SpriteRenderer rightBootSprite;//object to change sprite

    private Rigidbody2D rb;//to control movement
    private Animator animator;//to control animations
    private Vector2 movement;//to store the horizontal and vertical inputs
    private AudioSource audiosource;//players audiosource
    
    //initializes the compontets
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//locates the rigidbody
        animator = GetComponent<Animator>();//locates the Animator
        audiosource = GetComponent<AudioSource>();//locates the AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        if (!Inventory_Controller.UIisOpen)//animates the player only if the inventory is closes
        {
            //reads the input values and stores them
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            //sets the animator parameters to trigger the animations
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.magnitude);
            //plays the steps sound when whe star to walk
            
        }
        else//if the inventory is open stop the animations
        {
            movement.x = 0;
            movement.y = 0;
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetFloat("Speed", 0);
        }
        playAudio();
    }

    void FixedUpdate()
    {
        if (!Inventory_Controller.UIisOpen)//moves the player only if the inventory is closes
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
            
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
    

    //gets what side is the player looking at and sets the appropiate sprite from the players inventory(its triggered from the animation clips)
    public void setSideFace(int side)//side: 0=back, 1=front,2=left or righ
    {
        //changes the clothes sprite to the side the player is looking at
        if (inventory.wearingHat != null) hatSprite.sprite = inventory.wearingHat.sprites[side];//sets hat sprite
        else hatSprite.sprite = null;

        if (inventory.wearingClothes != null) clothesSprite.sprite = inventory.wearingClothes.sprites[side];//sets clothes sprite
        else clothesSprite.sprite = null;

        if (inventory.wearingBoots != null)//sets boots sprite
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
