#include <iostream>
#include <semaphore.h>

class Stack {
private:
	int* a;          // array for stack
	int max;         // max size of array
	int top;         // stack top
	sem_t emptySem;  // semaphore for available space
	sem_t fullSem;   // semaphore for used space

public:
	Stack(int m) : max(m), top(0) {
		a = new int[m];
		sem_init(&emptySem, 0, m);  // Initialize emptySem with max size
		sem_init(&fullSem, 0, 0);   // Initialize fullSem with 0
	}

	void push(int x) {
		sem_wait(&emptySem);  // Wait for available space

		// Critical Section
		a[top] = x;
		++top;

		sem_post(&fullSem);  // Signal that space is used
	}

	int pop() {
		sem_wait(&fullSem);  // Wait for used space

		// Critical Section
		--top;
		int tmp = a[top];

		sem_post(&emptySem);  // Signal that space is available

		return tmp;
	}
};

int main() {
	Stack myStack(5);

	// Example usage
	myStack.push(1);
	myStack.push(2);

	int poppedValue = myStack.pop();
	std::cout << "Popped value: " << poppedValue << std::endl;

	return 0;
}
