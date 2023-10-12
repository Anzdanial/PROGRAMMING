#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>
#include <stdbool.h>

//main1 file hai ye
//Write File
//This will run First
int main() {
    const char* pipeName = "/tmp/ass_2.1";
    char buffer[1024];
    mkfifo(pipeName, 0666);

    int fd = open(pipeName, O_WRONLY);
    if (fd == -1) {
        perror("open");
        exit(EXIT_FAILURE);
    }

    fgets(buffer,sizeof(buffer), stdin);
    write(fd, buffer, sizeof(buffer));
    close(fd);

    const char* pipeName1 = "/tmp/ass_2.2";
    double result = 0;
    int fd1 = open(pipeName1, O_RDONLY);

    if (fd1 == -1) {
        perror("open");
        exit(EXIT_FAILURE);
    }

    read(fd1, &result, sizeof(result));
    close(fd1);

    printf("The result is %f \n", result);

    return 0;
}
