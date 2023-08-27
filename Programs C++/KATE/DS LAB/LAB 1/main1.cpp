#include <iostream>
using namespace std;

void SelectionSort(int *array, int size){
	int temp;
	for(int i = 0; i < size - 1; i++){
		int least = i;  //Storing the index of least val
		for(int j = i+1; j < size; j++){
			if(array[j] < array[least])
				least = j; //Finding the least value index;
		}
		temp = array[i];
		array[i] = array[least];
		array[least] = temp;
	}							//Swapping the Values;
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
	
	SelectionSort(arrayPtr,size);
	
	cout<<"The Sorted array: ";
	for(int i = 0; i < size; i++){
		cout << arrayPtr[i] << " ";
	}
	cout<<endl<<endl;
	
	int search;
	bool flag;
	cout<<"Enter the Element to Search: ";
	cin>>search;
	for(int i = 0; i < size; i++){
		if(arrayPtr[i] == search){
			flag = true;
			cout << "The Searched Element is Present on index: "<<i<<endl;
		}
	}
	if(!flag)
		cout << "The Searched Element is not Present"<<endl;
	
	return 0;
}
