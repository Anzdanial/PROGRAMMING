#include <iostream>
using namespace std;

class node{
public:
	int data;
	node *next;
	
	node(){
		data = 0;
		next = NULL;
	}
	
	node(int info){
		this->data = info;
		this->next = NULL;
	}
};

class linkedList{
	node *head;
public:
	linkedList(){
		head = NULL;
	}
	
	void insertNode(int);
	void printList();
	void deleteNode(int);
	
	
};

void linkedList::deleteNode(int nodeAddress){
	node *temp1 = head, *temp2 = NULL;
	int listLen = 0;
	
	if(head == NULL){
		cout<<"List is empty."<<endl;
		return;
	}
		
	
	while (temp1 != NULL){
		temp1 = temp1->next;
		listLen++;
	}
	
	if(listLen < nodeAddress){
		cout<<"Index is out of range."<<endl;
		return;
	}
	
	temp1 = head;
	if(nodeAddress == 1){
		head = head->next;
		delete temp1;
		return;
	}
	
	while (nodeAddress-->1){
		temp2 = temp1;
		temp1 = temp1->next;
	}
	
	temp2->next = temp1->next;
	delete temp1;
}

void linkedList::insertNode(int data){
	node *newNode = new node(data);
	if(head == NULL){
		head = newNode;
		return;
	}
	
	node *temp = head;
	while(temp->next != NULL)
		temp = temp->next;
	
	temp->next = newNode;
}

void linkedList::printList(){
	node *temp = head;
	
	if(head == NULL){
		cout<<"List Empty"<<endl;
		return;
	}
	
	while(temp != NULL){
		cout<< temp->data <<" ";
		temp = temp->next;
	}
}

int main(){
	linkedList list;
  
    // Inserting nodes
    list.insertNode(1);
    list.insertNode(2);
    list.insertNode(3);
    list.insertNode(4);
  
    cout << "Elements of the list are: ";
  
    // Print the list
    list.printList();
    cout << endl;
  
    // Delete node at position 2.
    list.deleteNode(2);
  
    cout << "Elements of the list are: ";
    list.printList();
    cout << endl;
	return 0;
}
