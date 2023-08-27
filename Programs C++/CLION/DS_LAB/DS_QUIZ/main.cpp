#include <iostream>

using namespace std;

template<typename T>
class list {
public:
	//Node Class
	class node {
		T data;
		node *next;

		node(T data, node *p) {
			this->data = data;
			next = p;
		}

		friend class list<T>;
	};

//Other Functions of Link list
	void printList();

	void InsertatTail(T value);


	//Deletion of Value when found;
	void findVal(int val) {
		if (head == nullptr)
			return head;
		for (temp1 = head; temp1->next->data != val || temp1->next != nullptr; temp1 = temp1->next) {
			node *temp3 = temp1->next;
			node *temp2 = temp1->next->next;
			temp1->next = nullptr;
			temp1->data = NULL;
			delete temp1;
			temp1 = temp2;
			delete temp2;
			temp3->next = temp1;
		}
		if (temp1->next != nullptr)
			return findVal(val);
		else
			return;
	}


	list() {
		head = nullptr;
		tail = nullptr;
		temp1 = nullptr;
	}

	~list();

private:
	node *head;
	node *tail;
	node *temp1;
};

//Destructor
template<typename T>
list<T>::~list() {
	node *temp;
	while (head != nullptr) {
		temp = head;
		head = head->next;
		delete temp;
	}
}

//print list
template<typename T>
void list<T>::printList() {
	node *current;
	current = head;
	while (current != nullptr) {
		cout << current->data << " -> ";
		current = current->next;
	}
	cout << "NULL" << endl;
}

//insertnode at tail
template<typename T>
void list<T>::InsertatTail(T value) {
	node *nnode = new node(value, nullptr);
	if (head == nullptr) {
		head = nnode;
		tail = nnode;
	} else {
		tail->next = nnode;
		tail = nnode;
	}
}


int main() {
	cout << "Hello World" << endl;
	return 0;
}