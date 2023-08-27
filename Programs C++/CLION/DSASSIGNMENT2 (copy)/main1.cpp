#include <iostream>
#include <time.h>
using namespace std;

class node {

public:
	node* next;
	node* prev;
	int data;
	node() {
		next = NULL;
		data = 0;
		prev = NULL;
	}
	node(int d) {
		next = NULL;
		data = d;
		prev = NULL;
	}
};

void insertathead(node*& head, int data) {

	node* newnode = new node(data);

	if (head == NULL) {

		head = newnode;
		return;
	}

	newnode->next = head;
	head->prev = newnode;
	head = newnode;
}

void insertatend(node*& head, int data) {

	node* newnode = new node(data);

	if (head == NULL) {

		head = newnode;
		return;
	}

	node* temp = head;

	while (temp->next != NULL)
		temp = temp->next;

	temp->next = newnode;
	newnode->prev = temp;
}

void insertcall(node*& head) {

	insertatend(head, rand() % 1000);
	insertatend(head, rand() % 1000);
	insertathead(head, rand() % 1000);
	insertathead(head, rand() % 1000);
}

void initiatelist(node*& head, int n) {

	if (n == 0)
		return;

	insertathead(head, rand() % 1000);
	initiatelist(head, n - 1);
}

void print(node* head) {

	if (!head) {
		cout << endl;
		return;
	}

	if (head) {
		cout << head->data << " ";
		print(head->next);
	}
}

node* movetomiddle(node* head, int size, int i = 0) {

	if (i == size / 2) {
		return head;
	}

	head = head->next;
	movetomiddle(head, size, i + 1);
}

bool search(node* left, node* right, int key) {

	if (!left || !right)
		return 0;

	//cout << left->data << " " << right->data << endl;

	if (left->data == key || right->data == key) {
		return 1;
	}

	return search(left->prev, right->next, key);
}

bool searchinitiate(node* head, node* middle, int size, int key) {

	if (middle->data == key)
		return 1;

	print(head);
	cout << endl;

	if (search(middle->prev, middle->next, key)) {

		return 1;
	}
	else {

		insertcall(head);
		return searchinitiate(head, middle, size + 4, key);
	}
}

int main() {

	srand(time(0));

	node* head = NULL;
	int size = 5;
	int key = 0;

	do {
		cout << "Enter odd size to initiate list : ";
		cin >> size;
	} while (size <= 0 || size % 2 == 0);

	initiatelist(head, size);

	cout << "Initialized List : ";
	print(head);
	cout << endl;

	do {
		cout << "Enter key less than 1000 and greater than 0 : ";
		cin >> key;

	} while (key < 0 || key > 1000);

	node* middle = movetomiddle(head, size);

	if (searchinitiate(head, middle, size, key))
		cout << "\n\nKey Found\n\n";
}