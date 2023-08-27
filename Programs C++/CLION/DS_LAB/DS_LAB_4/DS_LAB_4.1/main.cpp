#include <iostream>

using namespace std;

template<typename T>
struct Node {
	Node *next;
	Node *prev;
	T data;

	Node(T dataInput) {
		next = NULL;
		prev = NULL;
		data = dataInput;
	}

	Node() {
		next = NULL;
		prev = NULL;
	}
};

template<class T>
class List {
private:
	Node<T> *head;
	Node<T> *tail;
public:

	List() {
		head = NULL;
		tail = NULL;
	}

	List(const List &obj) {
		if (obj.head == NULL) {
			head = tail = NULL;
			return;
		}

		Node<T> *temp = obj.head;

		do {
			Node<T> *newNode = new Node<T>(temp->data);
			if (head != NULL) {
				Node<T> *tempH = head;
				while (tempH->next != NULL) {
					tempH = tempH->next;
				}
				tempH->next = newNode;
				newNode->prev = temp;
				tail = newNode;
			}
			if (temp == obj.head) {
				this->head = newNode;
			} else if (temp == obj.tail) {
				this->tail = newNode;
			}
			temp = temp->next;
		} while (temp != obj.head);
		head->prev = tail;
		tail->next = head;
	}

	~List() {}

	class iterator {
		friend class List;

	private:
		Node<T> *Nodeptr;
	public:
		iterator() {
			Nodeptr = NULL;
		}

		iterator(Node<T> *INode) : Nodeptr(INode) {}

		T &operator*() const {
			return Nodeptr->data;
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

		bool operator!=(const iterator &tempItr) {
			if (this->Nodeptr != tempItr.Nodeptr) {
				return 1;
			}
			return 0;
		}

		bool operator==(const iterator &tempItr) {
			if (this->Nodeptr == tempItr->Nodeptr) {
				return true;
			}
			return false;
		}

		void operator=(Node<T> *&value) {
			Node<T> *newNode = newNode(value->data);
			this->Nodeptr = value;
		}

		T &operator[](int num) {
			int count = 0;
			Node<T> *temp = Nodeptr;
			while (temp != NULL && count != num) {
				if (count == num) {
					return temp->data;
				}
				temp = temp->next;
				count++;
			}

			return this->Nodeptr->data;
		}
	};

	iterator begin() const {
		return iterator(head);
	}

	iterator end() const {
		return iterator(tail);
	}

	void insertAtTail(T const elemenet) {

		if (head == NULL) {

			Node<T> *newNode = new Node<T>(elemenet);
			head = tail = newNode;
			tail->next = head;
			head->prev = tail;
			return;
		}

		Node<T> *newNode = new Node<T>(elemenet);

		Node<T> *temp = head;

		while (temp->next != head) {
			temp = temp->next;
		}

		newNode->prev = temp;
		temp->next = newNode;
		tail = newNode;
		tail->next = head;
		head->prev = tail;

	}

	void RemoveCLL() {

		if (head == NULL)
			return;

		Node<T> *temp = head;

		do {

			int num = temp->data;
			int sum = 0;

			while (num > 0) {
				sum = sum + num % 10;
				num = num / 10;
			}

			if (sum % 2 == 0) {

				if (temp == head) {

					Node<T> *tempNext = temp->next;

					head = tempNext;
					tail->next = head;
					head->prev = tail;

					tempNext = temp;
					temp = temp->next;
					delete tempNext;
				} else if (temp == tail) {

					Node<T> *tempCurrent = temp;

					temp->prev->next = head;
					head->prev = temp->prev;
					tail = temp;
					delete temp;
				} else {

					Node<T> *tempCurrent = temp;

					temp->prev->next = temp->next;
					temp->next->prev = temp->prev;

				}
			}

			temp = temp->next;
		} while (temp != tail->next);

		temp = head;
		int product = 1;
		int sum = 0;

		for (List<int>::iterator i = this->begin(); i != this->end(); i++) {
			sum = sum + i.operator*();
			product = product * i.operator*();
		}


		cout << "Sum : " << sum << endl;
		cout << "Product : " << product << endl;
	}

	void print() {

		Node<T> *temp = head;

		do {
			cout << temp->data << "  ";
			temp = temp->next;
		} while (temp != head);

		cout << endl;
	}

	void MoveOccurance(T key) {

		int count = 0;

		for (List<int>::iterator i = this->begin(); i != this->end(); i++) {

			if (key == i.operator*()) {
				count++;
				erase(i);
			}
		}

		while (count > 0) {
			this->insertAtTail(key);
			count--;
		}
	}

	void erase(const iterator &itr) {

		itr.Nodeptr->prev->next = itr.Nodeptr->next;
		itr.Nodeptr->next->prev = itr.Nodeptr->prev;
	}

	int getSize() {

		int count = 0;
		Node<T> *temp = head;
		do {
			temp = temp->next;
			count++;
		} while (temp != head);

		return count;
	}

	bool isEmpty() {
		if (head == NULL)
			return true;
		return false;
	}

};

int main() {

	List<int> L1;

	L1.insertAtTail(1);
	L1.insertAtTail(2);
	L1.insertAtTail(3);
	L1.insertAtTail(3);
	L1.insertAtTail(43);
	L1.insertAtTail(55);
	L1.insertAtTail(6);
	L1.insertAtTail(7);

	L1.print();

	L1.RemoveCLL();

	L1.print();

	L1.MoveOccurance(3);

	L1.print();
	cout << L1.getSize() << endl;

	List<int> L2(L1);

	L2.print();
}