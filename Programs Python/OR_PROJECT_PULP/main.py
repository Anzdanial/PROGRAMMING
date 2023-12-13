from pulp import *

# Define the decision variables
x = LpVariable.dicts("x", range(4), cat="Integer")  # Production levels
y = LpVariable.dicts("y", range(4), cat="Integer")  # Inventory levels

# Define the constraints
model = LpProblem("Multi-Period Production Smoothing Model", LpMinimize)

# Production capacity constraints
for i in range(4):
    model.addConstraint(x[i] <= 3000)

# Inventory balance constraints
for i in range(4):
    model.addConstraint(x[i] - y[i] == demand[i])

# Demand constraints (Month 1)
for i in range(1, 4):
    model.addConstraint(x[i] - y[i] + y[i - 1] == demand[i])

# Objective function to minimize total cost
model.objective = lpSum(x[i] * production_cost + y[i] * inventory_cost)

# Solve the model
model.solve()

# Print the solution
print("Status:", LpStatus[model.status])
print("Objective value:", model.objective.varValue)
for v in model.variables():
    print(v.name, "=", v.varValue)