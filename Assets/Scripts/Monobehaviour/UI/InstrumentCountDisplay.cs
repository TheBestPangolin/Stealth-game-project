using TMPro;
using UnityEngine;

public class InstrumentCountDisplay : MonoBehaviour
{
    private TMP_Text Text;
    [SerializeField] int ID;
    private PlayerScript Player;

    void Start()
    {
        Text = GetComponentInChildren<TMP_Text>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.SetText(Player.InstrumentCount[ID].ToString());
    }
}
