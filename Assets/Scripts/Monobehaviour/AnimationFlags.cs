using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class AnimationFlags : MonoBehaviour
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
        var bulletObj = Instantiate(Resources.Load<GameObject>("Bullet"), parent.Entity.Rigidbody.position, Quaternion.LookRotation(Vector3.zero));
        bulletObj.GetComponent<Bullet>().EndPosition = parent.Target;
        SoundManager.instance.PlaySoundFXClip(Resources.Load<AudioClip>("Sounds/shot"), parent.transform, 0.5f);
    }

    void StopShooting()
    {
        var parent = GetComponentInParent<EnemyLogic>();
        (parent.Entity as DynamicEnemy).Agent.isStopped = false;
        parent.Entity.Animator.SetBool("IsShootPlaying", false);
        SoundManager.instance.PlaySoundFXClip(Resources.Load<AudioClip>("Sounds/reload"), parent.transform, 1);
    }

    void Respawn()
    {
        var load = SceneManager.GetActiveScene();
        SceneManager.LoadScene(load.name);
    }

    void SetDead()
    {
        var parent = GetComponentInParent<PlayerScript>();
        parent.Animator.SetBool("IsDead", true);
    }
}
