using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Instrument : MonoBehaviour
{
    public string InstrumentName;
    public Vector2 EndPosition;
    public LayerMask Walls = 7;
    Rigidbody2D rb;
    Vector2 Movement;
    private double epsilon = 0.1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        var move = EndPosition - rb.position;
        var ray = new Ray(rb.position, move);

        var hit = Physics2D.Raycast(rb.position, move, move.magnitude, Walls);
        if (hit)
            EndPosition = hit.point;
        Movement = (EndPosition - rb.position) * Time.fixedDeltaTime * 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Movement);
        if (Vector2.Distance(EndPosition, rb.position) < epsilon)
        {
            Movement = Vector2.zero;
        }
    }
}
