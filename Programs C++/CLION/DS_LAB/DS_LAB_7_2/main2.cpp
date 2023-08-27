#include <iostream>
using namespace std;

template<typename T>
class Node {

public:
	T data;
	Node* next;
	Node(T d) {
		data = d;
		next = NULL;
	}
};

//template<typename T>
//class LinkedList {
//private:
//	Node<T>* head;
//public:
//
//	LinkedList() {
//
//		head = NULL;
//	}
//	LinkedList(T d) {
//
//		Node<T>* newNode = new Node<T>(d);
//		head = newNode;
//	}
//	void Insert(T d) {
//
//		Node<T>* newNode = new Node<T>(d);
//
//		if (head == NULL) {
//
//			head = newNode;
//			return;
//		}
//
//		Node<T>* temp = head;
//
//		while (temp->next != NULL) {
//
//			temp = temp->next;
//		}
//
//		temp->next = newNode;
//	}
//	void recursivePrint() {
//
//
//	}
//};

template<typename T>
void Insert(Node<T>*& head, T d) {

	Node<T>* newNode = new Node<T>(d);

	if (head == NULL) {

		head = newNode;
		return;
	}

	newNode->next = head;
	head = newNode;
}

template<typename T>
void print(Node<T>* head) {

	if (head) {

		cout << head->data << "  ";
		print(head->next);
	}
}

template<typename T>
int length(Node<T>* head) {

	if (head) {

		return 1 + length(head->next);
	}

	return 0;
}


template<typename T>
bool isSorted(Node<T>* head) {

	if (head->next) {

		if (isSorted(head->next)) {
			if (head->data <= head->next->data) {
				return 1;
			}
			else {
				return 0;
			}
		}
		else
			return 0;
	}

	return 1;
}

template<typename T>
void deleteAll(Node<T>*& head) {

	if (head) {

		deleteAll(head->next);
		delete head;
		head = NULL;
	}
}

int main() {

	Node<int>* head = NULL;

	Insert(head, 10);
	Insert(head, 9);
	Insert(head, 7);
	Insert(head, 5);

	print(head);
	cout << endl;

	cout << "Length Before deletion : " << length(head) << endl;

	cout << isSorted(head) << endl;

	deleteAll(head);

	cout << "Length After deletion : " << length(head) << endl;

	print(head);
	cout << endl;
}