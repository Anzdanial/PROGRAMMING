# README
- The following project and supplementary materials provide details that elaborate on the provided datasets containing historic records of climatic information to create an informed decision regarding the prediction of surplus energy for renewable projects.

## STAGE 1: DATA EXPLORATION

- This stage targets the Data Exploration, Analysis, Cleaning and Preprocessing of the datasets provided.
- All of these actions were performed in Python using the latest version currently available (Python 3.12).
- Furthermore, additional libraries such as Pandas and OS.


### HOW TO USE/RUN THE CODE:

- Open the given python file using a Python IDE.
    - An editor can also be used, but requires separate compilation using python's interpreter from the terminal.
- Upon opening the file, the user just needs to replace the Dataset File Path with their own path.
- Now just run the code in the IDE (or Compile from Terminal and Run) and the results should be available.
- You could also upload your .ipynb to your JupyterLab or Jupyter Notebook running platforms.


### ASSUMPTIONS:

- I have assumed that the size of the dataset for Brighton isn't highly large which is why I have traversed and explored all files without creating subsets.
- Based on the traversal and results outputted from my code, I believe that Median and Mode are the best options to complete the missing values as they would not disturb the overall distribution of data and would not heavily skew the distribution unlike Mean which would support or make it difficult to highlight outliers.
- I have also not added Data Splits as I did not delve into the secondary portion of Data Modelling and I did not understand the model needed to properly split the data into testing and training.

- The adequate temperature for a surplus of energy, specifically in the context of Earth's climate system, is not a fixed temperature but rather a balance between incoming solar radiation and outgoing thermal infrared radiation.
- To establish thresholds for surplus energy based on the provided variables, one would need to develop a model that incorporates these variables and their relationships to energy production and consumption. This would require a comprehensive understanding of the energy system and the specific context in which the thresholds are being established.
- The National Renewable Energy Laboratory (NREL) report discusses surplus power in the context of electrical power system adequacy, but it does not provide specific thresholds for surplus energy based on the variables listed. Instead, it focuses on the concept of resource adequacy and the calculation of capacity credit for individual resources.


#### REFERENCES
https://earthobservatory.nasa.gov/features/EnergyBalance
https://www.nrel.gov/docs/fy21osti/79698.pdf
https://www.aceee.org/sites/default/files/pdfs/u2006.pdf