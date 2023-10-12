#include <stdio.h>
#include <stdbool.h>
#include "header.h"

void swap(int *a, int *b) {
	int temp = *a;
	*a = *b;
	*b = temp;
}

//Ascending is True
//Descending is False
void sort(int array[],bool order){
    int size;
    for(size = 0; array[size] != '\0'; size++);
    if(order){
        for(int i = 0; i < size; i++){
            for(int j = i+1; j < size; j++){
                if(array[i] > array[j])
                    swap(&array[i],&array[j]);
            }
        }
    }
    else{
        for(int i = 0; i < size; i++){
            for(int j = i+1; j < size; j++){
                if(array[i] < array[j])
                    swap(&array[i],&array[j]);
            }
        }
    }
    printf("Sorted Array is: ");
    for(int i = 0; i < size; i++)
        printf("%d ",array[i]);
    printf("\n");
}

void findHighest(int array[], int position) {
	int n;
    for(n = 0; array[n] != '\0'; n++);
	if (position < 1 || position > n) {
		printf("Invalid position. Position should be between 1 and %d.\n", n);
		return;
	}

    bool order;
	if(array[0] > array[1]) {
        order = 0;

    }
    else if(array[1] > array[0]) {
        order = 1;
    }

    bool flag = false;
    for(int i = 0, j = n; flag!=true; i++){
        if(order){
            if(i == position-1) {
                printf("The %d Highest Element is: %d \n", position, array[i]);
                flag = true;
            }
        }
        else{
            if((n-j) == position) {
                printf("The %d Highest Element is: %d \n", position, array[j]);
                flag = true;
            }
            else
                j--;
        }
    }
}

void print(int array[]) {
    int size;
    for(size = 0; array[size] != '\0'; size++);
    for(int i = 0; i < size; i++)
        printf("%d ",array[i]);
    printf("\n");
}
