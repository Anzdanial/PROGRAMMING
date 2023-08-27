#include <iostream>
#include <vector>
#include <stack>
#include <queue>
#include <string>
using namespace std;

bool checkRepititionStack(string str) {

	stack<char> S;

	int count0 = 0;
	int count1 = 0;
	int size = str.length();

	for (int i = 0; i < size; i++) {

		S.push(str[i]);
	}

	while (!S.empty()) {

		while (!S.empty() && S.top() != '0') {
			S.pop();
			count1++;
		}

		while (!S.empty() && S.top() != '1') {
			S.pop();
			count0++;
		}

		if (count0 != count1)
			return 0;

		count0 = 0;
		count1 = 0;
	}

	return 1;
}

bool checkIfValidString(string str) {

	int size = str.length();

	for (int i = 0; i < size; i++) {

		if (str[i] == '0' || str[i] == '1') {

		}
		else {
			cout << endl << "Invalid String entered" << endl;
			return 0;
		}
	}

	return 1;
}

int main() {

	string str;

	cout << "Enter String : ";
	getline(cin, str);

	while (!checkIfValidString(str)) {

		cout << "Enter String : ";
		getline(cin, str);
	}

	if (checkRepititionStack(str))
		cout << "\nValid Input\n";
	else
		cout << "\nInValid Input\n";
}