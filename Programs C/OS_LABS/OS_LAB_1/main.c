//Question #4

#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>

int main(int argc, char **argv) {
    printf("The Name of Input file is: \n");
    printf("%s\n", argv[1]);
    printf("and the Name of Output file is: \n");
    printf("%s\n", argv[2]);

    FILE *inputFile = fopen(argv[1], "r");
    if (inputFile == NULL) {
        perror("Error opening input file");
        return 1;
    }

    FILE *outputFile = fopen(argv[2], "w");
    if (outputFile == NULL) {
        perror("Error opening output file");
        fclose(inputFile);
        return 1;
    }

    int ch;
    while ((ch = fgetc(inputFile)) != EOF) {
        if (isdigit(ch)) {
            printf("%d\n",ch-48);
            fputc(ch, outputFile);
        }
    }

    fclose(inputFile);
    fclose(outputFile);

    printf("Numbers Done Copying from %s to %s\n", argv[1], argv[2]);

    return 0;
}


//Question #3
/*
#include <stdio.h>
#include <malloc.h>
#include <stdlib.h>

int main(int argc, char **argv) {
    int sum = 0, average, counter = 0;
    int length = argc-1;
    int *array;
    array = (int*) malloc(argc-1 * sizeof(int));
    for(int i = 1; i < argc; i++){
        array[i-1] = atoi(argv[i]);
    }
    for(int i = 0; i < length; i++){
        sum += array[i];
        counter++;
    }
    average = sum / counter;
    printf("Sum is %d AND Average is %d \n", sum, average);
    return 0;
}
*/



//Question #2
//
//#include <stdio.h>
//int main(int argc, char **argv) {
//    printf("Welcome to BS-SE Lab \n");
//    printf("The name of the Course is ");
//    for(int i = 1; i < argc; i++) {
//        printf("%s ", argv[i]);
//    }
//    printf("\n");
//    return 0;
//}

