//DIRECT RECURSIONS:

#include <iostream>
using namespace std;

int factorial(int data){
	if(data > 1)
		return (data * factorial (data-1) );
	else
		return 1;
}

int fib(int n){
	if(n <= 1){
		return n;
	}
	return fib(n-1)+fib(n-2);
	int num;
	cin>>num;
	for(int i=0; i<num; i++){
		cout<<fib(i)<<" ";
	}
	return 0;
}

int main(){
	int input;
	cout<<"Enter the Value to Calculate Factorial: ";
	cin>>input;
	cout<<factorial(input)<<" ";
	cout<<endl;
	cout<<fib(input)<<" ";
	cout<<endl;
	return 0;
}
