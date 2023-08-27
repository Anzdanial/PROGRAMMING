//#include <iostream>
//using namespace std;
//
//template <typename T>
//struct Node
//{
//	Node<T>* link;
//	T data;
//
//	Node(T dataInput) {
//		link = NULL;
//		data = dataInput;
//	}
//	Node() {
//		link = NULL;
//	}
//};
//
//template <typename T>
//class List {
//private:
//	Node<T>* head;
//public:
//
//	List() {
//		head = NULL;
//	}
//	List(const List& obj) {
//
//		if (obj.head == NULL) {
//			head = NULL;
//			return;
//		}
//
//		Node<T>* temp = obj.head;
//
//		while (temp != NULL) {
//
//			Node<T>* newNode = new Node<T>(temp->data);
//
//			if (head != NULL) {
//				Node<T>* tempH = head;
//
//				while (tempH->link != NULL) {
//					tempH = tempH->link;
//				}
//
//				tempH->link = newNode;
//			}
//
//			if (temp == obj.head) {
//				this->head = newNode;
//			}
//
//			temp = temp->link;
//
//		}
//
//	}
//	~List() {
//
//	}
//
//	class iterator {
//		friend class List;
//
//	private:
//		Node<T>* Nodeptr;
//	public:
//		iterator() {
//			Nodeptr = NULL;
//		}
//
//		iterator(Node<T>* INode) : Nodeptr(INode) {}
//
//		T& operator*() const {
//			return Nodeptr->data;
//		}
//
//		iterator operator++(int) {
//			iterator temp = *this;
//			Nodeptr = Nodeptr->link;
//			return temp;
//		}
//
//		iterator operator++() {
//			iterator temp = *this;
//			Nodeptr = Nodeptr->link;
//			return temp;
//		}
//
//		bool operator!=(const iterator& tempItr) {
//			if (this->Nodeptr != tempItr.Nodeptr) {
//				return 1;
//			}
//
//			return 0;
//		}
//
//		bool operator==(const iterator& tempItr) {
//			if (this->Nodeptr == tempItr->Nodeptr) {
//				return 1;
//			}
//
//			return 0;
//		}
//
//		void operator=(Node<T>*& value) {
//
//			Node<T>* newNode = newNode(value->data);
//
//			this->Nodeptr = value;
//		}
//
//		T& operator[](int num) {
//
//			int count = 0;
//
//			Node<T>* temp = Nodeptr;
//
//			while (temp != NULL && count != num) {
//
//				if (count == num) {
//					return temp->data;
//				}
//				temp = temp->link;
//				count++;
//			}
//
//			return this->Nodeptr->data;
//		}
//	};
//
//	iterator begin() const {
//		return iterator(head);
//	}
//
//	iterator end() const {
//		Node<T>* temp = head;
//
//		if (temp == head) {
//			temp = NULL;
//		}
//		else {
//			while (temp->link != NULL)
//				temp = temp->link;
//		}
//
//		return iterator(temp);
//	}
//
//	void insertAtHead(T const elemenet) {
//
//		if (head == NULL) {
//
//			Node<T>* newNode = new Node<T>(elemenet);
//			head = newNode;
//			return;
//		}
//
//		Node<T>* newNode = new Node<T>(elemenet);
//
//		newNode->link = head;
//		head = newNode;
//
//	}
//
//	void deleteAtStart() {
//
//		if (this->head != NULL) {
//			this->erase(this->begin());
//		}
//	}
//
//	void print() {
//
//		Node<T>* temp = head;
//
//		while (temp != NULL) {
//			cout << temp->data << "  ";
//			temp = temp->link;
//		}
//
//		cout << endl;
//	}
//
//	void erase(const iterator& itr) {
//
//		if (itr.Nodeptr != NULL) {
//			this->head = this->head->link;
//		}
//	}
//
//	int getSize() {
//
//		int count = 0;
//		Node<T>* temp = head;
//
//		while (temp != NULL) {
//			temp = temp->link;
//			count++;
//		}
//
//		return count;
//	}
//
//	bool isEmpty() {
//		if (head == NULL)
//			return 1;
//		return 0;
//	}
//
//};
//
//template<typename T>
//class Stack {
//private:
//	List<T>* listH;
//public:
//	Stack() {
//		listH = new List<T>();
//	}
//	void push(T value) {
//
//		listH->insertAtHead(value);
//	}
//	T pop() {
//
//		if ((listH->begin() != listH->end()))
//		{
//			T newVal = *(listH->begin());
//			listH->erase(listH->begin());
//			return newVal;
//		}
//
//		cout << endl << "Memory error !!! removing from NULLPTR" << endl;
//		exit(0);
//	}
//	void display() {
//
//		for (List<char>::iterator i = listH->begin(); i != listH->end(); i++) {
//			cout << *i << "  ";
//		}
//
//		cout << endl;
//	}
//	bool Top(T& val) {
//		if (!(listH->begin() != listH->end()))
//			return 0;
//
//		val = *listH->begin();
//		return 1;
//	}
//	T peek() {
//		if (!(listH->begin() != listH->end()))
//			return 0;
//
//		T val = *listH->begin();
//		return val;
//	}
//	bool isEmpty() {
//		if (listH->begin() != listH->end())
//			return 0;
//		return 1;
//	}
//	int size() {
//		return this->listH->getSize();
//	}
//	void removeAll(const T& ch) {
//
//		Stack<char> temp;
//
//		for (List<char>::iterator i = listH->begin(); i != listH->end(); i++) {
//
//			if (*i != ch) {
//				temp.push(*i);
//			}
//
//		}
//
//		while (listH->begin() != NULL) {
//			this->pop();
//		}
//
//		while (temp.listH->begin() != NULL) {
//			this->push(temp.pop());
//		}
//	}
//	void removeAdjacent(const T& ch) {
//
//		Stack<char> temp;
//		int count = 0;
//
//		for (List<char>::iterator i = listH->begin(); i != listH->end(); i++) {
//
//			temp.push(*i);
//
//			if (*i == ch) {
//				count++;
//			}
//			else if(*i != ch) {
//				count = 0;
//			}
//
//			if (count == 3) {
//
//				while (count > 0) {
//					temp.pop();
//					count--;
//				}
//			}
//
//		}
//
//		while (listH->begin() != NULL) {
//			this->pop();
//		}
//
//		while (temp.listH->begin() != NULL) {
//			this->push(temp.pop());
//		}
//	}
//};
//
//
//int main() {
//
//	Stack<char>s;
//
//	s.push('v');
//	s.push('v');
//	s.push('v');
//	s.push('a');
//	s.push('b');
//	s.push('v');
//	s.push('v');
//	s.push('c');
//	s.push('d');
//	s.push('v');
//	s.push('v');
//	s.push('v');
//	s.display();
//	//s.removeAll('v');
//	s.removeAdjacent('v');
//	s.display();
//
//}