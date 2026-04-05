using System;
using UnityEngine;

public abstract class Enemy
{
    public Vector2 DetectedPosition;
    public abstract void OnDetect();
    public double StunTime { get; }
    public PolygonCollider2D Vision;
}

