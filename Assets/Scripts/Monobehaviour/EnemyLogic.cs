using System.Linq;
using System.Threading;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyLogic : MonoBehaviour
{
    Animator Animator;
    public IEnemy Entity;
    bool IsDynamic => Entity is DynamicEnemy;
    public Vector2 StartPoint;
    public Transform[] MovePointsTransform;
    public Transform[] LookPoints;
    private int CurPoint = 0;
    private bool IsMovingBack = false;
    private double epsilon = 0.1;
    private FOV_Logic FOV_Checker;
    public LayerMask Walls = 7;
    private Vector3 LookVector;
    [SerializeField] float MoveSpeed = 0;
    double StunTime = 0;
    bool IsChasing = false;
    public Vector2 Target;

    private GameObject Player;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        StartPoint = new Vector2(transform.position.x, transform.position.y);
        var agent = GetComponent<NavMeshAgent>();
        var rb = GetComponent<Rigidbody2D>();
        if (name.StartsWith("melee"))
        {
            Entity = new MeleeEnemy(agent, rb, Animator);
            MoveSpeed = (Entity as DynamicEnemy).Agent.speed;
        }
        else if (name.StartsWith("shoot"))
        {
            Entity = new ShootEnemy(agent, rb, Animator);
            MoveSpeed = (Entity as DynamicEnemy).Agent.speed;
        }
        else if (name.StartsWith("camera"))
            Entity = new CameraEnemy(rb, Animator);
        else if (name.StartsWith("laser"))
            Entity = new LaserEnemy(rb, Animator);
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.acceleration = MoveSpeed * 30;
        Player = GameObject.FindGameObjectWithTag("Player");

        if (IsDynamic)
        {
            var dynamic = Entity as DynamicEnemy;
            dynamic.GoNext(ConvertLocal3DToWorld2D(MovePointsTransform[CurPoint].localPosition));
        }
        FOV_Checker = new FOV_Logic(10f, 45f, Walls, Player, () => transform.position, () => LookVector, target => Entity.OnDetect(target), StartChase, SetTarget);
        StartCoroutine(FOV_Checker.FOV_Coroutine());
    }

    void FixedUpdate()
    {
        if (StunTime > 0)
        {
            StunTime -= Time.fixedDeltaTime;
            if (StunTime <= 0.5 && StunTime > 0 && !Animator.GetCurrentAnimatorStateInfo(0).IsName("Rise"))
                Animator.Play("Rise");
            return;
        }
        if (IsDynamic)
        {
            var dynamic = Entity as DynamicEnemy;
            LookVector = dynamic.Agent.desiredVelocity;
            AnimationMethods.ChangeAnimation(Animator, true, LookVector);
            if (dynamic.Agent.remainingDistance <= epsilon)
            {
                if (IsChasing)
                {
                    IsChasing = false;
                    dynamic.Agent.speed = MoveSpeed;
                }
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
            Player.SendMessage("Respawn");
    }

    private Vector2 ConvertLocal3DToWorld2D(Vector3 localPosition)
    {
        return new Vector2(localPosition.x, localPosition.y) + StartPoint;
    }

    public void OnDetect(Vector2 MovePoint)
    {
        if (!Entity.IsStunned && !IsChasing)
            Entity.OnDetect(MovePoint);
    }

    public void Stun()
    {
        Animator.Play("Death");
        if (IsDynamic)
        {
            var dynamic = Entity as DynamicEnemy;
            dynamic.Agent.isStopped = true;
        }
        Entity.IsStunned = true;
        StunTime = Entity.StunTime;
    }

    public void ResetAfterStun()
    {
        Entity.IsStunned = false;
        if (IsDynamic)
        {
            var dynamic = Entity as DynamicEnemy;
            dynamic.Agent.isStopped = false;
            Debug.Log("passed");
        }
        StunTime = 0;
    }

    private void StartChase()
    {
        IsChasing = true;
        if (Entity is MeleeEnemy)
        {
            var temp = Entity as MeleeEnemy;
            temp.Agent.speed = MoveSpeed * 1.75f;
        }
    }

    public void SetTarget(Vector2 target)
    {
        Target = target;
    }
}
