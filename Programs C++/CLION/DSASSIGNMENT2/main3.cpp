#include <iostream>
#include <vector>
#include <stack>
#include <queue>
using namespace std;

bool checkRepititionStack(stack<char> S) {
	int count0 = 0;
	int count1 = 0;

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

int main() {

	stack<char> S;

	S.push('0');
	S.push('1');
	S.push('0');
	S.push('0');
	S.push('1');
	S.push('1');

	cout << checkRepititionStack(S);
}