#include <iostream>

using namespace std;

void FibonacciSeries(int num) {
	int num1 = 1, num2 = 2, temp;
	int sum = 2;
	for (int i = 0; i < num & num2 < 4000000; i++) {
			temp = num2;
			num2 = num1 + num2;
			num1 = temp;
		if (num2 % 2 == 0)
			sum += num2;
		cout << num2 << endl;
	}
	cout<<sum<<endl;
}

int main() {
	int input;
	cout << "Enter the Number of Terms to Print: ";
	cin >> input;
	FibonacciSeries(input);
	return 0;
}