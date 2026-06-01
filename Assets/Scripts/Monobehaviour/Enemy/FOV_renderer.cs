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
        DrawCircleSector(32, EnemyLogic.Distance / transform.parent.localScale.x, Enemy.LookVector);
    }

    void DrawCircleSector(int steps, float radius, Vector2 lookVector)
    {
        CircleRenderer.positionCount = steps + 3;
        CircleRenderer.useWorldSpace = false; // Оставляем false

        CircleRenderer.startColor = Enemy.IsSeeing ? Color.green : Color.red;
        CircleRenderer.endColor = Enemy.IsSeeing ? Color.green : Color.red;

        // Угол сектора (например, 90 градусов)
        float sectorAngleDegrees = 90f;
        float sectorAngleRad = sectorAngleDegrees * Mathf.Deg2Rad;

        // lookVector — это направление в локальных координатах объекта
        Vector2 direction = lookVector.normalized;

        // Вычисляем угол направления (0° = вправо, 90° = вверх)
        float centerAngle = Mathf.Atan2(direction.y, direction.x);

        // Если ваш спрайт смотрит вверх (0, 1), а вам нужно вперёд (1, 0)
        // Раскомментируйте следующую строку:
        // centerAngle -= 90 * Mathf.Deg2Rad;

        float startAngle = centerAngle - sectorAngleRad / 2f;

        // Центр
        CircleRenderer.SetPosition(0, Vector3.zero);

        // Дуга
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            float currentAngle = startAngle + t * sectorAngleRad;

            float x = Mathf.Cos(currentAngle) * radius;
            float y = Mathf.Sin(currentAngle) * radius;

            CircleRenderer.SetPosition(i + 1, new Vector3(x, y, 0));
        }

        // Замыкание
        CircleRenderer.SetPosition(steps + 2, Vector3.zero);
    }


}
