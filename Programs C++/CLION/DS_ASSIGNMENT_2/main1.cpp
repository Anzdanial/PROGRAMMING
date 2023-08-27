#include <iostream>
#include "LinkedList.h"
using namespace std;

template <class T>
struct node{
	T data;
	node *next, *prev;
};

template <class T>
node <T> *searchmiddle(node<T> *left,node <T> *right, const T&key){
	if(left -> data == key)
		return left;
	if(right->data == key)
		return right;
	return searchmiddle(left->prev, right->next, key);
}

template <class T>
node<T> *searchmiddle(node<T> *middle, const T&key){
	return searchmiddle(middle, middle->next, key);
}



int main(){
	return 0;
}