using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LetPlayerInsideAHouse1 : MonoBehaviour
{
    List<GameObject> objToDelete = new List<GameObject>();
    void Start()
    {
        objToDelete = GameObject.FindGameObjectsWithTag("ToDeleteOnEnter").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var obj in objToDelete)
        {
            obj.SetActive(!obj.activeInHierarchy);
        }
    }
}
