using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class StaticEnemyLogic : MonoBehaviour
{
    StaticEnemy Entity;
    public Transform[] LookPoints;
    private FOV_Logic FOV_Checker;
    public LayerMask Walls;
    private Vector3 LookVector;

    private void Awake()
    {
        LookVector = transform.up;
        var rb = GetComponent<Rigidbody2D>();
        if (name.StartsWith("camera"))
            Entity = new CameraEnemy(rb);
        else if (name.StartsWith("laser"))
            Entity = new LaserEnemy(rb);
        FOV_Checker = new FOV_Logic(10f, 45f, Walls, GameObject.FindGameObjectWithTag("Player"), () => transform.position, () => LookVector, target => Entity.OnDetect(target));
        StartCoroutine(FOV_Checker.FOV_Coroutine());
    }
    void FixedUpdate()
    {

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
