## UnityLevySimulation
# A simulation environment to for Lévy foraging simulations.

These are the instructions to run the Lévy and random walker simulations as shown in the report. Note that for this to work you need to have Unity version 2019.4.25f1 and a compatible version of Unity Hub installed. All the statistics are gathered in standard .csv formats which can be directly imported into Python using pd.read_csv(filename). 

To run the levy simulation:
1. Start the "simulation project" with Unity version 2019.4.25f1.
2. Press the play button inside the Unity Editor. 
3. Let the simulation run for x amount of time, depending on how many samples you want to gather. The .csv files containing the statistics will be written to files in the "Statistics" folder of the project. Note that you can stop running and start running again without any problem.

To adjust the simulation environment in terms of agents:
1. Start the "simulation project" with Unity version 2019.4.25f1.
2. Select the "ForagerSpawner" object in the hierarchy.
3. Adjust the "Min Mu" and "Max Mu" float values in the "Levy Distribution Values" shown in the inspector to the values you want to investigate. Note that a wider range means more agents.
4. If necessary, the inspector now also shows "Simulation Settings" that you can adjust
5. If necessary, drag the "ForagerLevyNew" prefab from the "NC_Project" folder if you want to add extra agents with custom settings, for example if you want a random walker.

To adjust the simulation environment in terms of food distribution:
1. Start the "simulation project" with Unity version 2019.4.25f1.
2. Select the "FoodSourceGenerator" object in the hierarchy.
3. Adjust the "N_sources" integer value shown in the inspector.
4. Select the "FoodSource" object in the hierarchy.
5. Adjust the "Max_drop" integer value shown in the inspector.
