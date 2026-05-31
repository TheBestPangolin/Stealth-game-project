using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 EndPosition;
    Vector2 Movement;
    const float MoveSpeed = 30f;
    const float MinRotation = -15f;
    const float MaxRotation = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var randAngle = Random.value * (MaxRotation - MinRotation) + MinRotation;
        rb = GetComponent<Rigidbody2D>();
        Movement = (EndPosition - rb.position).normalized;
        var x = Movement.x;
        var y = Movement.y;
        Movement = Quaternion.Euler(0, 0, randAngle) * Movement;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Movement * MoveSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(Movement.GetAngle() + 90f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("Respawn");
            Destroy(gameObject);
        }
        if (collision != null && collision.gameObject.layer == 7)
            Destroy(gameObject);
    }
}
