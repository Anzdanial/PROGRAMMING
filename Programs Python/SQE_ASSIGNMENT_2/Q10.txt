import csv
import matplotlib.pyplot as plt
from collections import Counter

# Lists to store data
ids = []
issue_types = []
statuses = []
resolutions = []
components = []
severities = []
reporters = []
created_dates = []
updated_dates = []
assignees = []
resolved = []

# Reading data from the CSV file
with open('dataset3.csv', mode='r', encoding='latin1') as file:
    reader = csv.DictReader(file)
    for row in reader:
        row_components = row['component'].strip().split(';')  # Split the components if they are separated by ';'
        for component in row_components:
            if not component:  # Check if the component is empty
                component = 'Empty'
            ids.append(row['issue_id'].strip())
            issue_types.append(row['type'].strip())
            statuses.append(row['status'].strip())
            resolutions.append(row['resolution'].strip())
            components.append(component.strip())
            severities.append(row['priority'].strip())
            reporters.append(row['reporter'].strip())
            created_dates.append(row['created'].strip())
            updated_dates.append(row['assigned'].strip())
            assignees.append(row['assignee'].strip())
            resolved.append(row['resolved'].strip())



idsA = []
issue_typesA = []
statusesA = []
resolutionsA = []
componentsA = []
severitiesA = []
reportersA = []
created_datesA = []
updated_datesA = []
assigneesA = []
resolvedA = []

# Reading data from the CSV file
with open('dataset3AModules.csv', mode='r', encoding='latin1') as file:
    reader = csv.DictReader(file)
    for row in reader:
        row_components = row['component'].strip().split(';')  # Split the components if they are separated by ';'
        for component in row_components:
            if not component:  # Check if the component is empty
                component = 'Empty'
            idsA.append(row['issue_id'].strip())
            issue_typesA.append(row['type'].strip())
            statusesA.append(row['status'].strip())
            resolutionsA.append(row['resolution'].strip())
            componentsA.append(component.strip())
            severitiesA.append(row['priority'].strip())
            reportersA.append(row['reporter'].strip())
            created_datesA.append(row['created'].strip())
            updated_datesA.append(row['assigned'].strip())
            assigneesA.append(row['assignee'].strip())
            resolvedA.append(row['resolved'].strip())


idsC = []
issue_typesC = []
statusesC = []
resolutionsC = []
componentsC = []
severitiesC = []
reportersC = []
created_datesC = []
updated_datesC = []
assigneesC = []
resolvedC = []

with open('dataset3CModules.csv', mode='r', encoding='latin1') as file:
    reader = csv.DictReader(file)
    for row in reader:
        row_components = row['component'].strip().split(';')  # Split the components if they are separated by ';'
        for component in row_components:
            if not component:  # Check if the component is empty
                component = 'Empty'
            idsC.append(row['issue_id'].strip())
            issue_typesC.append(row['type'].strip())
            statusesC.append(row['status'].strip())
            resolutionsC.append(row['resolution'].strip())
            componentsC.append(component.strip())
            severitiesC.append(row['priority'].strip())
            reportersC.append(row['reporter'].strip())
            created_datesC.append(row['created'].strip())
            updated_datesC.append(row['assigned'].strip())
            assigneesC.append(row['assignee'].strip())
            resolvedC.append(row['resolved'].strip())


idsD = []
issue_typesD = []
statusesD = []
resolutionsD = []
componentsD = []
severitiesD = []
reportersD = []
created_datesD = []
updated_datesD = []
assigneesD = []
resolvedD = []

with open('dataset3DModules.csv', mode='r', encoding='latin1') as file:
    reader = csv.DictReader(file)
    for row in reader:
        row_components = row['component'].strip().split(';')  # Split the components if they are separated by ';'
        for component in row_components:
            if not component:  # Check if the component is empty
                component = 'Empty'
            idsD.append(row['IssueId'].strip())
            issue_typesD.append(row['type'].strip())
            statusesD.append(row['status'].strip())
            resolutionsD.append(row['resolution'].strip())
            componentsD.append(component.strip())
            severitiesD.append(row['priority'].strip())
            reportersD.append(row['reporter'].strip())
            created_datesD.append(row['created'].strip())
            updated_datesD.append(row['assigned'].strip())
            assigneesD.append(row['assignee'].strip())
            resolvedD.append(row['resolved'].strip())

idsW = []
issue_typesW = []
statusesW = []
resolutionsW = []
componentsW = []
severitiesW = []
reportersW = []
created_datesW = []
updated_datesW = []
assigneesW = []
resolvedW = []

with open('dataset3WModules.csv', mode='r', encoding='latin1') as file:
    reader = csv.DictReader(file)
    for row in reader:
        row_components = row['component'].strip().split(';')  # Split the components if they are separated by ';'
        for component in row_components:
            if not component:  # Check if the component is empty
                component = 'Empty'
            idsW.append(row['issue_id'].strip())
            issue_typesW.append(row['type'].strip())
            statusesW.append(row['status'].strip())
            resolutionsW.append(row['resolution'].strip())
            componentsW.append(component.strip())
            severitiesW.append(row['priority'].strip())
            reportersW.append(row['reporter'].strip())
            created_datesW.append(row['created'].strip())
            updated_datesW.append(row['assigned'].strip())
            assigneesW.append(row['assignee'].strip())
            resolvedW.append(row['resolved'].strip())

all_assignees = assignees + assigneesA + assigneesC + assigneesD + assigneesW
all_resolved = resolved + resolvedA + resolvedC + resolvedD + resolvedW

# Count the occurrences of each assignee
assignee_counts = Counter(all_assignees)

# Filter the open bugs
open_assignees = [assignee for i, assignee in enumerate(all_assignees) if all_resolved[i] != 'Closed']
open_assignee_counts = Counter(open_assignees)

# Extract the names and counts for the Pareto chart
all_assignee_names = list(open_assignee_counts.keys())
all_assignee_counts = list(open_assignee_counts.values())

# Sort the data in descending order
sorted_indices = sorted(range(len(all_assignee_counts)), key=lambda i: all_assignee_counts[i], reverse=True)
sorted_assignee_names = [all_assignee_names[i] for i in sorted_indices]
sorted_assignee_counts = [all_assignee_counts[i] for i in sorted_indices]

# Calculate the cumulative percentage
total_assignees = sum(sorted_assignee_counts)
cumulative_percent = [sum(sorted_assignee_counts[:i + 1]) / total_assignees * 100 for i in range(len(sorted_assignee_counts))]

# Create a Pareto chart
fig, ax1 = plt.subplots()

ax1.bar(range(len(sorted_assignee_counts)), sorted_assignee_counts, color='tab:blue')
ax1.set_xlabel('Assignees')
ax1.set_ylabel('Number of Open Bugs')
ax1.set_title('Assignees with Open Bugs to Resolve in Dataset 3')

ax2 = ax1.twinx()
ax2.plot(range(len(sorted_assignee_counts)), cumulative_percent, color='tab:red', marker='o', linestyle='--', label='Cumulative Percentage')

ax2.set_ylabel('Cumulative Percentage', color='tab:red')
for tl in ax2.get_yticklabels():
    tl.set_color('tab:red')

plt.xticks(range(len(sorted_assignee_counts)), sorted_assignee_names, rotation='vertical')
plt.show()