using System;
using System.Collections;
using UnityEngine;

public class TimerObj : MonoBehaviour
{
    public Action OnElapsed;
    public float Delay;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(Delay);
        OnElapsed();
        Destroy(gameObject);
    }
}
