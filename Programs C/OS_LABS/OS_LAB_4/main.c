#include <stdio.h>
#include <unistd.h>
#include <wait.h>
#include <stdlib.h>
#include <string.h>


int main(int argc, char **argv) {
    int fd[2];
    pipe(fd);
    char inputFile[10];
    char outputFile[10];
    for(int j = 0; argv[1][j] != '\0'; j++){
            inputFile[j] = argv[1][j];
    }
    for(int j = 0; argv[2][j] != '\0'; j++){
            outputFile[j] = argv[2][j];
    }
    int val = fork();
    if(val == 0){
        close(fd[1]);
        char data[100];
        read(fd[0], data, sizeof(data));
        int size = strlen(data);
        printf("[Child: %d] - The size of content received is: %d\n",getpid(), size);
        printf("[Child: %d] - The content of File received is: ",getpid());
        for(int i = 0; i < size; i++)
            printf("%c",data[i]);
        printf("\n");
        FILE *fptr;
        if ((fptr = fopen("output.txt","w")) == NULL){
            perror("Error! opening file");
            exit(1);
        }
        for(int i = 0; i < size-2; i++){
            fputc(data[i], fptr);
        }
        fclose(fptr);
    }
    else if(val > 0){
        close(fd[0]);
        char data[100];
        FILE *fptr;
        if ((fptr = fopen(inputFile,"r")) == NULL){
                perror("Error! opening file");
                exit(1);
            }
        fgets(data, sizeof(data), fptr);
        int size;
        printf("[Parent: %d] - The content of File is: ", getpid());
        for(size = 0; data[size] != '\0'; size++)
            printf("%c", data[size]);
        printf("\n[Parent: %d] - The size of the content is: %d \n", getpid(), size);
        fclose(fptr);
        write(fd[1], data, size);
        wait(NULL);
    }
    return 0;
}
