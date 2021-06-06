using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForagerSpawner : MonoBehaviour
{
    [Header("Levy Distribution Values")]
    public GameObject foragerPrefab;
    public float minMu= 1.0f;
    public float maxMu = 3.0f;
    List<float> mus = new List<float>();
    public float scale = 1f;

    [Header("Agent Values")]
    public float speed = 5.0f;
    public float vision_radius = 1f;

    [Header("Simulation Settings")]
    public float maxSimulationTime = 300f;
    public float timeScale = 1f;
    public bool writeStatToFile = false;
    public bool useRandomDistInstead = false;
    public string fileName = "something";
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
        // Get range of mu values
        float curFloat = minMu;
        while (curFloat <= maxMu)
        {
            curFloat = Mathf.Round(curFloat * 10) / 10;
            mus.Add(curFloat);
            Debug.Log("MU: " + curFloat.ToString());
            curFloat += 0.1f;
        }
        // Spawn foragers with correct values
        foreach (float m in mus)
        {
            var forager = Instantiate(foragerPrefab, transform.position, Quaternion.identity);
            var foragerScript = forager.GetComponent<Forager_Sim>();
            foragerScript.mean = m;
            foragerScript.speed = speed;
            foragerScript.vision_radius = vision_radius;
            foragerScript.maxSimulationTime = maxSimulationTime;
            foragerScript.scale = scale;
            foragerScript.writeStatToFile = writeStatToFile;
            foragerScript.useRandomDistInstead = useRandomDistInstead;
            foragerScript.fileName = fileName;
        }
    }

}
