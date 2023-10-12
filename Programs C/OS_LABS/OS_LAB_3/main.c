#include <stdio.h>
#include <stdlib.h>
#include "header.h"

int main(int argc, char *argv[]) {
	if (argc != 4) {
		fprintf(stderr, "Usage: %s <array> <order (1/0)> <position>\n", argv[0]);
		return 1;
	}

	char *inputArrayStr = argv[1];
	char *ptr;
	int array[100];
	int n = 0;

	while (*inputArrayStr != '\0') {
		array[n] = strtol(inputArrayStr, &ptr, 10);
		if (ptr == inputArrayStr) {
			fprintf(stderr, "Error: Invalid input array\n");
			return 1;
		}
		inputArrayStr = ptr;
		n++;
	}

	int order = atoi(argv[2]);
	int position = atoi(argv[3]);

    printf("Array Elements are: ");
    print(array);
    printf("Sorted Elements are: ");
	sort(array, order);
    findHighest(array, position);
	return 0;
}
