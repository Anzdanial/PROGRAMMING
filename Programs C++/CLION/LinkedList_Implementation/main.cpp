#include <iostream>
using namespace std;

template<class T>
class LinkedList {
	struct node {
		T data;
		node *next;
	};
	node *head;
public:
	LinkedList() {
		head = nullptr;
	}

	~LinkedList() {
		node *temp;
		while (head != nullptr) {
			temp = head;
			head = head->next;
			delete temp;
		}
	}

	node *createNewNode() {
		node *nn = new node;
		nn->data = 0;
		nn->next = nullptr;
	}

	void insertNodeAtHead(T val) {
		if (head == nullptr) {
			head = createNewNode();
			head->data = val;
		} else {
			node *temp = head;
			head = createNewNode();
			head->next = temp;
			head->data = val;
		}
	}

	void insertNodeAfterHead(T val){
		if(head == nullptr){
			head = createNewNode();
			head->data = val;
		}
		else if(head->next != nullptr){
			node *temp = head->next;
			head->next = createNewNode();
			node *temp1 = head->next;
			temp1->next = temp;
			temp1->data = val;
		}
	}

	void deletionAtEndList(){
		if(head == nullptr)
			return;
		else if(head->next == nullptr)
			head = nullptr;
		else {
			node *temp, *temp1;
			for (temp = head; temp->next->next != nullptr; temp = temp->next);
			temp1 = temp->next;
			temp->next = nullptr;
			temp = nullptr;
			delete temp1;
			delete temp;
		}
	}

	void print() {
		if(head == nullptr)
			cout<<"List already empty, cannot be emptied!!!"<<endl;
		else {
			node *temp = head;
			while (temp != nullptr) {
				if (temp->next == nullptr) {
					cout << temp->data;
					temp = temp->next;
				} else {
					cout << temp->data << "->";
					temp = temp->next;
				}
			}
			cout << endl;
			delete temp;
		}
	}

	void reversePrint(){
		if(head->next == nullptr) {
			cout << head->data << "->";
			return;
		}
		else
			head = head->next;
		cout<<head->data<<"->";
		reversePrint();
	}


};

int main() {
	LinkedList<int> test;
	for (int i = 6; i > 0; i--)
		test.insertNodeAtHead(i);
	test.print();
	//test.reversePrint();
	return 0;
}