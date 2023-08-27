#include <iostream>
using namespace std;

template <class T>
class LinkedList_TAIL{
	struct node{
		T data;
		node *next;
		node(){
			data = 0;
			next = nullptr;
		}
	};
	node *head, *tail;
public:
	LinkedList_TAIL(){
		head = tail = nullptr;
	}

	~LinkedList_TAIL(){
		node *deleteptr;
		for(node *temp = head; temp!=tail; temp = temp->next) {
			deleteptr = temp;
			delete deleteptr;
		}
	}

	node *createNode(){
		node *nn = new node;
		nn->data = 0;
		nn->next = nullptr;
		return nn;
	}

	void insertion(T val){
		if(head == nullptr)
			return;
		else {
			node *curr;
			node *temp = createNode();
			for(node *curr = head; head)
			head->next = temp;
			temp->data = val;
			temp->next = nullptr;
			tail = temp;
			cout<<"Inserted"<<endl;
		}
	}

	void printList(){
		for(node *temp = head; temp!=tail; temp = temp->next)
			cout<<temp->data<<"->";
		cout<<endl;
	}
};

int main(){
	LinkedList_TAIL <int> list;
	list.insertion(1);
	list.insertion(2);
	list.printList();
	return 0;
}