#include <stdio.h>

int main() {
	FILE *file = fopen("input.txt", "r");
	if (file == NULL) {
		printf("Error opening the file.\n");
		return 1;
	}

	char character;
	while ((character = fgetc(file)) != EOF) {
		putchar(character);
	}
	printf("\n");
	fclose(file);
	return 0;
}
