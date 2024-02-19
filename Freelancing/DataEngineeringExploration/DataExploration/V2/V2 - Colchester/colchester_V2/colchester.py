import os
import pandas as pd
import matplotlib.pyplot as plt

# Define the directory containing the dataset
data_directory = r"C:\Users\anzda\OneDrive\Desktop\Freelance Work\weatherdata_for_students"

# Identify and collect filenames starting with "colchester"
file_paths = [os.path.join(data_directory, filename) for filename in os.listdir(data_directory) if filename.startswith("colchester")]

# Count the number of files utilized
num_files_used = len(file_paths)

# Load data from all files into a single DataFrame
dfs = [pd.read_csv(file_path) for file_path in file_paths]
full_df = pd.concat(dfs)

# Remove columns with numerical names
full_df.drop(full_df.filter(regex='^\d+$').columns, axis=1, inplace=True)

# Display basic information about the dataset
print("Number of files utilized:", num_files_used)
print("\nDataset Information:")
print(full_df.info())

# Display the first few rows of the dataset
print("\nFirst few rows of the dataset:")
print(full_df.head())

# Check for missing values
print("\nMissing Values:")
print(full_df.isnull().sum())

# Generate summary statistics
summary_stats = full_df.describe()

# Clean and preprocess the dataset

# Handle missing values for numerical columns by filling with the median
numerical_cols = full_df.select_dtypes(include=['float64']).columns
full_df[numerical_cols] = full_df[numerical_cols].fillna(full_df[numerical_cols].median())

# Handle missing values for categorical columns by filling with the mode
categorical_cols = full_df.select_dtypes(include=['object']).columns
full_df[categorical_cols] = full_df[categorical_cols].fillna(full_df[categorical_cols].mode().iloc[0])

# Check for missing values after handling
print("\nMissing Values after Handling:")
print(full_df.isnull().sum())

# Save the cleaned dataset
full_df.to_csv('cleaned_data.csv', index=False)

# Display updated summary statistics after cleaning
print("\nUpdated Summary Statistics:")
print(full_df.describe())

# Generate boxplots for each numerical column
plt.figure(figsize=(14, 8))

num_numerical_cols = len(numerical_cols)
num_rows = (num_numerical_cols // 3) + (1 if num_numerical_cols % 3 != 0 else 0)
for i, col in enumerate(numerical_cols, start=1):
	plt.subplot(num_rows, 3, i)
	plt.boxplot(full_df[col].dropna())
	plt.title(col)

plt.tight_layout()
plt.show()

# Plot histograms for each numerical column in separate windows
for col in numerical_cols:
	fig, ax = plt.subplots()
	ax.hist(full_df[col], bins=20, color='blue', alpha=0.7)
	ax.set_title(f'Distribution of {col}')
	ax.set_xlabel(col)
	ax.set_ylabel('Frequency')
	ax.grid(True)
	plt.show()

# Generate Comparative for each numerical column
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