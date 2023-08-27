#include <iostream>
using namespace std;

template <typename T>
struct Node
{
	Node<T>* link;
	T data;

	Node(T dataInput) {
		link = NULL;
		data = dataInput;
	}
	Node() {
		link = NULL;
	}
};

template <typename T>
class List {
private:
	Node<T>* head;
public:

	List() {
		head = NULL;
	}
	List(const List& obj) {

		if (obj.head == NULL) {
			head = NULL;
			return;
		}

		Node<T>* temp = obj.head;

		while (temp != NULL){

			Node<T>* newNode = new Node<T>(temp->data);

			if (head != NULL) {
				Node<T>* tempH = head;

				while (tempH->link != NULL){
					tempH = tempH->link;
				}

				tempH->link = newNode;
			}

			if (temp == obj.head) {
				this->head = newNode;
			}

			temp = temp->link;

		}

	}
	~List() {

	}

	class iterator {
		friend class List;

	private:
		Node<T>* Nodeptr;
	public:
		iterator() {
			Nodeptr = NULL;
		}

		iterator(Node<T>* INode) : Nodeptr(INode) {}

		T& operator*() const {
			return Nodeptr->data;
		}

		iterator operator++(int) {
			iterator temp = *this;
			Nodeptr = Nodeptr->link;
			return temp;
		}

		iterator operator++() {
			iterator temp = *this;
			Nodeptr = Nodeptr->link;
			return temp;
		}

		bool operator!=(const iterator& tempItr) {
			if (this->Nodeptr != tempItr.Nodeptr) {
				return 1;
			}

			return 0;
		}

		bool operator==(const iterator& tempItr) {
			if (this->Nodeptr == tempItr->Nodeptr) {
				return 1;
			}

			return 0;
		}

		void operator=(Node<T>*& value) {

			Node<T>* newNode = newNode(value->data);

			this->Nodeptr = value;
		}

		T& operator[](int num) {

			int count = 0;

			Node<T>* temp = Nodeptr;

			while (temp != NULL && count != num) {

				if (count == num) {
					return temp->data;
				}
				temp = temp->link;
				count++;
			}

			return this->Nodeptr->data;
		}
	};

	iterator begin() const {
		return iterator(head);
	}

	iterator end() const {
		Node<T>* temp = head;

		if (temp == head) {
			temp = NULL;
		}
		else {
			while (temp->link != NULL)
				temp = temp->link;
		}

		return iterator(temp);
	}

	void insertAtHead(T const elemenet) {

		if (head == NULL) {

			Node<T>* newNode = new Node<T>(elemenet);
			head = newNode;
			return;
		}

		Node<T>* newNode = new Node<T>(elemenet);

		newNode->link = head;
		head = newNode;

	}

	void deleteAtStart() {

		if (this->head != NULL) {
			this->erase(this->begin());
		}
	}

	void print() {

		Node<T>* temp = head;

		while (temp != NULL){
			cout << temp->data << "  ";
			temp = temp->link;
		}

		cout << endl;
	}

	void erase(const iterator& itr) {

		if (itr.Nodeptr != NULL) {
			this->head = this->head->link;
		}
	}

	int getSize() {

		int count = 0;
		Node<T>* temp = head;

		while (temp != NULL) {
			temp = temp->link;
			count++;
		}

		return count;
	}

	bool isEmpty() {
		if (head == NULL)
			return 1;
		return 0;
	}

};

template<typename T>
class Stack {
private:
	List<T>* listH;
public:
	Stack() {
		listH = new List<T>();
	}
	void push(T value) {

		listH->insertAtHead(value);
	}
	T pop() {

		if ((listH->begin() != listH->end()))
		{
			T newVal = *(listH->begin());
			listH->erase(listH->begin());
			return newVal;
		}

		cout << endl << "ERROR!" << endl;
		exit(0);
	}
	void display() {

		for (List<int>::iterator i = listH->begin(); i != listH->end(); i++) {
			cout << *i << "  ";
		}

		cout << endl;
	}
	bool Top(T& val) {
		if (!(listH->begin() != listH->end()))
			return 0;

		val = *listH->begin();
		return 1;
	}
	T peek() {
		if (!(listH->begin() != listH->end()))
			return 0;

		T val = *listH->begin();
		return val;
	}
	bool isEmpty() {
		if (listH->begin() != listH->end())
			return 0;
		return 1;
	}
	int size() {
		return this->listH->getSize();
	}
};

int main() {

	Stack<int>s;

	s.push(1);
	s.push(2);
	s.push(3);

	int v = 0;
	s.display();

	cout << endl;

	for (int i = 0; i < 3; i++) {
		s.Top(v);
		cout << v << "  ";
	}

	cout << endl;
	for (int i = 0; i < 4; i++) {
		cout << s.pop() << "  ";
	}
	cout << endl;
	v = -1;
	s.Top(v);

	cout << v;

}