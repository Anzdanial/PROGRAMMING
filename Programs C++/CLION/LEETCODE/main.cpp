#include <iostream>
using namespace std;

class linkedList{
	struct node{
		int data;
		node *next;
		node(){
			data = 0;
			next = nullptr;
		}
		node(int val){
			data = val;
			next = nullptr;
		}
		node(int val, node *ptr){
			data = val;
			next = ptr;
		}
	};
	node *head, *tail;
public:
	linkedList(){
		head = tail = nullptr;
	}

	void insertAtEnd(int val){
		if(head == nullptr) {
			head = new node(val);
		}
		else{
			node *curr;
			for(curr = head; curr->next != nullptr; curr = curr->next);
			curr->next = new node(val);
		}

	}

	void printList(){
		for(node *curr = head; curr != nullptr; curr = curr->next)
			cout<<curr->data<<"->";
		cout<<endl;
	}

	void mergeTwoLists(node *list1, node *list2){
		node *list3, *curr1 = list1, *curr2 = list2, *curr3;
		if(curr1->data <= curr2->data)
			list3 = new node(curr1->data);
		else
			list3 = new node(curr2->data);
		curr3 = list3;
		while(curr)
	}

	~linkedList(){
		node *temp;
		for(node *curr = head; curr != nullptr; curr = curr->next){
			temp = curr->next;
			delete curr;
			curr = temp;
		}
		head = nullptr;
	}
};




int main(){
	linkedList l1;
	l1.insertAtEnd(1);
	l1.insertAtEnd(2);
	l1.printList();
	return 0;
}