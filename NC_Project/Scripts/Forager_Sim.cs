using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Forager_Sim : MonoBehaviour //, IAgentTester
{
    /// <summary>
    /// Event subscriptions to notify controller when test is finished
    /// </summary>
    /// <param name="source">Source of the event (this)</param>
    /// <param name="args">Nothing</param>
    //public delegate void TestFinishedEventHandler(object source, EventArgs args);
    //public event TestFinishedEventHandler TestFinished;

    //private bool isActive = false; // is this agent active
    //private bool finished = false; // is this agent finished.  Making sure only 1 event is sent.

    //private NEATNet net; //The brain

    //private const string ACTION_ON_FINISHED = "OnFinished"; //On finished method

    //private NEATGeneticControllerV2 controller; //Controller

    public List<GameObject> visible_food;
    public bool moving = false;
    public float fitness = 0f;
    public List<int> consumed_food = new List<int>();
    //public float t_last_food = 0f;
    public float move_time_left = 0f;
    public float x_limit = 50f;
    public float z_limit = 50f;
    public Vector2 direction;
    public bool see_food;

    public GameObject[] food;
    
    [Header("Agent Values")]
    public float speed = 5.0f;
    public float vision_radius = 1f;
    private Color originalColor;

    [Header("Levy Distribution Values")]
    public bool useRandomDistInstead = false;
    public float mean = 2f;
    public float scale = 1f;
    public float distCutoffPoint = 5000f;

    [Header("Simulation Settings")]
    public float maxSimulationTime = 300f;
    public bool writeStatToFile = false;
    public string fileName = "stats";

    [Header("Flight Statistics")]
    [SerializeField] private int consumedFoodCount = 0;
    [SerializeField] private float distanceTraversed = 0f;
    [SerializeField] private float searchEfficiency = 0f;

    // Add this to be able to call Levy functions
    private Levy levy = new Levy();

    /// <summary>
    /// Set Color to this agent. Looks visually pleasing and may help in debugging? 
    /// </summary>
    /// <param name="color"> color</param>
    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
        GetComponent<TrailRenderer>().startColor = color;
        GetComponent<TrailRenderer>().endColor = color;
    }

    private void Start()
    {
        originalColor = UnityEngine.Random.ColorHSV();
        SetColor(originalColor);
    }

    void writeStatisticsAndRestartSim()
    {
        if (Time.timeSinceLevelLoad > maxSimulationTime)
        {
            // END THE GAME 
            if (writeStatToFile)
            {
                if (!useRandomDistInstead)
                { fileName = fileName + "(mu=" + mean.ToString("f1").Replace(',', '.') + ", loc=" + scale.ToString("f1").Replace(',', '.') + ").csv"; }
                // If there is no file yet, then we need a header for statistics, make it and add it
                if (!System.IO.File.Exists("Assets/Statistics/" + fileName))
                {
                    System.IO.StreamWriter oneLineWriter = new StreamWriter("Assets/Statistics/" + fileName, true); //True for append, false for overwrite                                                                          //writer.WriteLine("consumedFoodCount,distanceTraversed,searchEfficiency");
                    oneLineWriter.WriteLine("consumedFoodCount,distanceTraversed,searchEfficiency,agentSpeed");
                    oneLineWriter.Close();
                }
                // Write statistics line per line 
                StreamWriter writer = new StreamWriter("Assets/Statistics/" + fileName, true); //True for append, false for overwrite
                writer.WriteLine(
                    consumedFoodCount.ToString() + "," +
                    distanceTraversed.ToString().Replace(',', '.') + "," +
                    searchEfficiency.ToString().Replace(',', '.') + "," +
                    speed.ToString().Replace(',', '.')
                    );
                writer.Close();
                print("  - DONE WRITING TO FILE -  ");
            }
            writeStatToFile = false; // TEMPORARY!
            // RESTART CURRENT SCENE
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("--- EPISODE HAS ENDED ---");
        }
    }

    void FixedUpdate()
    {
        // Write statistics to file
        writeStatisticsAndRestartSim();

        // Change color to white when rotating/not moving
        if (!moving) SetColor(Color.white);
        else SetColor(originalColor);

        if (!moving) //Get random direction and initiate moving
        {
            // Generate destination for distributions
            direction = UnityEngine.Random.insideUnitCircle.normalized;
            if (useRandomDistInstead) move_time_left = distCutoffPoint;
            else move_time_left = levy.GetLevyCutoff(mean, scale, distCutoffPoint) / speed;

            // Generate destination for NEAT
            //move_time_left = output[0];

            // Set movement parameters and move 
            moving = true;
            float step = speed * Time.fixedDeltaTime; // calculate distance to move
            MoveAgent(step);
        }
        else if (moving)
        {
            // Check if there is food in 1 unit distance away and get that food's position
            food = GameObject.FindGameObjectsWithTag("Food");
            Vector3 foodPos = CheckIfFoodInVisionRadius(food);
            if (foodPos != new Vector3(0f, 0f, 0f))
            {
                //Just a public debugging variable doesn't do anything
                see_food = true;

                //Recalculate direction
                Vector3 newDir = (foodPos - transform.position).normalized;
                direction = new Vector2(newDir.x, newDir.z);

                // Move there for as long as you want
                move_time_left = distCutoffPoint; // Max flight distance cut off

                // Set movement paramaters
                moving = true;
                // Draw debug line to show when you found food
                Debug.DrawLine(transform.position, transform.position + new Vector3(direction.x, 0, direction.y) * 5f, Color.blue, 0f);
            }
            else see_food = false;

            //Actually move the agent when moving
            float step = speed * Time.fixedDeltaTime;
            MoveAgent(step);

            // Stop moving if flight distance is below or equal to 0
            if (move_time_left <= 0f)
            {
                moving = false;
            }
            
        }
        searchEfficiency = consumedFoodCount / distanceTraversed;
        Debug.DrawRay(transform.position, new Vector3(direction.x * 5f, transform.position.y, direction.y * 5f), GetComponent<Renderer>().material.color);

    }

    // Returns the closest food's position if there is a valid non-consumed food 1 unit range away
    public Vector3 CheckIfFoodInVisionRadius(GameObject[] allFood)
    {
        float distance = Mathf.Infinity;
        Vector3 bestFoodPosition = new Vector3(0f, 0f, 0f);
        foreach (GameObject food in allFood)
        {
            if (!consumed_food.Contains(food.GetInstanceID()))
            {
                // Get positions and set y's to 0 just in case
                Vector3 agentPosition = transform.position;
                agentPosition.y = 0f;
                Vector3 foodPosition = food.transform.position;
                foodPosition.y = 0f;

                // If distance is better and food is actually in range
                float distFoodBird = Vector3.Distance(agentPosition, foodPosition);
                if (distFoodBird < distance && distFoodBird < vision_radius)
                {
                    // Update best food position
                    distance = distFoodBird;
                    bestFoodPosition = food.transform.position;
                }
            }
        }
        if (bestFoodPosition != new Vector3(0f, 0f, 0f))
        {
            // If there is food draw a line to the food.
            Debug.DrawLine(transform.position, bestFoodPosition, new Color(1f, 0f, 0f, 0.5f), 0f);
        }
        return bestFoodPosition;
    }

    private void MoveAgent(float step)
    {
        // Double normalize, can't harm and something Unity did not normalize correctly
        direction = direction.normalized;
        if (direction.magnitude > 1.0001f || direction.magnitude < 0.9999f) Debug.Log("SHOULD BE NORMALIZED WHAT!");

        // Do actual movement
        distanceTraversed += step;
        transform.position += new Vector3(direction.x * step, 0f, direction.y * step);

        // Torus Wrapping
        if (transform.position.x > x_limit)
            transform.position = new Vector3(-x_limit, transform.position.y, transform.position.z);
        if (transform.position.x < -x_limit)
            transform.position = new Vector3(x_limit, transform.position.y, transform.position.z);
        if (transform.position.z > z_limit)
            transform.position = new Vector3(transform.position.x, transform.position.y, -z_limit);
        if (transform.position.z < -z_limit)
            transform.position = new Vector3(transform.position.x, transform.position.y, z_limit);
        move_time_left -= Time.deltaTime;

        // Rotate agent towards the direction it is going
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, new Vector3(direction.x, 0f, direction.y), 1000f, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    // If the collider hits something
    void OnTriggerEnter(Collider collider)
    {
        GameObject go = collider.gameObject; ;
        if (go.tag == "Food" && !consumed_food.Contains(go.GetInstanceID()))
        {
            consumed_food.Add(go.GetInstanceID());
            consumedFoodCount++;
            fitness += 100f;
            moving = false;
            //Debug.Log("Adding " + go.GetInstanceID().ToString() + " to consumed food");
        }
    }

    // If the collider hits something and stays within it
    private void OnTriggerStay(Collider collider)
    {
        GameObject go = collider.gameObject; ; 
        if (go.tag == "Food" && !consumed_food.Contains(go.GetInstanceID()))
        {
            consumed_food.Add(go.GetInstanceID());
            consumedFoodCount++;
            fitness += 100f;
            moving = false;
            //Debug.Log("Adding " + go.GetInstanceID().ToString() + " to consumed food");
        }
    }
    // When you exit the collider do the following
    void OnTriggerExit(Collider collider)
    {
        GameObject go = collider.gameObject;
        if (go.tag == "Food" && consumed_food.Contains(go.GetInstanceID()))
        {
            StartCoroutine(AddFoodAfterTime(3f/speed, go));
        }
    }

    IEnumerator AddFoodAfterTime(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        consumed_food.Remove(go.GetInstanceID());
        //Debug.Log("Removing " + go.GetInstanceID().ToString() + " from consumed food");

    }


}
