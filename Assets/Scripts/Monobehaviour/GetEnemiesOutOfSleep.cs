using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GetEnemiesOutOfSleep : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WakeUp()
    {
        var parent = GetComponentInParent<EnemyLogic>();
        parent.ResetAfterStun();
    }

    void Shoot()
    {
        var parent = GetComponentInParent<EnemyLogic>();
        var bulletObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Bullet"), parent.Entity.Rigidbody.position, Quaternion.LookRotation(Vector3.zero));
        bulletObj.GetComponent<Bullet>().EndPosition = parent.Target;
    }

    void StopShooting()
    {
        var parent = GetComponentInParent<EnemyLogic>();
        (parent.Entity as DynamicEnemy).Agent.isStopped = false;
    }
}
