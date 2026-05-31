using UnityEngine;

public class Door : MonoBehaviour
{
    public int NumbOfSwitches = 0;
    public int ActuatedSwitches = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NumbOfSwitches == ActuatedSwitches)
            Destroy(gameObject);
    }
}
