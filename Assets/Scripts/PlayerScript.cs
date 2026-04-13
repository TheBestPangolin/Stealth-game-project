using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    Rigidbody2D rb;
    const float MoveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }

    
    void FixedUpdate()
    {
        // Логика движения
        var moveVector = GetMovementVector() * MoveSpeed;
        var newPos = rb.position + moveVector * Time.fixedDeltaTime;

        // Логика слежения модельки за курсором мыши
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var lookVector = new Vector2(mousePos.x, mousePos.y) - newPos;

        // Передвижение 
        rb.MovePositionAndRotation(newPos, Mathf.Atan2(lookVector.y, lookVector.x) * Mathf.Rad2Deg);
        
        // Слежение камеры за игроком
        Camera.main.transform.position = new Vector3(rb.position.x, rb.position.y, -10);
    }

    /// <summary>
    /// Возвращает вектор передвижения по нажатым клавишам W,A,S,D
    /// </summary>
    Vector2 GetMovementVector()
    {
        var input = Keyboard.current;
        var movement = new Vector2();
        if (input.wKey.isPressed)
            movement += new Vector2(0, 1);
        if (input.sKey.isPressed)
            movement += new Vector2(0, -1);
        if (input.aKey.isPressed)
            movement += new Vector2(-1, 0);
        if (input.dKey.isPressed)
            movement += new Vector2(1, 0);
        return movement.normalized;
    }
}
