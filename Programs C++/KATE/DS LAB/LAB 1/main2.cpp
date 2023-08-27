#include <iostream>
using namespace std;

void SelectionSort(int *array, int size){
	for(int i=0; i<size-1; i++){
		int least=i;
		for(int j=i+1; j<size; j++){
			if(array[j]<array[least])
				least=j;
		}
		int temp=array[least];
		array[least]=array[i];
		array[i]=temp;
	}
}

void grow(int *&array, int &size){
	int *newarray = new int [size+1];
	for(int i=0; i<size+1; i++)
		newarray[i]=array[i];
	delete []array;
	array=newarray;
	size++;
}

int main(){
	int *arrayPtr, size;
	cout << "Enter the size of the array: ";
	cin >> size;
	arrayPtr = new int [size];
	
	cout<<"Enter the Elements of the Array: ";
	for(int i = 0; i < size; i++){
		cin >> arrayPtr [i];
	}
	cout<<endl;
	
	cout<<"Array: ";
	for(int i = 0; i < size; i++){
		cout << arrayPtr [i] << " ";
	}
	cout<<endl;
	
	SelectionSort(arrayPtr,size);
	
	cout<<"The Sorted array: ";
	for(int i = 0; i < size; i++){
		cout << arrayPtr[i] << " ";
	}
	cout<<endl<<endl;
	
	grow(arrayPtr,size);
	
	cout<<"Array: ";
	for(int i = 0; i < size-1; i++){
		cout << arrayPtr [i] << " ";
	}
	cout<<endl;
	
	cout<<endl;
	int insert;
	cout<<"Insert Element: ";
	cin>>insert;
	
	arrayPtr[size - 1] = insert;
	SelectionSort(arrayPtr, size);
	
	cout<<"Array: ";
	for(int i = 0; i < size; i++){
		cout << arrayPtr [i] << " ";
	}
	cout<<endl;
	
	return 0;
}
