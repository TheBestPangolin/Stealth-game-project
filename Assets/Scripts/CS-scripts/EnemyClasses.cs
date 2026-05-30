using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public interface IEnemy
{
    int StunTime { get; set; }
    bool IsStunned { get; set; }
    Rigidbody2D Rigidbody { get;}
    Animator Animator { get; set; }

    void OnDetect(Vector2 target);
}

public abstract class DynamicEnemy : IEnemy
{
    public int StunTime { get; set; }
    public bool IsStunned { get; set; }
    public NavMeshAgent Agent;
    public Rigidbody2D Rigidbody { get;}
    public Animator Animator { get; set; }

    public DynamicEnemy(NavMeshAgent agent, Rigidbody2D rigidbody, Animator animator)
    {
        Agent = agent;
        Rigidbody = rigidbody;
        Animator = animator;
    }
    public abstract void OnDetect(Vector2 target);
    public void GoNext(Vector3 MovePoint)
    {
        Agent.SetDestination(MovePoint);
    }
}

public abstract class StaticEnemy : IEnemy
{
    public int StunTime { get; set; }
    public bool IsStunned { get; set; }
    public Rigidbody2D Rigidbody { get; }
    public Animator Animator { get; set; }

    public StaticEnemy(Rigidbody2D rigidbody, Animator animator)
    {
        Rigidbody = rigidbody;
        Animator = animator;
    }
    public abstract void OnDetect(Vector2 target);
}
public class MeleeEnemy : DynamicEnemy
{
    public MeleeEnemy(NavMeshAgent agent, Rigidbody2D rigidbody, Animator animator) : base(agent, rigidbody, animator)
    {
        StunTime = 2;
        Agent.speed = 7f;
    }

    public override void OnDetect(Vector2 target)
    {
        if (!IsStunned)
            Agent.SetDestination(target);
    }
}

public class ShootEnemy : DynamicEnemy
{

    public ShootEnemy(NavMeshAgent agent, Rigidbody2D rigidbody, Animator animator) : base(agent, rigidbody, animator)
    {
        StunTime = 3;
        Agent.speed = 5f;
    }

    public override void OnDetect(Vector2 target)
    {
        if (!IsStunned && !Agent.isStopped)
        {
            Agent.SetDestination(target);
            AnimationMethods.PlayShootAnimation(Animator, Agent.desiredVelocity);
            Agent.isStopped = true;
            
        }
    }
}

public class CameraEnemy : StaticEnemy
{

    public CameraEnemy(Rigidbody2D rigidbody, Animator animator) : base(rigidbody, animator)
    {
        StunTime = 3;
    }

    public override void OnDetect(Vector2 target)
    {
        SoundMethods.MakeAlarmSound(Rigidbody.position, 30f);
    }
}

public class LaserEnemy : StaticEnemy
{

    public LaserEnemy(Rigidbody2D rigidbody, Animator animator) : base(rigidbody, animator)
    {
        StunTime = 3;
    }

    public override void OnDetect(Vector2 target)
    {
    }
}
