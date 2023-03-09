using UnityEngine;
/// <summary>
/// controlls the NPCs behaviour
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class NPC_Controller : MonoBehaviour
{
    [Header("Components")]
    public NPC_Data inventory;//scriptable with the inventory info
    public GameObject Text;//"press space to interact" text that appears when the player is near
    public Dialogue_Controller dialog;//"press space to interact" text that appears when the player is near
    [Header("Audio")]
    public AudioClip greetings;//hello sound
    public AudioClip mumbling;//talking sound
    public AudioClip thanks;//thanks sound
    public AudioClip bye;//bye sound

    private Animator animator;//to control NPCs animations
    private AudioSource audiosource;//NPCs audiosource
    private bool playerIsNear = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();//locates the Animator
        audiosource = GetComponent<AudioSource>();//locates the AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !dialog.panel.activeSelf && playerIsNear && !Inventory_Controller.UIisOpen)
        {
            //playerIsNear = false;
            dialog.ShowDialog(inventory);
            //Text.SetActive(false);
        }
    }

    //when the player enters the field of view it enables interaction
    private void OnTriggerEnter2D(Collider2D other)
    {
        //plays a sound and enables text
        if (other.gameObject.tag=="Player")
        {
            playerIsNear = true;
            audiosource.clip = greetings;
            audiosource.volume = 1.0f;
            audiosource.Play();
            animator.SetTrigger("PlayerIsNear");
            Text.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //plays a sound and disables text
        if (other.gameObject.tag == "Player")
        {
            playerIsNear = false;
            audiosource.clip = bye;
            audiosource.volume = 0.65f;
            audiosource.Play();
            animator.SetTrigger("PlayerIsNear");
            Text.SetActive(false);
            dialog.CloseDialog();
        }
    }
}
