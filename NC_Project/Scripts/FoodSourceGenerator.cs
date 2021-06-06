using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSourceGenerator : MonoBehaviour
{
    public int n_sources = 5;
    public int seed = 42;
    public GameObject foodSource;
    public float range = 500f;
    public bool randomSeedOnStartup = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!randomSeedOnStartup) Random.InitState(seed);
        for (int i = 0; i < n_sources; i++)
        {
            GameObject a = Instantiate(foodSource) as GameObject;
            a.transform.parent = gameObject.transform;
            a.transform.position = gameObject.transform.position + new Vector3(Random.Range(-range, range), 0.15f, Random.Range(-range, range));
            a.SetActive(true);
        }
    }
}
