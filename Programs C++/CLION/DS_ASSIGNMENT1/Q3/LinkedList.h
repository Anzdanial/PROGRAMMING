#include<iostream>

class LinkedList{
private:
	struct Node{
		Node() : next(nullptr), prev(nullptr) {}
		Node(int v) : data(v), next(nullptr), prev(nullptr) {}
		Node* next;
		Node* prev;
		int data;
	};
	Node *head, *tail;

public:
	LinkedList() : head(nullptr), tail(nullptr) {}
	~LinkedList(){
		while (head){
			Node* next = head->next;
			delete head;
			head = next;
		}
	};

	void insert(int value){
		if(head == nullptr){
			head = new Node(value);
			head->next=nullptr;
			head->prev=nullptr;
			tail = head;
		}else{
			tail->next=new Node(value);
			tail->next->prev=tail;
			tail->next->next=nullptr;
			tail = tail->next;
		}
	}

	friend std::ostream& operator <<(std::ostream& ostr, const LinkedList& rhs);

	Node *extract(int index){
		if (index < 0)
			return nullptr;

		Node *node = head;
		for (int i = 0; i < index && node; i ++, node = node->next);
		if (!node)
			return nullptr;
		if (node == head){
			head = node->next;
			if (head)
				head->prev = nullptr;
		}else if (node == tail){
			tail = tail->prev;
			if (tail)
				tail->next = nullptr;
		}else{
			node->prev->next = node->next;
			node->next->prev = node->prev;
		}
		node->next = nullptr;
		node->prev = nullptr;
		return node;
	}

	bool insert(int index, Node *node){
		if (index < 0)
			return false;

		Node *after = head;
		for (int i = 0; i < index && after; i ++, after = after->next);

		if (after == head){
			node->prev = nullptr;
			node->next = head;
			head->prev = node;
			head = node;
		}else if (after == nullptr){
			node->next = nullptr;
			node->prev = tail;
			tail->next = node;
			tail = node;
		}else{
			node->prev = after->prev;
			node->prev->next = node;
			node->next = after;
			after->prev = node;
		}
		return true;
	}

	void swap(int x,int y){
		if (x > y){
			int temp = x;
			x = y;
			y = temp;
		}
		if (x == y)
			return;
		Node *b = extract(y), *a = extract(x);
		insert(x, b);
		insert(y, a);
	}


};

inline std::ostream& operator <<(std::ostream& ostr, const LinkedList& rhs){
	auto *current = rhs.head;
	while(current != nullptr){
		ostr << current->data <<" ";
		current = current->next;
	}
	return ostr;
}