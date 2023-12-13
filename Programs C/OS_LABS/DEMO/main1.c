#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <stdbool.h>
#include <string.h>

const char * lowerCase(char* array){
	int size;
	for(size = 0; array[size]!='\0'; size++);
	for(int i = 0; i < size; i++){
		if(array[i] > 64 && array[i] < 91) {
			array[i] = array[i] + 32;
		}
	}
	return array;
}

int wordCounter(char array[], int size){
	int wordCounter = 0;
	for(int i = 0; i < size; i++){
		if(array[i+1] == ' ' || i+1 >= size)
			wordCounter++;
	}
	return wordCounter;
}

char ** storeWords(char array[], int size) {
	int wordCount = wordCounter(array, size);
	char **words = (char **) malloc(wordCount * sizeof(char *));
	int startIndex, endIndex = 0, space = 0, counter = 0;
	bool flag;
	for (int i = 0; i < size; i++) {
		flag = false;
		if (space > 0)
			startIndex = endIndex + 1;
		else
			startIndex = 0;
		if (array[i + 1] == ' ' || i+1 == size) {
			endIndex = i + 1;
			words[counter] = (char *) malloc((endIndex - startIndex) * sizeof(char));
			for (int j = 0, k = startIndex; k < endIndex; k++, j++) {
				words[counter][j] = array[k];
			}
			space++;
			flag = true;
		}
		if (flag)
			counter++;
	}
	return words;
}


int main() {
	const char* pipeName = "/tmp/story_pipe";
	char buffer[1024];
	char buffer2[1024] = "a to be it for of the";

	int fd = open(pipeName, O_RDONLY);
	if (fd == -1) {
		perror("open");
		exit(EXIT_FAILURE);
	}

	read(fd, buffer, sizeof(buffer));
	close(fd);

	int size;
	for(size = 0; buffer[size]!='\0'; size++);
	int size2;
	for(size2 = 0; buffer[size2]!='\0'; size2++);

	int wordsSize = wordCounter(buffer,size);
	char **words = storeWords(buffer,size);

	int stopWordsSize = wordCounter(buffer2,size2);
	char **stopWords = storeWords(buffer2,size2);

	int frequency[stopWordsSize];
	for(int i = 0; i < stopWordsSize; i++){
		frequency[i] = 0;
	}


	for(int i = 0; i < wordsSize; i++){
		for(int j = 0; j < stopWordsSize; j++) {
			if(strcmp(lowerCase(words[i]), lowerCase(stopWords[j])) == 0){
				frequency[j]++;
			}
		}
	}

	for(int i = 0; i < stopWordsSize; i++){
		printf("The Frequency for %s is: ",stopWords[i]);
		printf("%d\n",frequency[i]);

	}


	return 0;
}