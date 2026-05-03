using System;
using System.Linq;
using UnityEngine;

public enum Directions
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
}
public static class VectorWork
{
    private static float epsilon = 45f;
    private static Vector2 Left = new Vector2(-1, 0);
    private static Vector2 Up = new Vector2(0, 1);
    private static Vector2 Right = new Vector2(1, 0);
    private static Vector2 Down = new Vector2(0, -1);

    public static Vector2 GetFourSidedDirection(Vector2 realDirection, out Directions direction)
    {
        if (Vector2.Angle(realDirection, Left) <= epsilon)
        {
            direction = Directions.Left;
            return Left;
        }
        if (Vector2.Angle(realDirection, Up) <= epsilon)
        {
            direction = Directions.Up;
            return Up;
        }
        if (Vector2.Angle(realDirection, Down) <= epsilon)
        {
            direction = Directions.Down;
            return Down;
        }
        direction = Directions.Right;
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

public static class AnimationMethods
{
    public static void ChangeAnimation(Animator animator, bool isMoving, Vector2 directionVector, Vector2 moveVector)
    {
        VectorWork.GetFourSidedDirection(directionVector, out var lookDirection);
        VectorWork.GetFourSidedDirection(moveVector, out var moveDirection);
        var animationName = isMoving ? "Walk" : "Idle";
        animationName += Enum.GetName(
            typeof(Directions),
            isMoving ? moveDirection : lookDirection);
        animator.Play(animationName);
    }

    public static void ChangeAnimation(Animator animator, bool isMoving, Vector2 directionVector)
    {
        VectorWork.GetFourSidedDirection(directionVector, out var lookDirection);
        var animationName = isMoving ? "Walk" : "Idle";
        animationName += Enum.GetName(typeof(Directions), lookDirection);
        animator.Play(animationName);
    }
}

public static class SoundMethods
{
    public static void MakeAlarmSound(Vector2 from, float radius)
    {
        var alarmedEnemies = Physics2D.OverlapCircleAll(from, radius);
        foreach (var enemy in alarmedEnemies
                                .Select(enemy => enemy.gameObject)
                                .Where(enemy => enemy.CompareTag("Enemy")))
        {
            enemy.gameObject.GetComponent<DynamicEnemyLogic>().OnDetect(from);
        }
    }
}