using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodRespawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit(Collider collider)
    {
        List<int> consumed_food = transform.parent.GetComponent<Forager_Sim>().consumed_food;
        if (collider.gameObject.tag == "Food" && consumed_food.Contains(collider.gameObject.GetInstanceID()))
        {
            consumed_food.Remove(collider.gameObject.GetInstanceID());
        }
    }
}
