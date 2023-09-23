#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <wait.h>
#include <stdlib.h>


int main(int argc, char **argv) {
    int fd[2];
    pipe(fd);
    char inputFile[9];
    char outputFile[10];
    for(int i = 0; i < argc - 1; i++){
        for(int j = 0; argv[i+1][j] != '\0'; j++){
            if(i+1 == 1)
                inputFile[j] = argv[1][j];
            else if(i+1 == 2)
                outputFile[j] = argv[2][j];
        }
    }
//    printf("The name of input file is: ");
//    for(int i = 0; i < 9; i++)
//        printf("%c", inputFile[i]);
//    printf(" \n");
//    printf("The name of output file is: ");
//    for(int i = 0; i < 10; i++)
//        printf("%c", outputFile[i]);
//    printf(" \n");

    int val = fork();
    if(val == 0){
        
    }
    else if(val > 0){

    }
    return 0;
}
