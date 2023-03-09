using System.Collections;
using UnityEngine;
using TMPro;
/// <summary>
/// slowly types the characters dialogue
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Dialogue_Controller : MonoBehaviour
{
    public NPC_Data npc;//NPC Info
    [SerializeField] private float textDelay;//delay between characters
    public GameObject panel;//text panel
    [SerializeField] private GameObject buttons;//button panel
    [SerializeField] private TMP_Text text;//text component to type on
    [SerializeField] private AudioClip buttonSound,talkingSound;//text component to type on
    [SerializeField] private Trader_controller trading;

    private AudioSource audiosource;//players audiosource
    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();//initializes audiosource
    }

    
    public void ShowDialog(NPC_Data npcData) // shows dialog panel
    {
        npc = npcData;//sets the info of the NPC calling the function
        text.text = "";//clears text
        panel.SetActive(true);//enables panel
        if (npc.job.Equals(role.Trader))//if we can trade we show the buttons
        {
            buttons.SetActive(true);
        }
        else//if we cant trade we hide the buttons
        {
            buttons.SetActive(false);
        }
        writeDialog();
    }

    public void tradeButton()
    {
        if (!Inventory_Controller.UIisOpen)
        {
            CloseDialog();
            trading.openTrade();
        }
    }
    public void closeButton()
    {
        audiosource.clip = buttonSound;
        audiosource.loop = false;
        audiosource.Play();
        CloseDialog();
    }
    public void CloseDialog()//closes the dialog panel
    {
        buttons.SetActive(false);//hides the buttons
        panel.SetActive(false);//hides the panel
        text.text = "";//clears the text
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !isTyping && panel.activeSelf)//if we press space and the panel is not typing we close it
        {
           CloseDialog();
        }
    }

    int prevrand = -1;//variable to avoid type the same dialog twice
    int rand(int range) //returns a random number between 0 and range
    {
        int customSeed = System.DateTime.Now.Millisecond;//regenerates the seed
        Random.InitState(customSeed);//sets the seed
        int random= Random.Range(0, range);//creates random number
        while (random == prevrand)//checks if is not the same number twice
        {
            random = Random.Range(0, range);
        }
        prevrand = random;//actualizes the previous number
        return random;//returns value
    }

    bool isTyping = false;//variable to avoid closing it while typing

    public void writeDialog()
    {
        if (npc.job.Equals(role.Villager))//checks if we can trade with the NPC and sets the dialogs 
        {
            var coroutine = WriteDialogue(npc.dialogues[rand(npc.dialogues.Count - 1)]);
            StartCoroutine(coroutine);
        }
        else if (npc.job.Equals(role.Trader))
        {
            var coroutine = WriteDialogue(npc.dialogues[0]);
            StartCoroutine(coroutine);
        }
        

    }
    public void writeDialog(int index)
    {
        var coroutine = WriteDialogue(npc.dialogues[index]);
        StartCoroutine(coroutine);
    }
    IEnumerator WriteDialogue(string dialogue)
    {
        isTyping = true;//sets variable to true to avoid closing it while typing
        audiosource.clip = talkingSound;
        audiosource.loop = true;
        audiosource.Play();
        text.text = npc.NPCName + ": ";//ads the NPC name to the text
        for (int i = 0; i < dialogue.Length; i++)//types the dialog letter by letter
        {
            yield return new WaitForSeconds(textDelay);
            text.text += dialogue[i];
            if (Input.GetKeyDown("space"))//if we press space we skip the typing animation
            {
                text.text = dialogue;
                break;
            }
        }
        audiosource.Stop();//stops the typing sound
        isTyping = false;//sets variable to false so now we can close the panel with space
    }
}
