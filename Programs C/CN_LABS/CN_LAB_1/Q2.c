#include <stdio.h>
#include <stdlib.h>

int is_digit(char c) {
	return c >= '0' && c <= '9';
}

int main() {
	FILE *inputFile = fopen("input.txt", "r");

	if (inputFile == NULL) {
		printf("Error opening the input file.\n");
		return 1;
	}

	FILE *outputFile = fopen("output.txt", "w");

	if (outputFile == NULL) {
		printf("Error creating/opening the output file.\n");
		fclose(inputFile);
		return 1;
	}

	int number;
	char character;

	while ((character = fgetc(inputFile)) != EOF) {
		if (is_digit(character)) {
			ungetc(character, inputFile);  // put back the digit
			fscanf(inputFile, "%d", &number);
			fprintf(outputFile, "%d\n", number);
		}
	}

	fclose(inputFile);
	fclose(outputFile);

	return 0;
}
