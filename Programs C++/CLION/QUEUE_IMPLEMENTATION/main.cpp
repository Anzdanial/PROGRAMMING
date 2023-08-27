#include <iostream>
using namespace std;

template <class T>
class node{
public:
	T data;
	node <T> next;
};

template <class T>
class LinkedList{
private:
	node <T> *head, *tail;
	int counter = 0;
public:
	LinkedList(){
		head = tail = nullptr;
		counter = 0;
	}

	//Copy Constructor
	LinkedList(const LinkedList &from){
		head = tail = nullptr;
		counter = 0;
		for(auto itr = from.begin(); itr != from.end(); ++itr)
			this->append(*itr);
	}

	~LinkedList(){
		while(head){
			node <T> *next = head->next;
			delete head;
			head = next;
		}
	}

	class iterator{
		friend class LinkedList;
		node <T> *inode;
	public:
		//Constructor
		iterator(node <T> *inode) : inode <T> (inode){}
		//Copy Constructor
		iterator(const iterator &from) : node <T> (from)

	};
};

