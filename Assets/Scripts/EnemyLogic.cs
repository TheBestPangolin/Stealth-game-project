using UnityEngine;
using UnityEngine.AI;

public class DynamicEnemyLogic : MonoBehaviour
{
    DynamicEnemy Entity;
    public Transform[] MovePoints;
    public Transform[] LookPoints;
    private int CurPoint = 0;
    private bool IsMovingBack = false;
    private double epsilon = 0.1;

    private void Awake()
    {
        var agent = GetComponent<NavMeshAgent>();
        var rb = GetComponent<Rigidbody2D>();
        if (name.StartsWith("melee"))
            Entity = new MeleeEnemy(agent, rb);
        else if (name.StartsWith("shoot"))
            Entity = new ShootEnemy(agent, rb);
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Entity.GoNext(MovePoints[CurPoint].position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var curDest = MovePoints[CurPoint].position;
        if ((Entity.Rigidbody.position - new Vector2(curDest.x, curDest.y)).magnitude < epsilon)
        {
            CurPoint += IsMovingBack ? -1 : 1;
            if (CurPoint == MovePoints.Length || CurPoint == -1)
            {
                CurPoint += IsMovingBack ? 1 : -1;
                IsMovingBack = !IsMovingBack;
            }
            Entity.GoNext(MovePoints[CurPoint].position);
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
}
