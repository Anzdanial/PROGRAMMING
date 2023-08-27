#include <iostream>
using namespace std;

template <typename T>
struct node{
	T data;
	node <T> *next;
	node <T> *prev;
};

template <typename T>
class CDL{
	node <T> *head, *tail;
	
	class iterator{
	private:
		node <T> *nodePtr;
	public:
		iterator(){
			nodePtr = NULL;
		}
		
		bool operator !=(const iterator &itr) const{
			return (nodePtr != itr.nodePtr);
		}
		
		bool operator ==(const iterator &itr) const{
			return (nodePtr == itr.nodePtr);
		}
		
		T &operator =(){
			ret
		}
		
		T &operator *() const{
			return nodePtr -> next ->data;
		}
		
		iterator operator++(){
			if(nodePtr)
				nodePtr = nodePtr->next;
			return *this;
		}
		
		iterator operator++(int){
			iterator temp = *this;
			nodePtr = nodePtr->next;
			return temp;
		}
	};
	
	iterator begin() const{
		return head;
	}
	
	iterator end() const{
		return tail;
	}
};

int main(){
	
	return 0;
}
