#include <iostream>
#include <stack>
using namespace std;

template<typename T>
void sortedInsert(stack<T>& S, int x)
{
    if (S.empty() || x > S.top()) {
        S.push(x);
        return;
    }

    int temp = S.top();
    S.pop();
    sortedInsert(S, x);

    S.push(temp);
}

template<typename T>
void sortStack(stack<T>& S)
{

    if (!S.empty()) {
        // Remove the top item
        int x = S.top();
        S.pop();

        // Sort remaining stack
        sortStack(S);

        // Push the top item back in sorted stack
        sortedInsert(S, x);
    }
}

int main() {

    stack<int> S;

    S.push(30);
    S.push(-5);
    S.push(14);
    S.push(18);
    S.push(-3);

    cout << "Before Sorting : ";

    for (int i = 0; i < 5; i++) {

        cout << S.top() << " ";
        S.pop();
    }

    cout << endl;

    S.push(30);
    S.push(-5);
    S.push(14);
    S.push(18);
    S.push(-3);

    sortStack(S);

    cout << "After Sorting : ";

    for (int i = 0; i < 5; i++) {

        cout << S.top() << " ";
        S.pop();
    }
}