using UnityEngine;

public class ChosenInstr : MonoBehaviour
{
    [SerializeField] PlayerScript PlayerRef;
    RectTransform Rect;
    void Start()
    {
        Rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Rect.anchoredPosition = new Vector2(PlayerRef.CurrentInstrument < 0 ? 1000 : PlayerRef.CurrentInstrument * 150 - 150, 0);
    }
}
