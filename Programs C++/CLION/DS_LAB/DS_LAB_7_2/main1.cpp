#include <iostream>
using namespace std;

int fibonacciS(int n) {
	if (n == 1 || n == 0)
		return n;
	return fibonacciS(n - 1) + (n - 2);
}

int main() {
	int n = 0;
	cout << "Enter n : ";
	cin >> n;

	for (int i = 0; i < n; i++) {
		cout << fibonacciS(i) << "  ";
	}
}