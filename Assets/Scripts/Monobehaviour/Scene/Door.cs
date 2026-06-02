using UnityEngine;

public class Door : MonoBehaviour
{
    public int NumbOfSwitches = 0;
    public int ActuatedSwitches = 0;
    [SerializeField] public EnemyLogic[] Enemies;

    [SerializeField] public GameObject InteractableOnDestroy;
    bool iscreated;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (NumbOfSwitches == ActuatedSwitches)
        {
            foreach (EnemyLogic enemy in Enemies)
                Destroy(enemy.gameObject);
            if (InteractableOnDestroy == default)
                Destroy(gameObject);
            else if (!iscreated)
            {
                Instantiate(InteractableOnDestroy);
                iscreated = true;
            }
        }
    }
}

