#include <iostream>
using namespace std;

template <class T>
class node{
public:
	T data;
	node *next;
	
	node(){
		data = 0;
		next = NULL;
	}
	
	node(T info, node *nextPtr){
		data = info;
		next = nextPtr;
	}
	
	node(T info){
		data = info;
		next = NULL;
	}
};

template <class T>
class linkedList{
public:
	node <T> *head, *tail;
	
	linkedList(){
		head = tail = NULL;
	}
	
	void addToHead(int);
	void addToTail(int);
	int deleteFromHead();
	int deleteFromTail();
	void deleteNode(int);
	bool isInList(int) const;
	int isEmpty();
	
	~linkedList(){
		node *p;
		while(!isEmpty()){
			p = head->next;
			delete head;
			head = p;
		}
	}
};

int linkedList::isEmpty(){
	if(head == 0)
		return true;
	else
		return false;
}

void linkedList::addToHead(int el){
	head = new node (el,head);
	if(tail == 0)
		tail = head;
}
	void addToTail(int);
	int deleteFromHead();
	int deleteFromTail();
	void deleteNode(int);
	bool isInList(int) const;


int main(){
	return 0;
}
