using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class DynamicEnemy
{
    public double StunTime;
    public PolygonCollider2D Vision;
    public NavMeshAgent Agent;
    public Rigidbody2D Rigidbody;
    public DynamicEnemy(NavMeshAgent agent, Rigidbody2D rigidbody)
    {
        Agent = agent;
        Rigidbody = rigidbody;
    }
    public abstract void OnDetect(Vector2 target);
    public void GoNext(Vector3 MovePoint)
    {
        lock (Agent)
        {
            Agent.SetDestination(MovePoint);
        }
    }
}

public abstract class StaticEnemy
{
    public double StunTime;
    public PolygonCollider2D Vision;
    public Rigidbody2D Rigidbody;

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
        lock (Agent)
        {
            Agent.SetDestination(target);
        }
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
