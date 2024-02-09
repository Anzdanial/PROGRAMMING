#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <algorithm>
#include <thread>
#include <mutex>

std::mutex result_mutex;
std::vector<int> result;

void process_data(const std::string& filename) {
	std::ifstream file(filename);
	if (!file.is_open()) {
		std::cerr << "Error opening file: " << filename << std::endl;
		return;
	}

	std::vector<int> numbers;
	std::string line;
	while (std::getline(file, line)) {
		for (char& c : line) {
			if (isdigit(c)) {
				std::string number_str;
				while (isdigit(c)) {
					number_str += c;
					if (!(file.get(c))) break;
				}
				numbers.push_back(std::stoi(number_str));
			}
		}
	}

	// Sort the numbers in ascending order
	std::sort(numbers.begin(), numbers.end());

	// Lock before updating the shared result vector
	std::lock_guard<std::mutex> lock(result_mutex);
	result.insert(result.end(), numbers.begin(), numbers.end());
}

int main() {
	// File paths
	std::string file1_path = "datafile1.txt";
	std::string file2_path = "datafile2.txt";

	// Create threads
	std::thread thread1(process_data, file1_path);
	std::thread thread2(process_data, file2_path);

	// Wait for threads to finish
	thread1.join();
	thread2.join();

	// Sort the final result
	std::sort(result.begin(), result.end());

	// Print result before merge
	std::cout << "Result before merge: ";
	for (const auto& num : result) {
		std::cout << num << " ";
	}
	std::cout << std::endl;

	// Merge the results
	// (Since the result vector is already sorted, no additional sorting is required)

	// Print result after merge
	std::cout << "Result after merge: ";
	for (const auto& num : result) {
		std::cout << num << " ";
	}
	std::cout << std::endl;

	return 0;
}