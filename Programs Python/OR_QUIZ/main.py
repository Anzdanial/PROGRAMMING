# Function to calculate the minimum cost of disassembling the rocket
def calculate_minimum_cost(sequence):
    total_cost = 0

    while len(sequence) > 1:
        min_index = sequence.index(min(sequence))
        if min_index == 0:
            total_cost += sequence[0] + sequence[1]
            sequence = sequence[2:]
        elif min_index == len(sequence) - 1:
            total_cost += sequence[-1] + sequence[-2]
            sequence = sequence[:-2]
        else:
            if sequence[min_index - 1] < sequence[min_index + 1]:
                total_cost += sequence[min_index] + sequence[min_index - 1]
                sequence = sequence[:min_index - 1] + sequence[min_index + 1:]
            else:
                total_cost += sequence[min_index] + sequence[min_index + 1]
                sequence = sequence[:min_index] + sequence[min_index + 2:]

    return total_cost

# Input processing and function calling
test_cases = int(input())
for _ in range(test_cases):
    sequence = list(map(int, input().split()))
    print(calculate_minimum_cost(sequence))
