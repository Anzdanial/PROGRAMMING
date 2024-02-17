import matplotlib.pyplot as plt
from matplotlib.widgets import Button
from sklearn.preprocessing import MinMaxScaler
import os
import pandas as pd

# Directory where the dataset is located
data_dir = r"C:\Users\anzda\OneDrive\Desktop\Freelance Work\Data Engineering\weatherdata_for_students"

# Note: Opening and Using all files since the dataset for Brighton is small.
# List all files in the directory that start with "brighton"
file_names = [os.path.join(data_dir, f) for f in os.listdir(data_dir) if f.startswith("brighton")]

# Count the number of files opened/used
num_files_used = len(file_names)

# Load all files into a single DataFrame
dfs = []
for file_name in file_names:
	df = pd.read_csv(file_name)
	dfs.append(df)

full_df = pd.concat(dfs)

# Drop columns with numbered names
full_df.drop(full_df.filter(regex='^\d+$').columns, axis=1, inplace=True)

# Display basic information about the dataset
print("Number of files opened/used:", num_files_used)
print("\nDataset Info:")
print(full_df.info())

# Display first few rows of the dataset
print("\nFirst few rows of the dataset:")
print(full_df.head())

# # Check for missing values
# print("\nMissing values:")
# print(full_df.isnull().sum())
#
# summary_stats = full_df.describe()
#
# # Display mean, median, mode, max, and min for each column
# print("\nSummary Statistics:")
# for col in full_df.columns:
# 	print(f"\nColumn: {col}")
# 	if col != 'datetime' and col != 'preciptype' and full_df[col].notna().any():  # Skip summary statistics for 'datetime' and 'preciptype' columns, and all null columns
# 		print(f"Mean: {summary_stats.loc['mean', col]}")
# 		print(f"Median: {summary_stats.loc['50%', col]}")
# 		print(f"Mode: {full_df[col].mode().iloc[0]}")
# 		print(f"Max: {summary_stats.loc['max', col]}")
# 		print(f"Min: {summary_stats.loc['min', col]}")

# Clean and preprocess the data set

# Handling missing values for numerical columns
# You can choose to fill missing numerical values with mean or median
# Here, we'll fill missing values with the median
numerical_cols = full_df.select_dtypes(include=['float64']).columns
full_df[numerical_cols] = full_df[numerical_cols].fillna(full_df[numerical_cols].median())

# Handling missing values for categorical columns
# You can replace missing categorical values with a placeholder or mode
# Here, we'll fill missing categorical values with the mode
categorical_cols = full_df.select_dtypes(include=['object']).columns
full_df[categorical_cols] = full_df[categorical_cols].fillna(full_df[categorical_cols].mode().iloc[0])

# # Check for missing values after handling
# print("\nMissing values after handling:")
# print(full_df.isnull().sum())
#
# # Save the cleaned dataset
# full_df.to_csv('cleaned_data.csv', index=False)
#
# summary_stats = full_df.describe()
# # Display mean, median, mode, max, and min for each column
# print("\nSummary Statistics:")
# for col in full_df.columns:
# 	print(f"\nColumn: {col}")
# 	if col != 'datetime' and col != 'preciptype' and full_df[col].notna().any():  # Skip summary statistics for 'datetime' and 'preciptype' columns, and all null columns
# 		print(f"Mean: {summary_stats.loc['mean', col]}")
# 		print(f"Median: {summary_stats.loc['50%', col]}")
# 		print(f"Mode: {full_df[col].mode().iloc[0]}")
# 		print(f"Max: {summary_stats.loc['max', col]}")
# 		print(f"Min: {summary_stats.loc['min', col]}")

# # Get the column names
# numerical_cols = summary_stats.columns
#
# # Plot histograms for each numerical column with summary statistics
# plt.figure(figsize=(14, 8))
#
# for col in numerical_cols:
# 	plt.hist(full_df[col], bins=20, alpha=0.5, label=col)
# 	plt.axvline(x=summary_stats.loc['max', col], color='red', linestyle='--', linewidth=1)
# 	plt.axvline(x=summary_stats.loc['min', col], color='blue', linestyle='--', linewidth=1)
# 	plt.axvline(x=summary_stats.loc['50%', col], color='green', linestyle='--', linewidth=1)
# 	plt.axvline(x=summary_stats.loc['mean', col], color='orange', linestyle='--', linewidth=1)
# 	plt.axvline(x=full_df[col].mode().iloc[0], color='purple', linestyle='--', linewidth=1)
#
# plt.xlabel('Value')
# plt.ylabel('Frequency')
# plt.title('Histograms with Summary Statistics')
# plt.legend()
# plt.grid(True)
# plt.tight_layout()
# plt.show()

# Convert the 'datetime' column to datetime format
full_df['datetime'] = pd.to_datetime(full_df['datetime'])

# Sort the DataFrame by datetime
full_df.sort_values(by='datetime', inplace=True)

# Plotting line graphs for each numerical column
numerical_cols = full_df.select_dtypes(include=['float64']).columns

fig, ax = plt.subplots()
plt.subplots_adjust(bottom=0.2)  # Adjust the bottom margin to make room for buttons

# Initial plot
current_col_idx = 0
line, = ax.plot(full_df['datetime'], full_df[numerical_cols[current_col_idx]], color='blue')
ax.set_title(f'{numerical_cols[current_col_idx]} Over Time in Brighton')
ax.set_xlabel('Date')
ax.set_ylabel(numerical_cols[current_col_idx])
ax.grid(True)

# Function to handle the "Next" button click event
def next_plot(event):
	global current_col_idx
	current_col_idx = (current_col_idx + 1) % len(numerical_cols)
	update_plot()

# Function to handle the "Previous" button click event
def prev_plot(event):
	global current_col_idx
	current_col_idx = (current_col_idx - 1) % len(numerical_cols)
	update_plot()

# Function to update the plot
def update_plot():
	ax.clear()
	ax.plot(full_df['datetime'], full_df[numerical_cols[current_col_idx]], color='blue')
	ax.set_title(f'{numerical_cols[current_col_idx]} Over Time in Brighton')
	ax.set_xlabel('Date')
	ax.set_ylabel(numerical_cols[current_col_idx])
	ax.grid(True)
	fig.canvas.draw()

# Create "Next" button
axnext = plt.axes([0.81, 0.05, 0.1, 0.075])
bnext = Button(axnext, 'Next')
bnext.on_clicked(next_plot)

# Create "Previous" button
axprev = plt.axes([0.7, 0.05, 0.1, 0.075])
bprev = Button(axprev, 'Previous')
bprev.on_clicked(prev_plot)

plt.show()
