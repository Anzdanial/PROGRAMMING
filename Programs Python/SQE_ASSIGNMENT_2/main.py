import pandas as pd

module_results_bug = {}
module_results = {}

sheets = ["A", "C", "D", "W"]
for i in range(len(sheets)):
    df = pd.read_excel('./dataset 3 for sqe.xlsx', sheet_name=sheets[i])
    bug_df = df[df["type"] == "Bug"]

    module_results_bug[sheets[i]] = len(bug_df)
    module_results[sheets[i]] = len(df)

total_proportion = sum(module_results_bug.values()) / sum(module_results.values())
total_percentage = total_proportion * 100

print(f"Proportion of Actual Defects (Bugs) in Whole Dataset: {total_proportion:.2f}")
print(f"Percentage of Actual Defects (Bugs) in Whole Dataset: {total_percentage:.2f}%")

for i in range(len(sheets)):
    print(f"Proportion of Actual Defects in Module {sheets[i]}: {module_results_bug[sheets[i]] / module_results[sheets[i]]:.2f}")
    print(f"Percentage of Actual Defects in Module {sheets[i]}: {(module_results_bug[sheets[i]] / module_results[sheets[i]]) * 100:.2f}%")