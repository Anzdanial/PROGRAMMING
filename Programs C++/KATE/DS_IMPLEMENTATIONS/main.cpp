#include <iostream>
using namespace std;

template <class T>
class node{
public:
	T data;
	node *next;
	
	//Default Constructor Initializing node pointer
	node(){
		next = NULL;
	}
	
	//Parametrized Constructor Initializing node data and node pointer
	node(T val, node *ptr = NULL){
		data = val;
		next = ptr;
	}
};

template <class T>
class linkedListS{
public:
	node <T> *head, *tail;
	//Creating pointers to keep tabs of first node and last node.
	
	linkedListS(){
		head = tail = NULL;
	}
	
	~linkedListS(){
		node <T> *temp;
		while(!isEmpty()){
			temp = head->next;
			delete head;
			head = temp;
		}
		delete temp;
	}
	
	T isEmpty(){
		return (head == NULL);
	}
	
	void addToHead(T val){
		head = new node <T> (val, head);
		//Creating a new node by assigning the value of current head to the new pre-inserted node.
		
		//Evaluating if tail is NULL, which implies that the inserted node is a first node.
		if(tail == 0)
			tail = head;
	}
	
	void addToTail(T val){
		if(tail != 0){
			tail->next = new node <T> (val);
			tail = tail->next;
		}
		else
			head = tail = new node <T> (val);
	}
	
	T deleteFromHead(){
		int val = head -> data;
		node <T> *temp = head;
		if(head == tail)
			head = tail = NULL;
		else
			head = head->next;
		delete temp;
		return val;
	}
	
	T deleteFromTail(){
		int val = tail->data;
		if(head == tail){
			delete head;
			head = tail = 0;
		}
		else{
			node <T> *temp;
			for(temp = head; temp->next != tail; temp = temp->next);
			delete tail;
			tail = temp;
			tail->next = 0;
		}
		return val;
	}
	
	void deleteNode(T val){
		if(head != NULL){
			if(head == tail && val == head->data){
				delete head;
				head = tail = NULL;
			}
			else if(val == head->data){
				node <T> *temp = head;
				head = head->next;
				delete temp;
			}
			else{
				node <T> *pred, *temp;
				for(pred = head, temp = head->next; temp != 0 && !(temp->data == val); pred = pred->next, temp = temp->next);
				if(temp != 0){
					pred->next = temp->next;
					if(temp == tail)
						tail = pred;
					delete temp;
				}
			}
		}
	}
	
	bool isInList(int val) const {
		node <T> *temp;
		for(temp = head; temp != 0 && !(temp->data == val); temp = temp->next);
		return temp != 0;
	}
};

int main(){
	linkedListS <int> Anas;
	Anas.addToHead(25);
	cout<<Anas.isInList(25)<<endl;
	Anas.deleteFromHead();
	return 0;
}
