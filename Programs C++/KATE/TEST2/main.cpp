#include <iostream>
using namespace std;

template <class T>
class node{
public:
	T data;
	node <T> *next;
	
	node(){
		next = NULL;
	}
	
	node(T el, node <T> *ptr = 0){
		data = el;
		next = ptr;
	}
};

template <class T>
class linkedList{
public:
	linkedList <T> *head, *tail;
};

int main(){
	return 0;
}
