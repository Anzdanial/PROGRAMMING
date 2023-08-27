#include <iostream>
using namespace std;

struct Node{
	Node *ptr;
	int data;
};

Node* createLL(int* array, int size)
{
	if (size == 0){
		std::cout << "Nothing Exists in the array. EXITING.....";
		exit(1);
	}
	Node *first = new Node;
	first->data = array[0];
	array++;
	size--;
	Node *node = first;
	for(int i=0;i<size;i++){
		node->ptr = new Node;
		node->ptr->data = array[i];
		node = node->ptr;
	}
	node->ptr = first;
	return first;
}

void printLL(Node *head, int count){
	for (Node *node = head; count-- > 0; node = node->ptr)
		std::cout << node->data << " ";
}

int main(){
	int arr[] = {69,420,6,9,4,2,0};
	Node* first = createLL(arr, 7);
	printLL(first, 10);
	return 0;
}