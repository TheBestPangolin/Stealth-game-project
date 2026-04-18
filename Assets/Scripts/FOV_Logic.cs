using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FOV_Logic
{
    private readonly float ViewDistance;
    private readonly float ViewAngle;

    private readonly LayerMask Targets;
    private readonly LayerMask Walls;

    private readonly GameObject Player;

    private Func<Transform> TransformPosition;

    private Action<Vector2> OnDetect;

    public FOV_Logic(float viewDistance, float viewAngle, LayerMask targets, LayerMask walls, GameObject player, Func<Transform> position, Action<Vector2> onDetect)
    {
        ViewDistance = viewDistance;
        ViewAngle = viewAngle;
        Targets = targets;
        Walls = walls;
        Player = player;
        TransformPosition = position;
        OnDetect = onDetect;
    }

    public IEnumerator FOV_Coroutine()
    {
        // Действие происходит каждые 0.2 секунды
        var delay = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return delay;
            var playerPos = Player.transform.position;
            var myPos = TransformPosition().position;
            var up = TransformPosition().up;
            if (FOV_Check(playerPos, myPos, up))
                OnDetect(playerPos);
        }
    }

    private bool FOV_Check(Vector2 playerPos, Vector2 myPos, Vector2 up)
    {
        var distance = Vector2.Distance(playerPos, myPos);
        var direction = (playerPos - myPos).normalized;
        return distance <= ViewDistance
                    && Vector2.Angle(myPos, direction) < ViewAngle / 2
                    && !Physics2D.Raycast(myPos, direction, distance, Walls);
    }
}
