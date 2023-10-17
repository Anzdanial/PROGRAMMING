#include <stdio.h>
#include <pthread.h>
#include <stdlib.h>
#include <unistd.h>
#include <mqueue.h>
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

int getSize(char *array){
    int size;
    for(size = 0; array[size]!='\0'; size++);
    return size;
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


char ** uniqueWordCounter(char **words, int totalWords, int *duplicates, int *uniqueSize){
    char **uniqueWords;
    int counter = 0;
    int *arrayFlag = (int *) malloc((totalWords) * sizeof(int));
    for(int i = 0; i < totalWords; i++){ arrayFlag[i] = 0;}
    for(int i = 0; i < totalWords; i++){
        for(int j = i+1; j < totalWords; j++){;
            if(strcmp(lowerCase(words[i]),lowerCase(words[j])) == 0) {
                if(arrayFlag[j] == 0) {
                    arrayFlag[j] = j;
                }
                if(arrayFlag[i] > 0){
                    continue;
                }
                else {
                    counter++;
                }
            }
        }
    }
    uniqueWords = (char **) malloc((totalWords - counter) * sizeof(char *));
    for(int i = 0, k = 0; i < totalWords; i++){
        if(arrayFlag[i] > 0) {
            continue;
        }
        else{
            uniqueWords[k] = (char *) malloc((getSize(words[i])) * sizeof(char));
            uniqueWords[k] = words[i];
            k++;
        }
    }
    for(int i = 0; i < (totalWords - counter); i++){
        printf("%s \n",uniqueWords[i]);
    }
    *duplicates = counter;
    *uniqueSize = totalWords - counter;
    return uniqueWords;
}

int main(int argc, char **argv) {
    if(argc < 2){
        fprintf(stderr,"No Input Given \n");
        return 1;
    }
    int N = atoi(argv[1]);

    int fd, size;
    char data[1024];
    fd = open("data.txt", O_RDONLY);
    if(fd < 0){
        perror("Couldn't Read File \n");
        exit(1);
    }
    size = read(fd, data, sizeof(data));
    data[size] = '\0';

    int wordsSize = wordCounter(data, size);
    char **words = storeWords(data, size);
    int duplicates = 0, uniqueSize = 0;
    char **uniqueWords = uniqueWordCounter(words,wordsSize, &duplicates,&uniqueSize);
    for(int i = 0; i < uniqueSize; i++){
        printf("%s \n",uniqueWords[i]);
    }
    return 0;
}