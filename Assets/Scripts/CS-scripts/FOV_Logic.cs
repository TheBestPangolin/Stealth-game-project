using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FOV_Logic
{
    private readonly float ViewDistance;
    private readonly float ViewAngle;

    private readonly LayerMask Walls;

    private readonly GameObject Player;

    private Func<Vector3> Position;

    private Func<Vector3> Up;

    private Action<Vector2> OnDetect;

    private Action StartChase;

    public bool SawLastIteration;
    public Action<Vector2> SetTarget;

    private Action<bool> SetIsSeeing;

    public FOV_Logic(float viewDistance, float viewAngle, LayerMask walls, 
        GameObject player, Func<Vector3> position, Func<Vector3> up, Action<Vector2> onDetect, Action startChase, Action<Vector2> setTarget, Action<bool> setIsSeeing)
    {
        ViewDistance = viewDistance;
        ViewAngle = viewAngle;
        Walls = walls;
        Player = player;
        Position = position;
        Up = up;
        OnDetect = onDetect;
        StartChase = startChase;
        SetTarget = setTarget;
        SetIsSeeing = setIsSeeing;
    }

    public IEnumerator FOV_Coroutine()
    {
        // Действие происходит каждые 0.1 секунду
        var delay = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return delay;
            var playerPos = Player.transform.position;
            var myPos = Position();
            var up = Up();
            if (!PauseGame.isPaused && FOV_Check(playerPos, myPos, up))
            {
                SawLastIteration = true;
                SetIsSeeing(true);
                StartChase();
                OnDetect(playerPos);
                SetTarget(playerPos);
            }
            else
            {
                SawLastIteration = false;
                SetIsSeeing(false);
            }
        }
    }

    private bool FOV_Check(Vector2 playerPos, Vector2 myPos, Vector2 up)
    {
        var distance = (myPos - playerPos).magnitude;
        var direction = (playerPos - myPos).normalized;
        var hit = Physics2D.Raycast(myPos, direction, distance, Walls, -10, 50);
        return (distance <= ViewDistance
                    && Vector2.Angle(up, direction) < ViewAngle
                    && hit.collider == null);
    }

}
