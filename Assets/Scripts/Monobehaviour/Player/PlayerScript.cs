using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public Animator Animator;
    public LineOfThrowRenderer LineRend;
    public SpriteRenderer InstrumentRenderer;
    Rigidbody2D rb;
    const float MoveSpeed = 7f;
    public Vector2 CurrentRespawnPoint => Player_container.CurrentRespawn;
    public Action Interact;
    /// <summary>
    /// 0 = Stone;
    /// 1 = Smoke;
    /// 2 = EMP;
    /// </summary>
    string[] InstrumentNames = new[] { "Stone", "Smoke", "EMP" };
    [SerializeField] GameObject Instrument;

    public int CurrentInstrument = -1;
    public int[] InstrumentCount;

    private void Awake()
    {
        LineRend = GetComponentInChildren<LineOfThrowRenderer>();
        InstrumentRenderer = GetComponentsInChildren<SpriteRenderer>().Where(r => r.gameObject.name.StartsWith("InstrumentRend")).First();
        InstrumentCount = new int[InstrumentNames.Length];
        InstrumentCount[0] = 1;
        InstrumentCount[1] = 1;
        InstrumentCount[2] = 1;
        Animator = GetComponentInChildren<Animator>();
        
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        Player_container.CurrentRespawn = rb.position;
        transform.position = CurrentRespawnPoint;
    }


    void FixedUpdate()
    {
        if (!PauseGame.isPaused)
        {
            LineRend.SetStart(transform.position);
            if (!Animator.GetBool("IsDead"))
            {
                // Логика движения
                var moveVector = GetMovementVector() * MoveSpeed;
                var newPos = rb.position + moveVector * Time.fixedDeltaTime;

                // Логика слежения модельки за курсором мыши
                var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                if (CurrentInstrument >= 0)
                {
                    var vect = mousePos - transform.position;
                    var hit = Physics2D.Raycast(rb.position, vect, vect.magnitude, 7);
                    var hitpos = mousePos;
                    if (hit)
                        hitpos = hit.point;
                    LineRend.DrawLineOfThrow(hitpos);
                    InstrumentRenderer.sprite = Resources.Load<Sprite>($"Instrument/{InstrumentNames[CurrentInstrument]}");
                    InstrumentRenderer.transform.localPosition = mousePos - transform.position;
                }
                else
                    LineRend.StopDrawing();

                var lookVector = new Vector2(mousePos.x, mousePos.y) - newPos;

                AnimationMethods.ChangeAnimation(Animator, moveVector != Vector2.zero, lookVector, moveVector);
                // Передвижение 
                rb.MovePosition(newPos);

                // Слежение камеры за игроком
                Camera.main.transform.position = new Vector3(rb.position.x, rb.position.y, -10);

                ChangeEquipment();
            }
        }
    }

    private void Update()
    {
        if (!Animator.GetBool("IsDead") && !PauseGame.isPaused)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
                CheckThrow();
            if (Keyboard.current.eKey.wasPressedThisFrame)
                Interact?.Invoke();
        }
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
            LineRend.StopDrawing();
            InstrumentRenderer.sprite = null;
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
            LineRend.StopDrawing();
            InstrumentRenderer.sprite = null;
        }
    }

    public void Respawn()
    {
        Animator.Play("Death");
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
