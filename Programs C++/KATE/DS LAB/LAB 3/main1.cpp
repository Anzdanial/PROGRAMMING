#include <iostream>
using namespace std;

template <typename T>
class node {
public:
	T data;
	node<T> *next;

	node() : next(nullptr) {}
	node(const T& item, node<T> *ptr = nullptr)
	{
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
class Stack{
private:
	node<T>* top;
public:
	Stack()
	{
		top=nullptr;
	}
	
	void Push(T val){
			node<T>* temp = new node<T>;
			if (!temp) {
				cout << "\nStack Overflow";
				exit(1);
			}
			temp->data = val;
			temp->next = top;
			top = temp;
	}

	bool Pop(T &newval){
		node<T>* temp=new node<T>;
		if (top == NULL) {
			cout << "\nStack Underflow" << endl;
			return false;
		}
		else {
			temp = top;
			top = top->next;
			newval=top->data;
			free(temp);
			return true;
		}
	}

	bool isEmpty(){
		if(top==NULL)
			return true;
		else
			return false;
	}

	bool Top(T &val){
		if(top==NULL){
			cout<<"\nStack underflow";
			return false;
		}
		node<T>* temp=new node<T>;
		temp=top;
		val=temp->data;
		return true;
	}

	void display(){
		node<T>* temp=new node<T>;
		if (top == NULL) {
			cout << "\nStack Underflow";
			exit(1);
		}
		else {
		temp = top;
			while (temp != NULL) {
			// Print node data
			cout << temp->data << "-> ";
			// Assign temp link to temp
			temp = temp->next;
			}
		}
	}
};

int main()
{
	Stack <int> s1;
	s1.Push(1);
	s1.Push(2);
	s1.Push(3);
	s1.Push(4);
	s1.Push(5);
	if(s1.isEmpty())
		cout<<"Stack is empty"<<endl;
	else
	{
		cout<<"STACK : ";
		s1.display();
	}
	int top=10;
	s1.Pop(top);
	cout<<"\nNew top is: "<<top<<endl;
	cout<<"New Stack: ";
	s1.display();

	top=20;
	cout<<"\nAfter top function: ";
	s1.Top(top);
	s1.display();
	return 0;
}
