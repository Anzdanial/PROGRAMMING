#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/wait.h>
#include <sys/types.h>
#include <string.h>
#include <stdbool.h>

int main(int argc, char *argv[]) {
    char buffer1[100];
    bool flag = true;
    int status;
    while (flag) {
        printf("Please enter the complete command\n");
        fgets(buffer1, sizeof(buffer1), stdin);

        int len = strlen(buffer1);
        if (len > 0 && buffer1[len - 1] == '\n') {
            buffer1[len - 1] = '\0';
        }

        int i = 0, j = 0, count = 0;
        char *arg[50];  //up to 50 arguments including command
        char *token = strtok(buffer1, " "); //tokenizing the input

        while (token != NULL) {
            arg[count] = token; //storing each token as an argument
            count++;
            token = strtok(NULL, " ");
        }
        arg[count] = NULL;

        if (strcmp(arg[0], "exit") == 0) {
            flag = false; //will exit from loop
        }
        else {
            int id = fork();
            if(id ==-1){
                printf("Error in forking process");
            }
            else if(id == 0) {
                execvp(arg[0], arg);
                printf("Sorry error in executing command\n");
                return 1;
            }
            else if(id > 0) {
                wait(NULL);
            }
        }
    }
    return 0;
}
