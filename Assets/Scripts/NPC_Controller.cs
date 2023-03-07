using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class NPC_Controller : MonoBehaviour
{
    public NPC_Data inventory;
    [Header("Audio")]
    public AudioClip greetings;
    public AudioClip mumbling;
    public AudioClip thanks;
    public AudioClip bye;
    public GameObject Text;

    private Animator animator;//to control animations
    private AudioSource audiosource;//players audiosource

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();//locates the Animator
        audiosource = GetComponent<AudioSource>();//locates the AudioSource
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player")
        {
            audiosource.clip = greetings;
            audiosource.volume = 1.0f;
            audiosource.Play();
            animator.SetTrigger("PlayerIsNear");
            Text.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            audiosource.clip = bye;
            audiosource.volume = 0.65f;
            audiosource.Play();
            animator.SetTrigger("PlayerIsNear");
            Text.SetActive(false);
        }
    }
}
