using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 EndPosition;
    Vector2 Movement;
    const float MoveSpeed = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Movement = (EndPosition - rb.position).normalized;
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
