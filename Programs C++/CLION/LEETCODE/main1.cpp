#include <iostream>
using namespace std;

struct node{
	int data;
	node *next, *prev;
	node(){
		next = prev = nullptr;
	}
	node(int val){
		data = val;
		next = prev = nullptr;
	}
	node(int val, node *nptr, node *pptr){
		data = val;
		next = nptr;
		prev = pptr;
	}
};

class dlList{
	node *head, *tail;
public:
	dlList(){
		head = tail = nullptr;
	}

	void insertAtEnd(int val){
		if(head == nullptr)
			head = tail = new node(val);
	};


	~dlList(){

	}
};

int main(){
	cout<<"Hello World"<<endl;
	return 0;
}