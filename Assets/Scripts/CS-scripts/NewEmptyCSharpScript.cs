using System;
using UnityEngine;


public static class VectorWork
{
    private static float epsilon = 45f;
    private static Vector2 Left = new Vector2(1, 0);
    private static Vector2 Up = new Vector2(0, 1);
    private static Vector2 Right = new Vector2(-1, 0);
    private static Vector2 Down = new Vector2(0, -1);

    public static Vector2 GetFourSidedDirection(Vector2 realDirection)
    {
        if (Vector2.Angle(realDirection, Left) < epsilon)
            return Left;
        if (Vector2.Angle(realDirection, Up) < epsilon)
            return Up;
        if (Vector2.Angle(realDirection, Down) < epsilon)
            return Down;
        return Right;
    }
}

public static class VectorExtensions
{
    public static float GetAngle(this Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }
}
