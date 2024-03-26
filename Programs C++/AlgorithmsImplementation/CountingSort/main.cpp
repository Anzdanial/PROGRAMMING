#include <iostream>
using namespace std;

int maxVal(int *input, int size){
	int max = 0;
	for(int i = 0; i < size; i++){
		if(input[i] >= max){
			max = input[i];
		}
	}
	return max;
}

void countingSort(int *input, int size){
	int max = maxVal(input, size);
	int count[max + 1];
	int cumulativeCount[max+1];
	int sortedArray[size];
	for(int i = 0; i < max+1; i++){
		count[i] = 0;
	}
	//Evaluating the Occurrences for each Value
	for(int i = 0; i < size; i++){
		count[input[i]]++;
	}

	for(int i = 0; i < max+1; i++){
		if(i == 0)
			cumulativeCount[i] = count[i];
		else {
			cumulativeCount[i] = cumulativeCount[i-1] + count[i];
		}
	}

	for(int i = 0; i < size; i++){
		cout << cumulativeCount[input[i]]-1 << endl;
		sortedArray[cumulativeCount[input[i]]-1] = input[i];
	}

//	for(int i = 0; i < size; i++){
//		cout << sortedArray[i] << endl;
//	}

}

int main() {
	int array[] = {4, 2, 2, 8, 3, 3,1};
	countingSort(array,7);
	return 0;
}
