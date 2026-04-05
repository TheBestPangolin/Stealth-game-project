using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy
{
    
    public double StunTime;
    public Stopwatch StunWatch;
    public PolygonCollider2D Vision;
    public NavMeshAgent Agent;
    public GameObject GameObject;
    public Rigidbody2D Rigidbody;
    public Transform[] MovePoints;
    public float MoveSpeed;
    public Enemy(NavMeshAgent agent, GameObject gameObject, Rigidbody2D rigidbody)
    {
        Agent = agent;
        GameObject = gameObject;
        Rigidbody = rigidbody;
    }
    public abstract void OnDetect();
    //public void GoNext()
    //{
    //    Agent.
    //}
}

public class MeleeEnemy : Enemy
{
    public MeleeEnemy(NavMeshAgent agent, GameObject gameObject, Rigidbody2D rigidbody) : base(agent, gameObject, rigidbody)
    {
        MoveSpeed = 2f;
        StunTime = 2;
    }

    public override void OnDetect()
    {
        
    }
}

public class ShootEnemy : Enemy
{

    public ShootEnemy(NavMeshAgent agent, GameObject gameObject, Rigidbody2D rigidbody) : base(agent, gameObject, rigidbody)
    {
        MoveSpeed = 1f;
        StunTime = 3;
    }

    public override void OnDetect()
    {
        
    }
}

public class CameraEnemy : Enemy
{

    public CameraEnemy(NavMeshAgent agent, GameObject gameObject, Rigidbody2D rigidbody) : base(agent, gameObject, rigidbody)
    {
        MoveSpeed = 0f;
        StunTime = 3;
    }

    public override void OnDetect()
    {
    }
}

public class LaserEnemy : Enemy
{

    public LaserEnemy(NavMeshAgent agent, GameObject gameObject, Rigidbody2D rigidbody) : base(agent, gameObject, rigidbody)
    {
        MoveSpeed = 0f;
        StunTime = 3;
    }

    public override void OnDetect()
    {
    }
}
