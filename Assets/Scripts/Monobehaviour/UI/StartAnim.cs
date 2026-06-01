using UnityEngine;

public class StartAnim : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetComponent<Animator>().Play("Animation_Main");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
