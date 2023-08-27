#include <iostream>
#include <string>
#include <string.h>
using namespace std;


struct Node
{
	Node* next;
	Node* prev;
	string Type;
	int capacity;
	int Guests;
	bool filled;

	Node(string TypeName) {
		next = NULL;
		prev = NULL;
		Type = TypeName;
		Guests = 0;
		filled = 0;

		if (Type == "Family") {
			capacity = 6;
		}
		else if (Type == "Suite") {
			capacity = 10;
		}
		else if (Type == "Quad") {
			capacity = 4;
		}
		else if (Type == "Single") {
			capacity = 1;
		}
		else if (Type == "Triple") {
			capacity = 3;
		}
	}
};


class List {
private:
	Node* head;
	Node* tail;
public:

	List() {
		for (int i = 0; i < 7; i++) {

			Node* newNode = NULL;

			if (i == 0 || i == 1) {
				newNode = new Node("Single");

			}
			else if (i == 2) {
				newNode = new Node("Triple");

			}
			else if (i == 3) {
				newNode = new Node("Quad");

			}
			else if (i == 5 || i == 4) {
				newNode = new Node("Family");

			}
			else if (i == 6) {
				newNode = new Node("Suite");

			}

			if (head == NULL) {

				head = newNode;
			}
			else {

				Node* temp = head;

				while (temp->next != NULL){
					temp = temp->next;
				}

				temp->next = newNode;
				newNode->prev = temp;
			}
		}

		Node* temp = head;

		while (temp->next != NULL) {
			temp = temp->next;
		}

		tail = temp;
		tail->next = head;
		head->prev = tail;
	}
	~List() {

	}

	class iterator {
		friend class List;

	private:
		Node* Nodeptr;
	public:
		iterator() {
			Nodeptr = NULL;
		}

		iterator(Node* INode) : Nodeptr(INode) {}

		int& operator*() const {
			return Nodeptr->Guests;
		}

		iterator operator++(int) {
			iterator temp = *this;
			Nodeptr = Nodeptr->next;
			return temp;
		}

		iterator operator++() {
			iterator temp = *this;
			Nodeptr = Nodeptr->next;
			return temp;
		}

		bool operator!=(const iterator& tempItr) {
			if (this->Nodeptr != tempItr.Nodeptr) {
				return 1;
			}

			return 0;
		}

		bool operator==(const iterator& tempItr) {
			if (this->Nodeptr == tempItr.Nodeptr) {
				return 1;
			}

			return 0;
		}
	};

	iterator begin() const {
		return iterator(head);
	}

	iterator end() const {
		return iterator(tail);
	}

	void insertGuests() {

		int num = 0;

		do{

			cout << "Enter number of Guests : ";
			cin >> num;

		} while (num < 1);

		Node* temp = head;
		bool flag = 1;

		do {

			if (!temp->filled) {

				if (temp->Guests + num <= temp->capacity) {

					temp->Guests = temp->Guests + num;

					if (temp->Guests == temp->capacity) {

						temp->filled = 1;
					}

					flag = 0;
				}

			}

			temp = temp->next;

		} while (temp != head && flag);
	}

	void print() {

		Node* temp = head;

		do {
			cout << "Type : " << temp->Type << endl;
			cout << "Capacity : " << temp->capacity << endl;
			cout << "Guests : " << temp->Guests << endl;
			cout << "Filled : ";
			if (temp->filled) {
				cout << "yes" << endl;
			}
			else {
				cout << "no" << endl;
			}

			cout << endl;
			temp = temp->next;
		} while (temp != head);

		cout << endl;
	}
};

int main() {

	List Hotel1;

	for (int i = 0; i < 4; i++)
		Hotel1.insertGuests();

	Hotel1.print();
}
