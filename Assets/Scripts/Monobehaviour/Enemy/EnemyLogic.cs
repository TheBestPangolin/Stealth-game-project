using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
    public Vector3 LookVector;
    [SerializeField] float MoveSpeed = 0;
    double StunTime = 0;
    public bool IsChasing = false;
    public Vector2 Target;
    public const float Distance = 10f;
    public const float Angle = 45f;
    public bool IsSeeing;

    private GameObject Player;

    private void Awake()
    {
        MovePointsTransform = MovePointsTransform.Where(x => x != null).ToArray();
        Animator = GetComponentInChildren<Animator>();
        StartPoint = new Vector2(transform.position.x, transform.position.y);
        NavMeshAgent agent = null;
        if (!name.StartsWith("camera"))
            agent = GetComponent<NavMeshAgent>();
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
        else if (name.StartsWith("mannequin"))
            Entity = new LaserEnemy(rb, Animator);
        if (!name.StartsWith("camera"))
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.acceleration = MoveSpeed * 30;
            agent.stoppingDistance = 0;
            agent.autoBraking = false;
        }
        Player = GameObject.FindGameObjectWithTag("Player");

        if (IsDynamic)
        {
            var dynamic = Entity as DynamicEnemy;
            if (MovePointsTransform.Length == 0)
                MovePointsTransform = new[] { transform };
            dynamic.GoNext(ConvertLocal3DToWorld2D(MovePointsTransform[CurPoint].localPosition));
        }
        FOV_Checker = new FOV_Logic(Distance, Angle, Walls, Player, () => transform.position, () => LookVector, 
            Entity is LaserEnemy ? target => LookVector = target - Entity.Rigidbody.position : target => Entity.OnDetect(target), 
            StartChase, SetTarget, b => IsSeeing = b);
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
            if (!Animator.GetBool("IsShootPlaying"))
                AnimationMethods.ChangeAnimation(Animator, MovePointsTransform.Length > 1 || IsChasing, LookVector);
            if (dynamic.Agent.remainingDistance <= epsilon || dynamic.Agent.pathStatus != NavMeshPathStatus.PathComplete)
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
                if (MovePointsTransform.Length > 0)
                    dynamic.GoNext(ConvertLocal3DToWorld2D(MovePointsTransform[CurPoint].localPosition));
            }
        }
        else
        {
            if (MovePointsTransform.Length == 0)
            {
                if (!IsSeeing || Entity is not LaserEnemy)
                    LookVector = Vector3.right;
            }
            else
                LookVector = (Vector2)MovePointsTransform[0].position - Entity.Rigidbody.position;
            AnimationMethods.ChangeAnimation(Animator, false, LookVector);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerScript>().Respawn();
    }

    private Vector2 ConvertLocal3DToWorld2D(Vector3 localPosition)
    {
        return new Vector2(localPosition.x, localPosition.y) + StartPoint;
    }

    public void OnDetect(Vector2 MovePoint)
    {
        if (!Entity.IsStunned && !IsChasing && Entity is not CameraEnemy)
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
            temp.Agent.speed = MoveSpeed * 1.5f;
        }
    }

    public void SetTarget(Vector2 target)
    {
        Target = target;
    }
}
