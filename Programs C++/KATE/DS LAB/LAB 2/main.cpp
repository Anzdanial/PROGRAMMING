#include <iostream>
using namespace std;

template <class T>
class Node{
public:
	T data;
	Node <T> *next;
	Node <T> *prev;
};

template <class T>
class doubleLinkedList {
private:
	Node <T> *head;
	Node <T> *tail;
	
public:
	doubleLinkedList(){
		this->head = NULL;
		this->tail = NULL;
	}
	
	void insertAtStart(T const element){
		Node <T> *node = new Node <T>;
		node->data = element;
		if(head == NULL){
			head = node;
			cout<<"New Node added. (FIRST)"<<endl;
		}
		node->next = head;
		head = node;
		cout<<"Node added at the Start."<<endl;
	}
	
	void insertAtEnd(T const element){
		Node <T> *node = new Node <T>;
		node->data = element;
		if(tail == NULL){
			head = node;
			tail = node;
			cout<<"New Node added. (LAST)"<<endl;
		}
		node->next = tail;
		tail = node;
		cout<<"Node added at the End."<<endl;
	}
	
	void printForward() const{
		Node <T> *current = head;
		while(current != NULL){
			cout<< current->data <<endl;
		}
	}
};


int main(){
	
	return 0;
}
