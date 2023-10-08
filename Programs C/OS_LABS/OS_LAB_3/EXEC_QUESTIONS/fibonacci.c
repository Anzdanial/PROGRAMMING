#include <stdio.h>
#include <stdlib.h>

int main(int argc, char **argv) {
	int n, first = 0, second = 1, next, i;
	n = atoi(argv[1]);
	printf("Fibonacci Series:\n");
	for (i = 0; i < n; i++) {
		if (i <= 1) {
			next = i;
		} else {
			next = first + second;
			first = second;
			second = next;
		}
		printf("%d ", next);
	}
	printf("\n");

	return 0;
}
