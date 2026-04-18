using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DynamicEnemyLogic : MonoBehaviour
{
    DynamicEnemy Entity;
    public Vector2 StartPoint;
    public Transform[] MovePoints;
    public Transform[] LookPoints;
    private int CurPoint = 0;
    private bool IsMovingBack = false;
    private double epsilon = 0.1;
    private FOV_Logic FOV_Checker;
    public LayerMask Targets;
    public LayerMask Walls;

    private void Awake()
    {
        StartPoint = new Vector2(transform.position.x, transform.position.y);
        var agent = GetComponent<NavMeshAgent>();
        var rb = GetComponent<Rigidbody2D>();
        if (name.StartsWith("melee"))
            Entity = new MeleeEnemy(agent, rb);
        else if (name.StartsWith("shoot"))
            Entity = new ShootEnemy(agent, rb);
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Entity.GoNext(ConvertLocal3DToWorld2D(MovePoints[CurPoint].localPosition));
        FOV_Checker = new FOV_Logic(10f, 45f, Targets, Walls, GameObject.FindGameObjectWithTag("Player"), () => transform, target => Entity.OnDetect(target));
        StartCoroutine(FOV_Checker.FOV_Coroutine());
    }

    void FixedUpdate()
    {
        Vector2 curDest = Entity.Agent.destination;
        if ((Entity.Rigidbody.position - curDest).magnitude < epsilon)
        {
            CurPoint += IsMovingBack ? -1 : 1;
            if (CurPoint == MovePoints.Length || CurPoint == -1)
            {
                CurPoint += IsMovingBack ? 1 : -1;
                IsMovingBack = !IsMovingBack;
            }
            Entity.GoNext(ConvertLocal3DToWorld2D(MovePoints[CurPoint].localPosition));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("You Died!");
            collision.transform.position = new Vector3(12, 20, 0);
        }
    }

    private Vector2 ConvertLocal3DToWorld2D(Vector3 localPosition)
    {
        return new Vector2(localPosition.x, localPosition.y) + StartPoint;
    }
}
