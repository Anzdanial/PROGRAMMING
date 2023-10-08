#include <stdio.h>
#include <stdbool.h>
#include "header.h"

void swap(int *a, int *b) {
	int temp = *a;
	*a = *b;
	*b = temp;
}

void bubbleSort(int array[], int n, bool order) {
	for (int i = 0; i < n - 1; i++) {
		for (int j = 0; j < n - i - 1; j++) {
			if ((order && array[j] > array[j + 1]) || (!order && array[j] < array[j + 1])) {
				swap(&array[j], &array[j + 1]);
			}
		}
	}
}

void sort(int array[], bool order) {
	int n = sizeof(array) / sizeof(array[0]);
	bubbleSort(array, n, order);
}

void findHighest(int array[], int position) {
	int n = sizeof(array) / sizeof(array[0]);
	if (position < 1 || position > n) {
		printf("Invalid position. Position should be between 1 and %d.\n", n);
		return;
	}

	// Sort the array in descending order
	bubbleSort(array, n, false);

	printf("Sorted Array: ");
	for (int i = 0; i < n; i++) {
		printf("%d ", array[i]);
	}
	printf("\n");

	printf("The %d%s highest number is: %d\n", position, (position == 1) ? "st" : (position == 2) ? "nd" : "th", array[position - 1]);
}

void print(int array[], int n) {
	printf("Array: ");
	for (int i = 0; i < n; i++) {
		printf("%d ", array[i]);
	}
	printf("\n");
}
