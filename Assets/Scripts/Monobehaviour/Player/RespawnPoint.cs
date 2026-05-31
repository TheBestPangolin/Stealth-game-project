using System.Linq;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] public int Priority;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_container.CurrentRespawn = transform.position;
            foreach(var point in 
                GameObject.FindGameObjectsWithTag(gameObject.tag)
                .Select(obj => obj.GetComponent<RespawnPoint>()))
                if (point.Priority < Priority)
                    Destroy(point.gameObject);
        }
    }
}
