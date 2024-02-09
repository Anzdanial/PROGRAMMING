#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>

int is_non_alphabet_word(const char *word) {
	while (*word) {
		if (isalpha(*word)) {
			return 0;
		}
		word++;
	}
	return 1;
}

int main() {
	FILE *inputFile = fopen("input.txt", "r");

	if (inputFile == NULL) {
		printf("Error opening the input file.\n");
		return 1;
	}

	FILE *outputFile = fopen("NonAlphabetWords.txt", "w");

	if (outputFile == NULL) {
		printf("Error creating/opening the output file.\n");
		fclose(inputFile);
		return 1;
	}

	char word[1024]; 

	while (fscanf(inputFile, "%s", word) == 1) {
		if (is_non_alphabet_word(word)) {
			fprintf(outputFile, "%s\n", word);
		}
	}

	fclose(inputFile);
	fclose(outputFile);
	return 0;
}
