import numpy as np
import pandas as pd
import os
import seaborn as sns
import matplotlib.pyplot as plt

# Batch 1 was 278 files
# So take the last (longest_file -448) files

def mean_filenames(filename, means):
    """"Get the filenames corresponding to all the different mu values"""
    return [filename + "(mu=" + str(round(mean, 2)) + ", loc=1.0).csv" for mean in all_means]

def mu_names(filename, means):
    mus = []
    for fn in mean_filenames(filename, all_means):
        path = STAT_PATH + fn
        if os.path.exists(path):
            mus.append(fn[len(filename)+1:len(filename)+7])
    return mus

def create_df(filename):
    """
    Create the dataframe of all the statistics for a given filename, for example for all the
    Levy statistics use "Levy" as the filename
    :param filename:
    :return:
    """
    column_dict = []
    for fn in mean_filenames(filename, all_means):
        path = STAT_PATH + fn
        if os.path.exists(path):
            df = pd.read_csv(path)
            print(len(df.consumedFoodCount))

            # df = df[-180:]
            # print(len(df.consumedFoodCount))
            df_column_name = fn[len(filename)+1:len(filename)+7]
            column_dict.append({"Mu": float(fn[len(filename)+4:len(filename)+7]),
                                "Average Food Consumed": np.mean(df['consumedFoodCount']),
                                "Average Flight Distance": np.mean(df['distanceTraversed']),
                                "Average Search Efficiency": np.mean(df['searchEfficiency'])})
    return pd.DataFrame(column_dict)

STAT_PATH = r"D:\University Files\Natural Computing\NEAT_V2_Release\Assets\Statistics\\"
sns.set_style('darkgrid')
# df = pd.read_csv("Levy(mu=2.0, loc=1.0).csv")
# print(df.keys())
all_means = np.arange(0.0, 3.1, step=0.1)
# print(f"Number of means: {len(all_means)} \n {all_means}")
filename = "cluster_3"
df = create_df(filename)
print(df)
graph = sns.lineplot(x=df['Mu'], y=df['Average Search Efficiency'])
sns.regplot(x=df['Mu'], y=df['Average Search Efficiency'],
                 order=2, marker=".") # label="Fitted second order polynomial"
# sns.regplot(x=df['Mu'], y=df['Average Search Efficiency'])
# plt.ylim(0.223, 0.233)
plt.ylim(0.215, 0.24)
plt.title("Average search efficiency for different means \n of the Lévy distribution in the clustered simulation environment")

# Plot performance of random walker
random_mean = np.mean(pd.read_csv(filename + "_random.csv")["searchEfficiency"])
print(random_mean)
graph.axhline(random_mean, label="Random Walker", linestyle="--", color="black")
plt.legend()
plt.show()



# sns.regplot(x=df['Mu'], y=df['Average Search Efficiency'], data=df['Average Search Efficiency'],
#                  scatter_kws={"s": 80},
#                  order=2, ci=None)

# idxmmax = df['Average Search Efficiency'].idxmax()
# plt.vlines(df['Mu'][idxmmax], 0.125, max(df['Average Search Efficiency']))
# plt.plot(df['Average Search Efficiency'], "x-")




# fig, axs = plt.subplots(3, sharex=True)
# fig.suptitle('Vertically stacked subplots')
# sns.lineplot(x=df['Mu'], y=df['Average Food Consumed'], ax=axs[0])
# plt.ylabel('Average Food\nConsumed')
#
# sns.lineplot(x=df['Mu'], y=df['Average Flight Distance'], ax=axs[1])
# plt.ylabel('Average Flight\nDistance')
#
# sns.lineplot(x=df['Mu'], y=df['Average Search Efficiency'], ax=axs[2])
# plt.ylabel('Average Search\nEfficiency')
#
# plt.show()
