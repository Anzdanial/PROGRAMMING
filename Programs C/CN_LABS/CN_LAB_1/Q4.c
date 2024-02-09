#include <stdio.h>
#include <stdlib.h>
#include <string.h>

int contains_vowel(const char *word) {
	while (*word) {
		if (*word == 'a' || *word == 'e' || *word == 'i' || *word == 'o' || *word == 'u' ||
			*word == 'A' || *word == 'E' || *word == 'I' || *word == 'O' || *word == 'U') {
			return 1;
		}
		word++;
	}
	return 0;
}

void invert_word(char *word) {
	int length = strlen(word);
	for (int i = 0; i < length / 2; i++) {
		char temp = word[i];
		word[i] = word[length - i - 1];
		word[length - i - 1] = temp;
	}
}

int main() {
	FILE *inputFile = fopen("input.txt", "r");

	if (inputFile == NULL) {
		printf("Error opening the input file.\n");
		return 1;
	}

	FILE *outputFile = fopen("InvertedWords.txt", "w");

	if (outputFile == NULL) {
		printf("Error creating/opening the output file.\n");
		fclose(inputFile);
		return 1;
	}

	char word[100];

	while (fscanf(inputFile, "%s", word) == 1) {
		if (contains_vowel(word)) {
			invert_word(word);
		}
		fprintf(outputFile, "%s ", word);
	}

	fclose(inputFile);
	fclose(outputFile);

	return 0;
}
