# Project: Predicting Surplus Energy for Renewable Projects

---

## Overview

This project aims to utilize historic climatic data to predict surplus energy for renewable energy projects. By exploring, analyzing, cleaning, and preprocessing the provided datasets, we aim to create an informed decision-making tool for stakeholders involved in renewable energy initiatives. This document outlines the approach, methodologies, and assumptions made during the project.

---

## Stage 1: Data Exploration

### Objective
The primary goal of this stage is to explore, analyze, clean, and preprocess the datasets to prepare them for subsequent modeling stages.

### Tools and Technologies Used
- Programming Language: Python 3.12
- Libraries: Pandas, OS

### Instructions for Running the Code
1. Open the provided Python file using any Python IDE or text editor.
2. Replace the placeholder "Dataset File Path" with the path to your dataset.
3. Execute the code in the IDE or compile it from the terminal using Python's interpreter.
4. Alternatively, upload the provided .ipynb file to your preferred JupyterLab or Jupyter Notebook platform.

### Key Assumptions
1. **Dataset Size:** It is assumed that the dataset for Brighton is not excessively large, allowing for exploration without creating subsets.
2. **Handling Missing Values:** Median and Mode are considered suitable options for completing missing values, as they maintain the data distribution without heavily skewing it.
3. **Data Splits:** Data splitting for training and testing was not implemented at this stage, as the focus was on exploration rather than modeling.

### Considerations for Surplus Energy Prediction
1. **Surplus Energy Threshold:** Determining a specific surplus energy threshold requires a comprehensive model incorporating various climatic variables and their relationship to energy production and consumption.
2. **Resource Adequacy:** The National Renewable Energy Laboratory (NREL) report discusses surplus power in the context of electrical system adequacy, but does not provide specific thresholds based on the variables listed in the dataset.

---

## References
1. [NREL Report: Resource Adequacy](https://www.nrel.gov/docs/fy21osti/79698.pdf)

---