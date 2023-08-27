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
class ll {



public:
	node<t>* head;
	node<t>* top;

	node<t>* makenode(t x) {
		node<t>* newnode = new node<t>();
		newnode->data = x;
		newnode->next = NULL;

		return newnode;
	}


	ll() {
		head = NULL;
		top = NULL;
	}

};



template<typename t>
class stack {
public:
	ll<t>* stk;

	stack() {
		stk = new ll<t>();
	}



	void push(t x) {
		if (stk->head != NULL) {

			node<t>* newnode = stk->makenode(x);
			newnode->next = stk->top;
			stk->top = newnode;
		}
		else {

			node<t>* newnode = stk->makenode(x);
			stk->head = newnode;
			stk->top = newnode;

		}

	}

	bool pop() {
		if (stk->top != stk->head) {
			node<t>* temp = stk->top;
			stk->top = temp->next;
			delete temp;
			return true;
		} else if (stk->top == stk->head) {
			node<t>* temp = stk->top;
			delete temp;
			stk->head = stk->top = NULL;
		}
		else { return false; }
	}

	bool isEmpty() {
		return (stk->head == NULL && stk->top == NULL);
	}

	bool Top(t& val) {
		if (!isEmpty()) {
			val = stk->top->data;
			return true;
		}
		else { return false; }

	}


	friend class Iterator;
	class Iterator {

	public:
		node<t>* curr;
		friend class list;
		Iterator() {
			curr = NULL;
		}

		Iterator(node<t>* newPtr) {
			curr = newPtr;
		}


		t& operator*() {
			return curr->data;
		}

		Iterator operator++() {
			Iterator temp = *this;
			curr = curr->next;
			return temp;
		}

		Iterator operator++(int) {
			Iterator temp = *this;
			curr = curr->next;
			return temp;
		}

		bool operator!=(const Iterator& it) {
			return (curr != it.curr);
		}

		bool operator==(const Iterator& it) {
			return (curr == it.curr);
		}

		Iterator operator =(const Iterator& it) {
			curr = it.curr;
			return *this;
		}

	};



	Iterator begin() const {
		return stk->top;
	}

	Iterator end() const {
		return stk->head;
	}




	void print() {

		for (auto it = begin(); it != NULL; it++) {
			cout << " " << *it;
		}
		cout << endl;
	}

	void empty() {
		while (!isEmpty()) {
			pop();
		}

	}

	void removeAll(const t& removeChar) {


		node<t>* temp = stk->top;
		while (temp->data == removeChar) {
			stk->top = temp->next;
			delete temp;
			temp = stk->top;
		}
		while (temp->next) {
			node<t>* deleting = temp->next;
			if (deleting->data == removeChar) {
				temp->next = temp->next->next;
				delete deleting;
			}
			else {
				temp = temp->next;
			}

		}
	}

	void removeAdjacent(char removeChar) {


		node<t>* temp = stk->top;
		while (temp->data == removeChar && temp->next->data == removeChar && temp->next->next->data == removeChar) {
			stk->top = temp->next->next->next;
			delete temp->next->next;
			delete temp->next;
			delete temp;
			temp = stk->top;
		}
		while (temp->next->next->next->next) {
			node<t>* deleting = temp->next;
			if (temp->next->data == removeChar && temp->next->next->data == removeChar && temp->next->next->next->data == removeChar) {

				node<t>* jumpto = temp->next->next->next->next;
				delete temp->next->next->next;
				delete temp->next->next;
				delete temp->next;
				temp->next = jumpto;
			}
			else {
				temp = temp->next;
			}

		}

		if (temp->next->data == removeChar && temp->next->next->data == removeChar && temp->next->next->next->data == removeChar) {
			node<t>* jumpto = temp->next->next->next->next;
			delete temp->next->next->next;
			delete temp->next->next;
			delete temp->next;
			temp->next = jumpto;

		}


	}


	void deleteMiddle() {
		node<t>* fastit, * slowit;
		int s = -2;

		if (stk->top == stk->head) {
			empty();
			return;
		}

		fastit = slowit = stk->top;
		while (fastit != NULL) {
			fastit = fastit->next;
			s++;
		}
		s = s / 2;

		while (s) {
			s--;
			slowit = slowit->next;
		}

		node<t>* deletenode = slowit->next;
		slowit->next = slowit->next->next;
		delete deletenode;
	}
};



template<typename t>
class queueLL {

	

public:
	node<t>* head;
	node<t>* top;

	node<t>* makeNode(t x) {
		node<t>* newNode = new node<t>();
		newNode->data = x;
		newNode->next = NULL;

		return newNode;
	}


	queueLL() {
		head = NULL;
		top = NULL;
	}

	void enqueue(t x) {
		if (head != NULL) {

			node<t>* newNode = makeNode(x);
			newNode->next = top;
			top = newNode;
		}
		else if(head == top){

			node<t>* newNode = makeNode(x);
			head = newNode;
			top = newNode;

		} 

		else {
			node<t>* newNode = makeNode(x);
			newNode->next = top;
			top = newNode;

		}

	}

	bool dequeue(t& newval) {
		node<t>* temp = top;
		if (head != NULL && top != NULL) {
			while (temp->next != head) {
				temp = temp->next;
			}

			node<t>* deleteTemp = temp->next;
			delete deleteTemp;
			temp->next = NULL;
			head = temp;
			return true;
		}

		else {
			return false;
		}

	}


	bool isEmpty() {
		return (head != NULL && top != NULL);
	}

	bool Top(t& val) {
		if (isEmpty()) return false;
		else {
			val = top->data;
			return true;
		}

	}


	void print() {
		node<t>* temp = top;
		while (temp != head) {
			cout << temp->data << " ";
			temp = temp->next;
		}
		cout << "\n";
		return;

	
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

template<typename t>
class queueStk {

	stack<t> stack1;
	stack<t> stack2;

public:

	bool isEmpty() {
		return (stack1.isEmpty() && stack2.isEmpty());
	}
	bool isFull() {
		return (!stack1.isEmpty() || !stack2.isEmpty());
	}

	void print() {
		int val;
		while (!stack1.isEmpty()) {
			val = stack1.stk->top->data;
			stack1.pop();
			stack2.push(val);
		}


		while (!stack2.isEmpty()) {
			val = stack2.stk->top->data;
			cout << val << " ";
			stack2.pop();
			stack1.push(val);
		}
		cout << endl;
	}

	void enqueue(t x) {

		stack1.push(x);

	}

	void dequeue() {
		int val;
		while (!stack1.isEmpty()) {
			val = stack1.stk->top->data;
			stack1.pop();
			stack2.push(val);
		}

		stack2.pop();


		while (!stack2.isEmpty()) {
			val = stack2.stk->top->data;

			stack2.pop();
			stack1.push(val);
		}
	}

};


int main() {
	//queueLL<char> q;
	//char x;
	//if (q.isEmpty()) cout << "Empty\n";
	//q.enqueue('a');
	//q.enqueue('a');
	//q.enqueue('b');
	//q.enqueue('c');
	//q.enqueue('d');
	//cout << "Printing Queue: "; q.print();
	//q.dequeue(x);
	//q.dequeue(x);

	//cout << "Printing Queue after Dequeueing: ";q.print();
	//cout << "\n";

	//queueArr<int> q2;
	//int val;
	//q2.enqueue(1);
	//q2.enqueue(2);
	//q2.enqueue(3);
	//q2.enqueue(4);
	//q2.print();

	//q2.enqueue(5);
	//q2.enqueue(6);
	//q2.enqueue(7);
	//q2.enqueue(8);
	//q2.enqueue(9);
	//q2.print();

	//q2.enqueue(10);
	//q2.print();
	//q2.enqueue(11);
	//q2.print();

	queueStk<int> q3;
	q3.enqueue(1);
	q3.enqueue(2);
	q3.enqueue(3);
	q3.enqueue(4);
	q3.print();

	q3.dequeue();
	q3.dequeue();
	q3.print();
}