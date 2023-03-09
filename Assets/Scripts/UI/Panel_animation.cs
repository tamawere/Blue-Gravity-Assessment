using UnityEngine;

/// <summary>
/// UI animation moves from outside of the screen to a given point 
/// </summary>
public class Panel_animation : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private Vector2 initpos;
    [SerializeField]
    private Vector2 finalpos;

    private Vector2 posA, posB;
    bool move = false;
    bool togled = false;
    // Start is called before the first frame update
    void Start()
    {
        initpos = transform.GetComponent<RectTransform>().anchoredPosition;
        posA = initpos;
        posB = finalpos;
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            if (Vector2.Distance(transform.GetComponent<RectTransform>().anchoredPosition,finalpos)>0.2f)
            {
                transform.GetComponent<RectTransform>().anchoredPosition =
                   Vector2.MoveTowards(transform.GetComponent<RectTransform>().anchoredPosition, finalpos, speed * Time.deltaTime);
            }
            else
            {
                //this.enabled = false;
                move = false;
            }
        }
    }

    
    public void ToggleMenu()//function to display the panel
    {
        if (!togled)
        {
            initpos = posA;
            finalpos = posB;
            move = true;
            togled = true;
        }
        else
        {
            initpos = posB;
            finalpos = posA;
            move = true;
            togled = false;
        }

    }
}