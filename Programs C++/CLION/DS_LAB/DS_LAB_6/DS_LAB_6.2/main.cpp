#include<iostream>
using namespace std;

template<typename t>
struct node {
	t data;
	node<t>* next;
	node() : next(nullptr) {}
	node(const t& item, node<t>* ptr = nullptr)
	{
		data = item;
		next = ptr;
	}


};


template<typename t>
class queueArr {

public:

	int rear=-1 , front = -1;
	int size = 10;
	t* arr;

	queueArr() {
		arr = new t[size];
	}

	bool isEmpty() {
		if (rear == front && rear == -1) {
			return true;
		}
		return false;
	}

	bool isFull() {
		if ((rear + 1) % size == front) {
			return true;
		}
		return false;
	}

	void enqueue(t x) {

		if (isFull()) {
			return;
		}
		else if (isEmpty()) {
			front = rear = 0;
		}
		else {
			rear = (rear + 1) % size;
		}


		arr[rear] = x;


	}


	void dequeue(t&val) {
		if (isEmpty()) {
			return;
		}
		else if (front == rear) {
			front = rear = -1;
		}

		else {
			front = (front + 1) % size;
		}



	}

	void print() {
		if (isEmpty()) {
			cout << "Empty!\n";
			return;
		}

		for (int i = front; i%size  != rear; i++) {
			cout << arr[i] << " ";
		} cout << arr[rear] << " ";

		cout << endl;
	}

};




int main() {
	queueArr<int> q2;
	int val;
	q2.enqueue(1);
	q2.enqueue(2);
	q2.enqueue(3);
	q2.enqueue(4);
	q2.print();

	q2.enqueue(5);
	q2.enqueue(6);
	q2.enqueue(7);
	q2.enqueue(8);
	q2.enqueue(9);
	q2.print();

	q2.enqueue(10);
	q2.print();
	q2.enqueue(11);
	q2.print();
}