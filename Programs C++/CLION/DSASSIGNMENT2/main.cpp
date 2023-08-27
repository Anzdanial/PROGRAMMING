#include <iostream>
#include <string>
using namespace std;

char getChar(int i, int j) {
	if (i == 2) {
		if (j == 0)
			return 'A';
		else if (j == 1)
			return 'B';
		else if (j == 2)
			return 'C';
	}
	else if (i == 3) {
		if (j == 0)
			return 'D';
		else if (j == 1)
			return 'E';
		else if (j == 2)
			return 'F';
	}
	else if (i == 4) {
		if (j == 0)
			return 'G';
		else if (j == 1)
			return 'H';
		else if (j == 2)
			return 'I';
	}
	else if (i == 5) {
		if (j == 0)
			return 'J';
		else if (j == 1)
			return 'K';
		else if (j == 2)
			return 'L';
	}
	else if (i == 6) {
		if (j == 0)
			return 'M';
		else if (j == 1)
			return 'N';
		else if (j == 2)
			return 'O';
	}
	else if (i == 7) {
		if (j == 0)
			return 'P';
		else if (j == 1)
			return 'Q';
		else if (j == 2)
			return 'R';
		else if (j == 3)
			return 'S';
	}
	else if (i == 8) {
		if (j == 0)
			return 'T';
		else if (j == 1)
			return 'U';
		else if (j == 2)
			return 'V';
	}
	else if (i == 9) {
		if (j == 0)
			return 'W';
		else if (j == 1)
			return 'X';
		else if (j == 2)
			return 'Y';
		else if (j == 3)
			return 'Z';
	}
	return 0;
}

int getSize(int n) {
	if (n == 7 || n == 9)
		return 4;
	else
		return 3;
}

void print(int* arr, int n, string str, int s) {
	if (n <= 0)
		return;
	int size = getSize(*arr);
	for (int i = 0; i < size; i++) {
		if (n != 1)
			print(arr + 1, n - 1, str + getChar(*arr, i), s);

		else if (n == 1)
			cout << str + getChar(*arr, i) << "  ";

		if (n == s)
			cout << endl;
	}
}


void inputNumbers(int*& arr, int& size) {
	cout << "Enter Size : ";
	cin >> size;

	while (size < 2 || size > 8) {
		cout << "Enter Correct Size : ";
		cin >> size;
	}

	arr = new int[size];

	cout << endl << "Enter the Numbers for keypad: "<<endl;

	for (int i = 0; i < size; i++) {
		cin >> arr[i];

		while (arr[i] < 2 || arr[i] > 9) {
			cin >> arr[i];
		}
	}
}

int main() {
	int* arr;
	int size;

	inputNumbers(arr, size);

	print(arr, size, "", size);
	return 0;
}