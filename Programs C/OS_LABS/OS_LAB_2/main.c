
//Question #1
/*#include <stdio.h>
#include <sys/types.h>
#include <unistd.h>
#include <stdlib.h>

int main(int argc, char ** argv){
    pid_t id = fork();
    if(id == 0) {
        int *array = (int *) malloc(argc * sizeof(int));
        for (int i = 1; i < argc; i++) {
            array[i-1]= atoi(argv[i]);
        }
        for(int i = 0; i < argc-1; i++){
            printf("%d \n", array[i]);
        }
        printf("\n\n");
        for(int i = 0; i < argc-1; i++){
            for(int j = i + 1; j < argc - 1; j++){
                if(array[j] < array[i])
                {
                    int tmp = array[i];
                    array[i] = array[j];
                    array[j] = tmp;
                }
            }
        }
        for(int i = 0; i < argc-1; i++){
            printf("%d \n", array[i]);
        }

        printf("\n\n");
        printf("%d \n", id);
    }
    return 0;
}
*/




//Question #2
/*
#include <stdio.h>
#include <sys/types.h>
#include <unistd.h>
#include <stdlib.h>

void wait(void *pVoid);

int main(int argc, char ** argv){
    int size = argc;
    for (int i = 1; i < argc; i++) {
        printf("%s ", argv[i]);
    }
    printf("\n");

    pid_t id = fork();
    if(id > 0) {
        pid_t id2 = fork();
        if (id2 == 0) {
            printf("I am Child 2 (with ID = %d and Parent ID = %d): \n", getpid(), getppid());
            int *array1 = (int *) malloc(argc * sizeof(int));
            for (int i = 1; i < argc; i++) {
                array1[i - 1] = atoi(argv[i]);
            }
            for (int i = 0; i < argc - 1; i++) {
                for (int j = i + 1; j < argc - 1; j++) {
                    if (array1[j] > array1[i]) {
                        int tmp = array1[i];
                        array1[i] = array1[j];
                        array1[j] = tmp;
                    }
                }
            }
            for (int i = 0; i < argc - 1; i++) {
                printf("%d ", array1[i]);
            }
            printf("\n");
            printf("\n\n");

        }

        if(id2 > 0){
                printf("Process terminating with ID %d with Parent ID: %d", getpid(), getppid());
        }
    }
    else {
        if (id == 0) {
            printf("\n \n");
            printf("I am Child 1 (with ID = %d and Parent ID = %d): \n", getpid(), getppid());
            int *array = (int *) malloc(argc * sizeof(int));
            for (int i = 1; i < argc; i++) {
                array[i - 1] = atoi(argv[i]);
            }
            for (int i = 0; i < argc - 1; i++) {
                for (int j = i + 1; j < argc - 1; j++) {
                    if (array[j] < array[i]) {
                        int tmp = array[i];
                        array[i] = array[j];
                        array[j] = tmp;
                    }
                }
            }
            for (int i = 0; i < size - 1; i++) {
                printf("%d ", array[i]);
            }
            printf("\n");
            printf("\n\n");
        }
    }
    return 0;
}

void wait(void *pVoid) {

}
*/

//Question #3
/*#include <stdio.h>
#include <sys/types.h>
#include <unistd.h>
#include <stdlib.h>

int main(int argc, char ** argv){
    pid_t pid = 0;
    int y = atoi(argv[1]);
    for(int i = 0; i < y; i++){
        if(pid == 0){
            printf("I am a child with ID: %d", getpid());
            printf("with Parent ID: %d \n", getppid());
            pid = fork();
            wait(NULL);
        }
    }
    return 0;
}
*/
