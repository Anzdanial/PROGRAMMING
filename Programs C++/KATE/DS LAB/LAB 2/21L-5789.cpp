#include<iostream>
using namespace std;

template <class t>
class node {
public:
	t data;
	node* prev;
	node* next;
};

template <class t>
class doubleLinkedList {
public:
	node <t>* head = NULL;
	node <t>* tail = NULL;

	//Initialzing Nodes;
	node <t>* create(t val) {
		node<t>* nn = new node <t>;
		nn->data = val;
		nn->prev = NULL;
		nn->next = NULL;
		return nn;
	}

	void insertBefore(t val, t key) {
		node<t>* temp = head;
		while (temp->next->data != val) {
			if (temp->next->next == tail) { 
				insertAtEnd(key); 
				return; 
			}
			temp = temp->next;
		}
		node<t>* nn = create(key);
		nn->prev = temp;
		nn->next = temp->next;
		temp->next->prev = nn;
		temp->next = nn;
	}


	void insertAfter(t val, t key) {
		node<t>* temp = tail;
		while (temp->prev->data != val) {
			if (temp->prev->prev == head) {
				insertAtEnd(key); return;
			}
			temp = temp->prev;
		}

		node<t>* newNode = create(key);

		newNode->next = temp;
		newNode->prev = temp->prev;
		temp->prev->next = newNode;
		temp->prev = newNode;

	}

	void insertAtStart(t val) {
		//Creating New Node for Insertion
		node <t>* nn = create(val);
		if (head == NULL) {
			head = nn; //Assigning address of New Node 
			tail = nn; //Assigning address of New Node
			return;
		}
		//Inserting New Nodes if not the starting node.
		else {
			head->prev = nn;
			nn->next = head;
			head = nn;
		}
	}

	void insertAtEnd(t val) {
		node<t>* nn = create(val);
		node<t>* ln = tail;
		ln->next = nn;
		nn->prev = ln;
		tail = nn;
	}

	void deleteFromStart() {
		if (head == NULL) //Evaluating if head is still at start
			return;
		if (tail == head) { //Evaluating the Number of Nodes
			head = NULL;
			tail = NULL;
			return;
		}
		node<t>* temp = head;
		head = head->next;
		head->prev = 0;
		delete temp;
	}

	void deleteFromEnd() {
		if (tail == NULL)
			return;
		if (tail == head) {
			head = NULL;
			tail = NULL;
			return;
		}
		node<t>* temp = tail;
		tail = tail->prev;
		tail->next = 0;
		delete temp;

	}

	int size() {
		node<t>* temp = head;
		int sum = 0;
		while (temp != NULL) {
			sum++;
			temp = temp->next;
		}
		return sum;
	}

	node<t>* returnMiddle() {
		node<t>* start = head;
		node<t>* end = tail;
		while (start != end) { 
			if (start->prev == end)
				return end;
			start = start->next;
			end = end->prev;
		}
		return start;
	}


	bool isEmpty() {
		return (tail == NULL || head == NULL);
	}
	
	void printF() {
		node<t>* temp = head;
		while (temp != NULL) {
			cout << temp->data << " ";
			temp = temp->next;
		}
		cout << endl;
	}

	void printR() {
		node<t>* temp = tail;
		while (temp != NULL) {
			cout << temp->data << " ";
			temp = temp->prev;
		}
		cout << endl;
	}

	~doubleLinkedList <t> (){
		node<t>*temp1 = head;
		node<t>* temp2 = head;

		while (temp1 != NULL) {
			temp2 = temp1;
			temp1 = temp1->next;
			delete temp2;
		}
	}
};


bool isPalindrome(const char* arr) {
	doubleLinkedList<char> list;
	for (int i = 0; arr[i] != '\0'; i++) {
		list.insertAtStart(arr[i]);
	}

	list.printR();
	list.printF();

	node<char>* temp1 = list.head;
	node<char>* temp2 = list.tail;
	while (temp1 != NULL && temp2!=NULL) {
		if (temp1->data != temp2->data)
			return false;
		temp1 = temp1->next;
		temp2 = temp2->prev;
	}
	return true;
}

int main() {
	doubleLinkedList<int> list;
	
	list.insertAtStart(70);
	list.insertAtStart(25);
	list.insertAtEnd(111);
	list.insertAtEnd(69);
	list.insertBefore(260, 125);
	list.insertAfter(32, 300);
	
	//Evaluating Size of Array
	cout << "Size: ";
	cout<<list.size() << endl;
	cout << "Array: ";  
	list.printF(); cout << endl;

	//Evaluating Value at the Middle
	if (list.returnMiddle() != NULL) 
		cout << "Middle: ";  
		cout << list.returnMiddle()->data << endl;
	list.deleteFromEnd();
	list.deleteFromStart();
	
	cout << "After Deleting Array: ";  
	list.printF();
	cout << endl;
	
	cout << "Deleting All Elements: ";
	list.deleteFromStart(); 
	list.deleteFromStart(); 
	list.deleteFromStart(); 
	list.deleteFromStart();

	list.printF(); 
	cout << endl;

	
	//Testing Palindrome
	const char* tester = "REVIVER";
	bool flag;
	flag = isPalindrome(tester);
	if (flag) 
		cout << "Palindrome" << endl;
	else  
		cout << "Not Palindrome!" << endl;

	return 0;
}
