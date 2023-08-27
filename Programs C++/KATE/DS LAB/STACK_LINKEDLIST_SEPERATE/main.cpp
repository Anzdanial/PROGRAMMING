#include <iostream>
using namespace std;

template <class T>
class node{
public:
	T data;
	node *next;
	
	node(T data){
		data = NULL;
		next = NULL;
	}
	
	node(T val, node* nextPtr = 0){
		data = val;
		next = nextPtr;
	}
};

template <class T>
class linkedList{
	node <T> *head;
public:
	void insertathead(T val){
		if(isEmpty()){
			head = new node <T>(val,head);
		}
	}
	
	void isEmpty(){
		return head == 0;
	}
};

int main(){
	
	return 0;
}
