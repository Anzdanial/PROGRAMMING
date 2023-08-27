#include <iostream>
using namespace std;

template <typename T>
class node {
public:
	T data;
	node<T> *next;

	node() : next(nullptr) {}
	
	node(const T& item, node<T> *ptr = nullptr){
		data=item;
		next=ptr ;
	}
};

template <typename T>
class list {
private:
	node<T>* head;
	node<T>* tail;
public:
	list(){
	head = tail = new node<T>();
	}

	~list() {
		while (head->next != nullptr) {
			node<T>* temp = head;
			head = head->next;
			delete temp;
		}
		delete head;
	}


	// Inner class iterator
	class iterator {
	friend class list;
	private:
	node<T> *nodePtr;

	iterator(node<T> *newPtr) : nodePtr(newPtr) {}
	public:
	iterator() : nodePtr(nullptr) {}


	bool operator!=(const iterator& itr) const {
		return nodePtr != itr.nodePtr;
	}

	// Overload for the dereference operator *
	T& operator*() const {
		return nodePtr->next->data;
	}

	// Overload for the postincrement operator ++
	iterator operator++(int) {
		iterator temp = *this;
		nodePtr = nodePtr->next;
		return temp;
	}
	}; // End of inner class iterator

	iterator begin() const {
	return head;
	}

	iterator end() const {
	return tail;
	}

	iterator insert(iterator position,const T& value) {
	node<T>* newNode = new node<T>(value, position.nodePtr->next);
	if (position.nodePtr == tail) tail = newNode;
	position.nodePtr->next = newNode;
	return position;
	}

	iterator erase(iterator position) {
	node<T> *toDelete = position.nodePtr->next;
	position.nodePtr->next = position.nodePtr->next->next;
	if (toDelete == tail) tail = position.nodePtr;
	delete toDelete;
	return position;
	}
	
};

template <typename T>
class Queue{
	node<T> *front;
	node<T>* rear;
public:
	Queue(){
		front=rear=NULL;
	}

	void enQueue (T val)
	{
		node<T>* temp= new node<T>;
		temp->data= val;
			if (rear == NULL) {
				front = rear = temp;
			return;
		}
		rear->next = temp;
		rear = temp;
	}
	
	bool deQueue(T &newval)
	{
		// If queue is empty, return NULL.
		if (front == NULL)
			return false;

		node<T>* temp=new node<T>;
		temp= front;
		front = front->next;
		front->data=newval;

		// If front becomes NULL, then
		// change rear also as NULL
		if (front == NULL)
		{
			rear = NULL;
			return false;
		}

		delete (temp);
		return true;
	}
	
	bool isEmpty(){
		if(front==NULL && rear==NULL)
			return true;
		else
			return false;
	}
	
	bool Top( T &val){
		if(front==NULL){
			cout<<"\nQueue is empty";
			return false;
		}
		node<T>* temp=new node<T>;
		temp=front;
		temp->data=val;
		return true;
	}
	
	void Display(){
		node<T>* temp=new node<T>;
		temp=front;
		while (temp != NULL){
			cout<<temp->data<<" ";
			temp = temp->next;
		}
		cout<<endl;
	}
};


int main()
{
	Queue <int> q1;
	q1.enQueue(1);
	q1.enQueue(2);
	q1.enQueue(3);
	q1.enQueue(4);
	q1.enQueue(5);
	if(q1.isEmpty())
		cout<<"Queue is empty"<<endl;
	else
	{
		cout<<"Queue elements are: ";
		q1.Display();
	}
	int front=10;
	q1.deQueue(front);
	cout<<"\nNew front is: "<<front<<endl;
	cout<<"New Queue: ";
	q1.Display();

	return 0;
}
