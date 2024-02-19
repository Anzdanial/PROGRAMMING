import matplotlib.pyplot as plt
from matplotlib.widgets import Button
from sklearn.preprocessing import MinMaxScaler
import os
import pandas as pd

# Directory where the dataset is located
data_dir = r"C:\Anas's Data\Freelancing Projects\Data Engineering\Project documentation-20240213\weatherdata_for_students"

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
full_df.drop(full_df.filter(regex=r'^\d+$').columns, axis=1, inplace=True)

# Display basic information about the dataset
print("Number of files opened/used:", num_files_used)
print("\nDataset Info:")
print(full_df.info())

# Display first few rows of the dataset
print("\nFirst few rows of the dataset:")
print(full_df.head())

# Check for missing values
print("\nMissing values:")
print(full_df.isnull().sum())

summary_stats = full_df.describe()

# Display mean, median, mode, max, and min for each column
print("\nSummary Statistics:")
for col in full_df.columns:
	print(f"\nColumn: {col}")
	if col != 'datetime' and col != 'preciptype' and full_df[col].notna().any():  # Skip summary statistics for 'datetime' and 'preciptype' columns, and all null columns
		print(f"Mean: {summary_stats.loc['mean', col]}")
		print(f"Median: {summary_stats.loc['50%', col]}")
		print(f"Mode: {full_df[col].mode().iloc[0]}")
		print(f"Max: {summary_stats.loc['max', col]}")
		print(f"Min: {summary_stats.loc['min', col]}")

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

# Check for missing values after handling
print("\nMissing values after handling:")
print(full_df.isnull().sum())

# Save the cleaned dataset
full_df.to_csv('cleaned_data.csv', index=False)

summary_stats = full_df.describe()
# Display mean, median, mode, max, and min for each column
print("\nSummary Statistics:")
for col in full_df.columns:
	print(f"\nColumn: {col}")
	if col != 'datetime' and col != 'preciptype' and full_df[col].notna().any():  # Skip summary statistics for 'datetime' and 'preciptype' columns, and all null columns
		print(f"Mean: {summary_stats.loc['mean', col]}")
		print(f"Median: {summary_stats.loc['50%', col]}")
		print(f"Mode: {full_df[col].mode().iloc[0]}")
		print(f"Max: {summary_stats.loc['max', col]}")
		print(f"Min: {summary_stats.loc['min', col]}")

# Convert the 'datetime' column to datetime format
full_df['datetime'] = pd.to_datetime(full_df['datetime'])

# Sort the DataFrame by datetime
full_df.sort_values(by='datetime', inplace=True)

# Plotting line graphs for each numerical column in separate windows
numerical_cols = full_df.select_dtypes(include=['float64']).columns

for col in numerical_cols:
	fig, ax = plt.subplots()
	ax.plot(full_df['datetime'], full_df[col], color='blue')
	ax.set_title(f'{col} Over Time in Brighton')
	ax.set_xlabel('Date')
	ax.set_ylabel(col)
	ax.grid(True)
	plt.show()


for i, col1 in enumerate(numerical_cols):
	for j, col2 in enumerate(numerical_cols):
		if i < j:  # To avoid duplicate plots and self-correlations
			fig, ax = plt.subplots()
			ax.scatter(full_df[col1], full_df[col2], color='blue')
			ax.set_title(f'{col1} vs {col2} Scatter Plot')
			ax.set_xlabel(col1)
			ax.set_ylabel(col2)
			ax.grid(True)
			plt.show()


# Perform Min-Max normalization on numerical columns
scaler = MinMaxScaler()
normalized_data = scaler.fit_transform(full_df.select_dtypes(include=['float64']))
normalized_df = pd.DataFrame(normalized_data, columns=full_df.select_dtypes(include=['float64']).columns)

# Plot histograms for each normalized numerical column in separate windows
for col in normalized_df.columns:
	fig, ax = plt.subplots()
	ax.hist(normalized_df[col], bins=20, color='blue', alpha=0.7)
	ax.set_title(f'Normalized Distribution of {col}')
	ax.set_xlabel('Normalized Value')
	ax.set_ylabel('Frequency')
	ax.grid(True)
	plt.show()