using UnityEngine;

public class StartSequence : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("DynamicEnemy"))
        {
            obj.transform.localScale = new Vector3(5, 5, 5);
        }
        foreach (var obj in GameObject.FindGameObjectsWithTag("StaticEnemy"))
        {
            obj.transform.localScale = new Vector3(5, 5, 5);
        }
        GameObject.FindGameObjectWithTag("Player").transform.localScale = new Vector3(5, 5, 5);
    }
}
