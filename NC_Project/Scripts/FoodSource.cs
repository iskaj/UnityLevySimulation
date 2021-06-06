using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    public float spawn_time = 1.0f;
    public int max_drop = 5;
    public GameObject food;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(foodWave());
    }

    private void spawnFood()
    {
        if (gameObject.transform.childCount < max_drop)
        {
            GameObject a = Instantiate(food) as GameObject;
            a.transform.parent = gameObject.transform;
            a.transform.position = gameObject.transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        }
    }

    IEnumerator foodWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawn_time-0.5f, spawn_time+0.5f));
            spawnFood();
        }
    }
}
