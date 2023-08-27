#include<iostream>
using namespace std;

template<class t>
struct node {
	t data;
	node<t> *next;

	node() : next(nullptr) {}

	node(const t &item, node<t> *ptr = nullptr) {
		data = item;
		next = ptr;
	}
};

template<typename t>
class queueLL {
public:
	node<t> *head;
	node<t> *top;

	node <t> *create(t val) {
		node <t> *newNode = new node<t>();
		newNode->data = val;
		newNode->next = NULL;
		return newNode;
	}


	queueLL() {
		head = NULL;
		top = NULL;
	}

	void enqueue(t x) {
		if (head != NULL) {
			node<t> *newNode = create(x);
			newNode->next = top;
			top = newNode;
		}
		else if (head == top) {
			node<t> *newNode = create(x);
			head = newNode;
			top = newNode;
		}
		else {
			node<t> *newNode = create(x);
			newNode->next = top;
			top = newNode;

		}

	}

	bool dequeue(t &newval) {
		node<t> *temp = top;
		if (head != NULL && top != NULL) {
			while (temp->next != head) {
				temp = temp->next;
			}

			node<t> *deleteTemp = temp->next;
			delete deleteTemp;
			temp->next = NULL;
			head = temp;
			return true;
		} else {
			return false;
		}

	}


	bool isEmpty() {
		return (head != NULL && top != NULL);
	}

	bool Top(t &val) {
		if (isEmpty())
			return false;
		else {
			val = top->data;
			return true;
		}

	}


	void print() {
		node<t> *temp = top;
		while (temp != head->next) {
			cout << temp->data << " ";
			temp = temp->next;
		}
		cout << endl;
		return;
	}

	bool isFull(){
		return false;
	}
};


int main() {
	queueLL<char> q;
	char x;
	if (q.isEmpty())
		cout << "Queue is Empty!!"<<endl;
	q.enqueue('a');
	q.enqueue('a');
	q.enqueue('b');
	q.enqueue('c');
	q.enqueue('d');

	cout << "Queue: ";
	q.print();
	q.dequeue(x);
	q.dequeue(x);

	cout << "Dequeueing: ";
	q.print();
	cout << endl;
}