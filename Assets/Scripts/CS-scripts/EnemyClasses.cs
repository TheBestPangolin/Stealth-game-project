using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IEnemy
{
    double StunTime { get; set; }
    bool IsStunned { get; set; }
    Rigidbody2D Rigidbody { get;}

    void OnDetect(Vector2 target);
}

public abstract class DynamicEnemy : IEnemy
{
    public double StunTime { get; set; }
    public bool IsStunned { get; set; }
    public NavMeshAgent Agent;
    public Rigidbody2D Rigidbody { get;}
    public DynamicEnemy(NavMeshAgent agent, Rigidbody2D rigidbody)
    {
        Agent = agent;
        Rigidbody = rigidbody;
    }
    public abstract void OnDetect(Vector2 target);
    public void GoNext(Vector3 MovePoint)
    {
        Agent.SetDestination(MovePoint);
    }
}

public abstract class StaticEnemy : IEnemy
{
    public double StunTime { get; set; }
    public bool IsStunned { get; set; }
    public Rigidbody2D Rigidbody { get; }

    public StaticEnemy(Rigidbody2D rigidbody)
    {
        Rigidbody = rigidbody;
    }
    public abstract void OnDetect(Vector2 target);
}
public class MeleeEnemy : DynamicEnemy
{
    public MeleeEnemy(NavMeshAgent agent, Rigidbody2D rigidbody) : base(agent, rigidbody)
    {
        StunTime = 2;
        Agent.speed = 2f;
    }

    public override void OnDetect(Vector2 target)
    {
        if (!IsStunned)
            Agent.SetDestination(target);
    }
}

public class ShootEnemy : DynamicEnemy
{

    public ShootEnemy(NavMeshAgent agent, Rigidbody2D rigidbody) : base(agent, rigidbody)
    {
        StunTime = 3;
        Agent.speed = 1f;
    }

    public override void OnDetect(Vector2 target)
    {

    }
}

public class CameraEnemy : StaticEnemy
{

    public CameraEnemy(Rigidbody2D rigidbody) : base(rigidbody)
    {
        StunTime = 3;
    }

    public override void OnDetect(Vector2 target)
    {
    }
}

public class LaserEnemy : StaticEnemy
{

    public LaserEnemy(Rigidbody2D rigidbody) : base(rigidbody)
    {
        StunTime = 3;
    }

    public override void OnDetect(Vector2 target)
    {
    }
}
