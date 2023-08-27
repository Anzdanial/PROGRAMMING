#include <iostream>
#include <stack>
using namespace std;

template<typename T>
void insertAtEnd(stack<T>& S, T x) {

	if (S.size() == 0) {
		S.push(x);
	}
	else {

		T a = S.top();
		S.pop();
		insertAtEnd(S, x);
		S.push(a);
	}
}

template<typename T>
void reverseStack(stack<T>& S) {

	if (S.size() > 0) {
		T a = S.top();
		S.pop();
		reverseStack(S);
		insertAtEnd(S, a);
	}
	return;
}


int main() {

	stack<int> S;

	S.push(1);
	S.push(2);
	S.push(3);
	S.push(4);

	for (int i = 0; i < 4; i++) {

		cout << S.top() << " ";
		S.pop();
	}

	cout << endl;

	S.push(1);
	S.push(2);
	S.push(3);
	S.push(4);

	reverseStack(S);

	for (int i = 0; i < 4; i++) {

		cout << S.top() << " ";
		S.pop();
	}
}