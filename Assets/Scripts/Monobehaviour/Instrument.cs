using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Instrument : MonoBehaviour
{
    [SerializeField] GameObject Smoke;
    public string InstrumentName;
    public Vector2 EndPosition;
    public LayerMask Walls = 7;
    Rigidbody2D rb;
    Vector2 Movement;
    private double epsilon = 0.1;
    private bool WorkedOut = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
        var move = EndPosition - rb.position;

        var hit = Physics2D.Raycast(rb.position, move, move.magnitude, Walls);
        if (hit)
        {
            EndPosition = hit.point;
            epsilon = 0.75;
        }
        Movement = (EndPosition - rb.position) * Time.fixedDeltaTime * 2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Movement);
        if (Vector2.Distance(EndPosition, rb.position) < epsilon && !WorkedOut)
        {
            Movement = Vector2.zero;
            switch (InstrumentName)
            {
                case "Stone":
                    SoundMethods.MakeAlarmSound(rb.position, 20f);
                    break;
                case "EMP":
                    StunEnemies(rb.position, 10f);
                    break;
                case "Smoke":
                    CreateSmoke(rb.position);
                    break;
            }
            WorkedOut = true;
            Destroy(gameObject, 0.5f);
        }
    }

    private void StunEnemies(Vector2 origin, float radius)
    {
        var stunnedEnemies = Physics2D.OverlapCircleAll(origin, radius);
        foreach (var enemy in stunnedEnemies
                                .Select(enemy => enemy.gameObject)
                                .Where(enemy => enemy.CompareTag("Enemy")))
        {
            enemy.gameObject.GetComponent<EnemyLogic>().Stun();
        }
    }

    private void CreateSmoke(Vector2 origin)
    {
        var smokeCloudObject = Instantiate(Smoke, origin, Quaternion.LookRotation(Vector3.zero));
        Destroy(smokeCloudObject, 3f);
    }
}
