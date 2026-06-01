using System.Net;
using UnityEngine;

public class LineOfThrowRenderer : MonoBehaviour
{
    LineRenderer LineRend;
    void Start()
    {
        LineRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawLineOfThrow(Vector2 endPoint)
    {
        
        LineRend.SetPosition(1, endPoint);
    }

    public void StopDrawing()
    {
        LineRend.SetPosition(1, LineRend.GetPosition(0));
    }

    public void SetStart(Vector2 startPoint)
    {
        LineRend.SetPosition(0, startPoint);
    }
}
