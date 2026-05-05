using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class DynamicEnemyLogic : MonoBehaviour
{
    Animator Animator;
    IEnemy Entity;
    bool IsDynamic;
    public Vector2 StartPoint;
    public Transform[] MovePointsTransform;
    public Transform[] LookPoints;
    private int CurPoint = 0;
    private bool IsMovingBack = false;
    private double epsilon = 0.1;
    private FOV_Logic FOV_Checker;
    public LayerMask Walls = 7;
    private Vector3 LookVector;

    private GameObject Player;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        StartPoint = new Vector2(transform.position.x, transform.position.y);
        var agent = GetComponent<NavMeshAgent>();
        var rb = GetComponent<Rigidbody2D>();
        if (name.StartsWith("melee"))
        {
            IsDynamic = true;
            Entity = new MeleeEnemy(agent, rb);
        }
        else if (name.StartsWith("shoot"))
        {
            IsDynamic = true;
            Entity = new ShootEnemy(agent, rb);
        }
        else if (name.StartsWith("camera"))
            Entity = new CameraEnemy(rb);
        else if (name.StartsWith("laser"))
            Entity = new LaserEnemy(rb);
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Player = GameObject.FindGameObjectWithTag("Player");

        if (IsDynamic)
        {
            var dynamic = Entity as DynamicEnemy;
            dynamic.GoNext(ConvertLocal3DToWorld2D(MovePointsTransform[CurPoint].localPosition));
        }
        FOV_Checker = new FOV_Logic(15f, 45f, Walls, Player, () => transform.position, () => LookVector, target => Entity.OnDetect(target));
        StartCoroutine(FOV_Checker.FOV_Coroutine());
    }

    void FixedUpdate()
    {
        if (Entity.IsStunned)
            return;
        if (IsDynamic)
        {
            var dynamic = Entity as DynamicEnemy;
            Vector2 curDest = dynamic.Agent.destination;
            LookVector = dynamic.Agent.desiredVelocity;
            //AnimationMethods.ChangeAnimation(Animator, true, LookVector);
            if ((dynamic.Rigidbody.position - curDest).magnitude < epsilon)
            {
                CurPoint += IsMovingBack ? -1 : 1;
                if (CurPoint == MovePointsTransform.Length || CurPoint == -1)
                {
                    CurPoint += IsMovingBack ? 1 : -1;
                    IsMovingBack = !IsMovingBack;
                }
                dynamic.GoNext(ConvertLocal3DToWorld2D(MovePointsTransform[CurPoint].localPosition));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("You Died!");
            Player.SendMessage("Respawn");
        }
    }

    private Vector2 ConvertLocal3DToWorld2D(Vector3 localPosition)
    {
        return new Vector2(localPosition.x, localPosition.y) + StartPoint;
    }

    public void OnDetect(Vector2 MovePoint)
    {
        Entity.OnDetect(MovePoint);
    }
}
