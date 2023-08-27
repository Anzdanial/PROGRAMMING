#include <iostream>
using namespace std;

int stringCompare(char const* string1, char const *string2) {
	if (*string1 == '\0' && *string2 == '\0')
		return 0;
	else if (*string1 == '\0')
		return 1;
	else if (*string2 == '\0')
		return -1;

	if (*string1 == *string2)
		return stringCompare(string1 + 1, string2 + 1);
	else if (int(*string1) > int(*string2))
		return 1;
	else
		return -1;
}

int Fibonacci(int n){
	if(n == 0)
		return 0;
	if(n == 1)
		return 1;
	return Fibonacci(n-1)+ Fibonacci(n-2);
}

/*template <class T>
class linkedList{
	struct node{
		T data;
		node *next;
	};
	node *head, *tail;
public:
	linkedList(){
		head = tail = nullptr;
	}
	~linkedList(){}

	node createNewNode(){
		node *nn = new node;
		nn->data = 0;
		nn->next = nullptr;
	}

	node insertatHead(){

	}

};*/


int main(){
	/*int input;
	cout<<"Enter the Number: ";
	cin>>input;
	cout<<Fibonacci(input);*/

	cout<<stringCompare("anas","anas");

	return 0;
}