using UnityEngine;

public class Cardcounter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cardCounterHint.DisplayHint(Player_container.cardsCounter.ToString() + "/3");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            cardCounterHint.DisableHint();
        }
    }
}
