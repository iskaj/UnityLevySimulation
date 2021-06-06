# UnityLevySimulation
## A simulation environment to for Lévy foraging simulations.

These are the instructions to run the Lévy and random walker simulations as shown in the report. Note that for this to work you need to have Unity version 2019.4.25f1 and a compatible version of Unity Hub installed. All the statistics are gathered in standard .csv formats which can be directly imported into Python using pd.read_csv(filename). 

To boot up the project
1. Clone the Github Repo
2. Start Unity Hub
3. Click "add" and navigate to the project folder called "Simulation_Environment"
4. Start the "Simulation_Environment" project with Unity version 2019.4.25f1.

To run the levy simulation:
1. Press the play button inside the Unity Editor. 
2. Let the simulation run for x amount of time, depending on how many samples you want to gather. The .csv files containing the statistics will be written to files in the "Statistics" folder of the project. Note that you can stop running and start running again without any problem.

To adjust the simulation environment in terms of agents:
1. Select the "ForagerSpawner" object in the hierarchy.
2. Adjust the "Min Mu" and "Max Mu" float values in the "Levy Distribution Values" shown in the inspector to the values you want to investigate. Note that a wider range means more agents.
3. If necessary, the inspector now also shows "Simulation Settings" that you can adjust
4. If necessary, drag the "ForagerLevyNew" prefab from the "NC_Project" folder if you want to add extra agents with custom settings, for example if you want a random walker.

To adjust the simulation environment in terms of food distribution:
1. Select the "FoodSourceGenerator" object in the hierarchy.
2. Adjust the "N_sources" integer value shown in the inspector.
3. Select the "FoodSource" object in the hierarchy.
4. Adjust the "Max_drop" integer value shown in the inspector.
