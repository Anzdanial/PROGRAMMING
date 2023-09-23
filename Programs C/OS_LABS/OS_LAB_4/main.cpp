#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <wait.h>
#include <stdlib.h>

int main() {
    int fd[2];
    pipe(fd);
    if(fork() == 0){
        close(fd[1]);
        read(fd[0]);
    }
    else{
        close(fd[0]);
        write(fd[1]);
    }
    return 0;
}

