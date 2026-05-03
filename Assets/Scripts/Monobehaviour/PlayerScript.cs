using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    Animator Animator;
    Rigidbody2D rb;
    const float MoveSpeed = 10f;
    Vector2 CurrentRespawnPoint;
    /// <summary>
    /// 0 = Stone;
    /// 1 = Smoke;
    /// 2 = EMP;
    /// </summary>
    string[] InstrumentNames = new[] { "Stone", "Smoke", "EMP" };
    [SerializeField] GameObject Instrument;
    int CurrentInstrument = 0;
    int[] InstrumentCount;

    private void Awake()
    {
        InstrumentCount = new int[3];
        InstrumentCount[0] = 10;
        Animator = GetComponentInChildren<Animator>();
        
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        CurrentRespawnPoint = rb.position;
    }


    void FixedUpdate()
    {
        // Логика движения
        var moveVector = GetMovementVector() * MoveSpeed;
        var newPos = rb.position + moveVector * Time.fixedDeltaTime;

        // Логика слежения модельки за курсором мыши
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var lookVector = new Vector2(mousePos.x, mousePos.y) - newPos;

        AnimationMethods.ChangeAnimation(Animator, moveVector != Vector2.zero, lookVector, moveVector);
        // Передвижение 
        rb.MovePosition(newPos);

        // Слежение камеры за игроком
        Camera.main.transform.position = new Vector3(rb.position.x, rb.position.y, -10);

        ChangeEquipment();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            CheckThrow();
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

    void ChangeEquipment()
    {
        var input = Keyboard.current;
        if (input.digit1Key.isPressed)
            CurrentInstrument = 0;

        if (input.digit2Key.isPressed)
            CurrentInstrument = 1;

        if (input.digit3Key.isPressed)
            CurrentInstrument = 2;
    }

    void CheckThrow()
    {
        if (InstrumentCount[CurrentInstrument] > 0)
        {
            InstrumentCount[CurrentInstrument]--;
            var instrumentObject = Instantiate(Instrument, rb.position, Quaternion.LookRotation(Vector3.zero));
            var instrument = instrumentObject.GetComponent<Instrument>();
            
            instrument.EndPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            instrument.InstrumentName = InstrumentNames[CurrentInstrument];
        }
    }

    void Respawn()
    {
        transform.position = CurrentRespawnPoint;
    }
}
