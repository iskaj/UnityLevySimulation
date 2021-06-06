from scipy.stats import levy, norm
import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
import random
from scipy.stats import ks_2samp
# def scipy_levy():
#     fig, ax = plt.subplots(1, 1)
#     r = levy.rvs(loc=2, scale=1, size=1000)
#     new_r = []
#     for value in r:
#         if value <= 5000:
#             new_r.append(value)
#     r = new_r
#     ax.hist(r, density=True, histtype='stepfilled', alpha=0.2)
#     ax.legend(loc='best', frameon=False)
#     plt.show()


# def wikipedia_levy(U, mu, c):
#     normal = norm.cdf(mu, 0.0, c) # maybe (0.0, mu, c)
#     numerator = c
#     denominator = (normal**-1 * (1-U))**2
#     return numerator / denominator + mu

def wikipedia_levy(U, mu, c):
    normal = norm.ppf(1-U)**2
    return c / normal + mu

def scipy_levy(mu, c):
    return levy.rvs(loc=mu, scale=c, size=1)[0]


def get_levy(loc=2, scale=1, size=10000, truncate=5000, type='scipy'):
    values = []
    if type == 'wiki' or type == 'wikipedia':
        while len(values) < size:
            r = random.uniform(0, 1)
            value = wikipedia_levy(r, mu=loc, c=scale)
            if value <= truncate:
                values.append(value)
    elif type == 'scipy':
        while len(values) < size:
            value = scipy_levy(loc, scale)
            if value <= truncate:
                values.append(value)
    return values

# print(norm.ppf(0.25))
# print(norm.ppf(0.5))
# print(norm.ppf(0.75))

# Import data from unity
PATH_LEVY = r"D:\University Files\Natural Computing\zLevyTest_test\Assets\levy.txt"
with open(PATH_LEVY, "r", encoding='utf-8') as file:
    val_unity = []
    for line in file:
        val_unity.append(float(line[0:-2].replace(",", ".")))

loc = 2
scale = 1
iterations = 100000 # 100
truncate = 5000
bins = 5000 # 100
val_scipy = get_levy(loc, scale, iterations, truncate, 'scipy')
val_wiki = get_levy(loc, scale, iterations, truncate, 'wiki')
val_unity = val_unity[:iterations]
print(f"VALUES SCIPY of length {len(val_scipy)}")
print(f"VALUES WIKI of length {len(val_wiki)}")
print(f"VALUES UNITY of length {len(val_unity)}")
print(f"val_scipy - Mean: {np.mean(val_scipy)}, Median: {np.median(val_scipy)}")
print(f"val_wiki - Mean: {np.mean(val_wiki)}, Median: {np.median(val_wiki)}")
print(f"val_unity - Mean: {np.mean(val_unity)}, Median: {np.median(val_unity)}")

print(ks_2samp(val_scipy, val_wiki))
print(ks_2samp(val_scipy, val_unity))
print(ks_2samp(val_wiki, val_unity))

sns.set_style("darkgrid")
# fig = plt.figure(figsize=(10, 5))

# fig, (ax1, ax2, ax3) = plt.subplots(3, sharey=True)
# # ax1.title("Scipy.stats\nbased Lévy distribution")
# ax1 = sns.histplot(val_scipy, color='blue', bins=bins)
# ax1.axvline(np.mean(val_scipy), 0.0, 0.5, color='black', label='mean')
# ax1.axvline(np.median(val_scipy), 0.0, 0.5, color='gray', label='median')
# ax1.legend()
# # ax1.xlim(0, 100)
# # ax1.xlabel("Flight distance")
# print("scipy done")
#
# # ax2.title("Python inverse sampling \nbased Lévy distribution")
# ax2 = sns.histplot(val_wiki, color='red', bins=bins)
# ax2.axvline(np.mean(val_wiki), 0.0, 0.5, color='black', label='mean')
# ax2.axvline(np.median(val_wiki), 0.0, 0.5, color='gray', label='median')
# ax2.legend()
# # ax2.xlim(0, 100)
# # ax2.xlabel("Flight distance")
# print("wiki done")
#
# # ax3.title("Unity inverse sampling \nbased Lévy distribution")
# ax3 = sns.histplot(val_unity, color='green', bins=bins)
# ax3.axvline(np.mean(val_unity), 0.0, 0.5, color='black', label='mean')
# ax3.axvline(np.median(val_unity), 0.0, 0.5, color='gray', label='median')
# ax3.legend()
# # ax3.xlim(0, 100)
# # ax3.xlabel("Flight distance")
# print("unity done")
# plt.show()

fig = plt.figure(figsize=(10, 5))
ax1 = plt.subplot(1, 3, 1)
plt.title("Scipy.stats\nbased Lévy distribution")
sns.histplot(val_scipy, color='blue', bins=bins)
plt.axvline(np.mean(val_scipy), 0.0, 0.5, color='black', label='mean')
plt.axvline(np.median(val_scipy), 0.0, 0.5, color='gray', label='median')
plt.legend()
plt.xlim(0, 100)
plt.ylim(0, 35000)
# plt.ylim(0, 90)
plt.xlabel("Flight distance")
print("scipy done")

ax2 = plt.subplot(1, 3, 2)
plt.title("Python inverse sampling \nbased Lévy distribution")
sns.histplot(val_wiki, color='red', bins=bins)
plt.axvline(np.mean(val_wiki), 0.0, 0.5, color='black', label='mean')
plt.axvline(np.median(val_wiki), 0.0, 0.5, color='gray', label='median')
plt.legend()
plt.xlim(0, 100)
plt.ylim(0, 35000)
# plt.yticks([])
plt.ylabel("")
plt.xlabel("Flight distance")
print("wiki done")

plt.subplot(1, 3, 3)
plt.title("Unity inverse sampling \nbased Lévy distribution")
sns.histplot(val_unity, color='green', bins=bins)
plt.axvline(np.mean(val_unity), 0.0, 0.5, color='black', label='mean')
plt.axvline(np.median(val_unity), 0.0, 0.5, color='gray', label='median')
plt.legend()
plt.xlim(0, 100)
plt.ylim(0, 35000)
# plt.yticks([])
plt.ylabel("")
plt.xlabel("Flight distance")
print("unity done")

plt.show()


# sns.set_style("darkgrid")
# sns.histplot(val_scipy, label="scipy", color='blue', bins=bins)
# print("scipy done")
# sns.histplot(val_wiki, label="wikipedia", color='red', bins=bins)
# print("wiki done")
# sns.histplot(val_unity, label="unity", color='green', bins=bins)
# print("unity done")
#
# plt.axvline(np.mean(val_wiki), 0.0, 0.5, color='black', label='mean')
# plt.axvline(np.median(val_wiki), 0.0, 0.5, color='gray', label='median')
#
# plt.legend()
# plt.xlim(0, 100)
# plt.title("Histogram of Lévy distribution samples (loc=2, scale=1)")
# plt.xlabel("Flight distance")
# plt.show()
