#include <iostream>
using namespace std;

template <class T>
class linkedList{
	struct node {
		T data;
		node *next;
	};
	node *head, *tail;
public:
	linkedList(){
		head = tail = NULL;
	}

	~linkedList(){
		node *temp = head;

	}

	node *createNewNode(int val){
		node *nn = new node;
		nn->data = val;
		nn->next = nullptr;
		return nn;
	}

	void isEmpty(){
		return head == nullptr;
	}

	void insertatHead(int data){
		if(isEmpty())
			head = tail = createNewNode(data);
		else {
			node *temp = createNewNode(data);
			
			delete
		}
	}

};

int main(){

	return 0;
}