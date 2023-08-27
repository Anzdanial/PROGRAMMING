#include "LinkedList.h"

LinkedList::LinkedList():head(nullptr){}

LinkedList::~LinkedList(){}

void LinkedList::insert(int value){

    //empty case

    if(head == nullptr){
        head = new Node(value);
        head->next=nullptr;
    }
    else if(head->next== nullptr){
        Node *temp = new Node(value);
        head->next = temp;
    }
    else{

        //non empty case
        Node *current = head;
        while(current->next->next != nullptr){
            current = current->next;
        }
        Node *temp = new Node(value,current);
        temp->next = current->next;
        current->next = temp;
        temp->next->next=nullptr;
    }

}

std::ostream& operator <<(std::ostream& ostr, const LinkedList& rhs){
    Node *current = rhs.head;
    while(current != nullptr){
        std :: cout<< current->data <<" ";
        current = current->next;
    }
    return ostr;
}

void LinkedList::mergeList(const LinkedList &from) {
    Node *current = head;
    while (current -> next != nullptr)
        current = current->next;
	Node *node = from.head;
	if (node == nullptr)
		return;
	if (head == nullptr){
		head = new Node(from.head->data);
		current = head;
	}
	while (node){
		current->next = new Node(node->data);
		current = current->next;
		node = node->next;
	}
}


void LinkedList::del(){

    //empty list
    if(head->next==nullptr){
        delete head;
        head = nullptr;
    }
    else{
        Node *current = head;
        current = current->next;
        head->next=current->next;
        delete(current);
    }
}


Node* LinkedList::gethead(){
    return head;
}

void LinkedList::compress(){
    Node *start = head;
    Node *current = head;
    while(start!=nullptr){
        int x = start->data;

        while(current->next!=nullptr){

            if(current->next->data==x){

                Node*temp = current->next;
                current->next = current->next->next;
                delete(temp);
            }
            else
                current = current->next;
        }
        start = start->next;
        current = start;
    }

}








