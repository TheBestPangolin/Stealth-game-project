using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    Animator Animator;
    Rigidbody2D rb;
    const float MoveSpeed = 9f;
    public Vector2 CurrentRespawnPoint;
    public Action Interact;
    /// <summary>
    /// 0 = Stone;
    /// 1 = Smoke;
    /// 2 = EMP;
    /// </summary>
    string[] InstrumentNames = new[] { "Stone", "Smoke", "EMP" };
    [SerializeField] GameObject Instrument;
    [SerializeField] int CurrentInstrument = 0;
    public int[] InstrumentCount;

    private void Awake()
    {
        InstrumentCount = new int[InstrumentNames.Length];
        InstrumentCount[0] = 10;
        InstrumentCount[1] = 10;
        InstrumentCount[2] = 10;
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
        if (Keyboard.current.eKey.wasPressedThisFrame
            && Interact.GetInvocationList().Length > 0)
            Interact();
            
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
        {
            CurrentInstrument = 0;
            HolsterHint.DisplayHint();
        }

        if (input.digit2Key.isPressed)
        {
            CurrentInstrument = 1;
            HolsterHint.DisplayHint();
        }

        if (input.digit3Key.isPressed)
        {
            CurrentInstrument = 2;
            HolsterHint.DisplayHint();
        }

        if (input.digit0Key.isPressed)
        {
            CurrentInstrument = -1;
            HolsterHint.DisableHint();
        }
    }

    void CheckThrow()
    {
        if (CurrentInstrument >= 0 && InstrumentCount[CurrentInstrument] > 0)
        {
            InstrumentCount[CurrentInstrument]--;
            var instrumentObject = Instantiate(Instrument, rb.position, Quaternion.LookRotation(Vector3.zero));
            var instrument = instrumentObject.GetComponent<Instrument>();
            
            instrument.EndPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            instrument.InstrumentName = InstrumentNames[CurrentInstrument];
            instrument.InstrumentIndex = CurrentInstrument;
            CurrentInstrument = -1;
            HolsterHint.DisableHint();
        }
    }

    public void Respawn()
    {
        Debug.Log("You Died!");
        transform.position = CurrentRespawnPoint;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void PickUp(int[] pickable)
    {
        for (var i = 0; i < InstrumentCount.Length; i++)
        {
            InstrumentCount[i] += pickable[i];
        }
    }

    public void ChangePosition(Vector2 position)
    {
        rb.position = position;
    }
}
