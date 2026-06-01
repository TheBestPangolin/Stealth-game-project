using UnityEngine;

public class FOV_renderer : MonoBehaviour
{
    LineRenderer CircleRenderer;
    EnemyLogic Enemy;
    void Start()
    {
        CircleRenderer = GetComponent<LineRenderer>();
        Enemy = GetComponentInParent<EnemyLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawCircleSector(32, EnemyLogic.Distance, Enemy.LookVector);
    }

    void DrawCircleSector(int steps, float radius, Vector2 lookVector)
    {
        CircleRenderer.positionCount = steps + 3;
        //CircleRenderer.startColor

        var workVect = lookVector.normalized * radius;

        var leftLine = Quaternion.Euler(0, 0, Mathf.PI / 4) * workVect;

        var rightLine = Quaternion.Euler(0, 0, -Mathf.PI / 4) * workVect;

        CircleRenderer.SetPosition(0, Vector3.zero);

        CircleRenderer.SetPosition(1, rightLine);

        var startAngle = rightLine.GetAngle() * Mathf.Deg2Rad;

        var endAngle = leftLine.GetAngle() * Mathf.Deg2Rad;

        for (var i = 2; i < steps + 2; i++)
        {
            var circumferenceProg = (float)i / (steps * 4);

            var curRad = circumferenceProg * 2 * Mathf.PI;

            var x = Mathf.Cos(curRad + startAngle) * radius;
            var y = Mathf.Sin(curRad + startAngle) * radius;

            CircleRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

        CircleRenderer.SetPosition(steps + 2, leftLine);
    }
}
