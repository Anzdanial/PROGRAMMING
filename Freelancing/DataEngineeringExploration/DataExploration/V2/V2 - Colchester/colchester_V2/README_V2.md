User Guide
This document and accompanying materials offer comprehensive insights into the provided datasets containing historical climatic information. The goal is to facilitate informed decision-making regarding the prediction of surplus energy for renewable projects.
Phase 1: Data Exploration
This phase encompasses tasks such as Data Exploration, Analysis, Cleaning, and Preprocessing of the provided datasets.
All these tasks were executed using Python, leveraging the latest version available at the time (Python 3.12).
Additionally, various libraries including Pandas and OS were utilized.
Instructions for Implementation:
Open the provided Python file using a Python Integrated Development Environment (IDE).
Alternatively, you can use a text editor, but it requires separate compilation using Python's interpreter from the terminal.
After opening the file, simply replace the Dataset File Path with your own path.
Proceed to run the code within the IDE (or Compile from Terminal and Run), and the results will be generated.
Alternatively, you can upload your .ipynb file to JupyterLab or Jupyter Notebook platforms.
Assumptions Made:
It is assumed that the dataset size for Colchester is not excessively large, hence all files were traversed and explored without creating subsets.

Based on the exploration results, Median and Mode were deemed suitable for filling in missing values as they maintain the overall data distribution without heavily skewing it, unlike Mean which could affect outlier detection.

Data Splits were not implemented as the focus was not on the secondary phase of Data Modeling, and the specific model required for proper data partitioning was not identified.

The optimal temperature for surplus energy, within Earth's climate system, is not fixed but rather depends on a balance between incoming solar radiation and outgoing thermal infrared radiation.

Establishing surplus energy thresholds based on provided variables would necessitate developing a model that integrates these variables and their relationships with energy production and consumption. This requires a comprehensive understanding of the energy system and the specific context in which these thresholds are defined.

While the National Renewable Energy Laboratory (NREL) report addresses surplus power concerning electrical power system adequacy, it does not offer specific thresholds based on the listed variables. Instead, it focuses on resource adequacy and calculating capacity credit for individual resources.

References
NASA Earth Observatory - Energy Balance
ScienceDirect Article on Surplus Energy
ResearchGate Figure on Cumulative Surplus Energy