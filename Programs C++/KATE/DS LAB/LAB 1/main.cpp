#include <iostream>
using namespace std;

class myVector{
private:	
	int *arrayPtr;
	int capacity;
	int currentSize;
public:
	myVector(){
		arrayPtr = new int [2];
		for(int i = 0; i < 2; i++)
			arrayPtr[i] = 0;
		capacity = 2;
		currentSize = 0;
	}
	myVector(int length){
		arrayPtr = new int [length];
		for(int i = 0; i < length; i++)
			arrayPtr[i] = 0;
		capacity = length;
		currentSize = 0;
	}
	
	myVector &operator +(int num){
		if(capacity==currentSize){
			int *newarr = new int [capacity*2];
			for(int i = 0; i < capacity; i++)
				newarr[i] = arrayPtr[i];
			newarr[currentSize]=num;
			delete []arrayPtr;
			arrayPtr=newarr;
			currentSize++;
			capacity=capacity*2;
		}
		else{
			arrayPtr[currentSize]=num;
			currentSize++;
		}
		return *this;
	}
	
	myVector &operator--(){
		if(currentSize==0)
			return *this;
		else{
			currentSize--;
			arrayPtr[currentSize]=0;
			return *this;
		}
	}
	
	myVector(myVector &copy){
		capacity=copy.capacity;
		arrayPtr = new int[capacity];
		for(int i=0; i<capacity; i++)
			arrayPtr[i]=copy.arrayPtr[i];
		currentSize=copy.currentSize;
	}
	
	int gettotalcap(){
		return capacity;
	}
	
	void print(){
		for(int i=0; i<currentSize; i++)
			cout<<arrayPtr[i]<<" ";
		cout<<endl;
		cout<<"Current Size is "<<currentSize<<endl;
	}
	
	
	
	~myVector(){
		delete []arrayPtr;
	}
};


int main(){
	myVector Anas(25);
	Anas+1;
	Anas.print();
	//myVector Anas2(Anas);
	--Anas;
	Anas.print();
	//cout<<"The Anas2: ";
	//Anas2.print();
	return 0;
}
