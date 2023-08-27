#include <iostream>
using namespace std;

int sumFibonacci(int n){
	if( n == 0)
		return 0;
	if (n==1)
		return 1;
	int prev = 1, forward=1;
	for(int i=2; i<n; i++){
		int temp = forward;
		forward +=prev;
		prev = temp;
	}
	return forward;
}

int main(){
	int input1;
	cout<<"Enter the Index of Series: ";
	cin>>input1;
	cout<<"The Sum of Series upto "<<input1<<" is "<<sumFibonacci(input1)<<endl;;
	return 0;
}
