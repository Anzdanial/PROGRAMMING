#include <iostream>
using namespace std;

void insertionSort(int *array, int size){
	for(int i=1; i<size; i++){
		int sorted = array[i];
		int j = i - 1;
		while(sorted < array[j] && j>=0){
			array[j+1] = array[j];
			--j;
		}
		array[j+1] = sorted;
	}
}

int main(){
	int size, *array;
	cout<<"Enter the size of the array: ";
	cin>>size;
	array = new int [size];
	cout<<"Enter the elements of the array: ";
	for(int i=0; i<size; i++){
		cin>>array[i];
	}
	cout<<endl;
	
	cout<<"Unsorted Array: ";
	for(int i=0; i<size; i++){
		cout<<array[i]<<" ";
	}
	cout<<endl;
	
	insertionSort(array,size);
	
	cout<<"Sorted Array: ";
	for(int i=0; i<size; i++){
		cout<<array[i]<<" ";
	}
	cout<<endl;
	
	return 0;
}
