#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>

int arraySum(int array[], int size) {
    int sum = 0;
    for (int i = 0; i < size; i++) {
        sum += array[i];
    }
    return sum;
}

int main() {
    const char* pipeName = "/tmp/lab_5.1";
    int buffer[1024];
    int fd = open(pipeName, O_RDONLY);

    int size;
    read(fd, &size, sizeof(int)); // Reading the size of the array
    read(fd, buffer, sizeof(int) * size); // Reading the array
    close(fd);

//    for (int i = 0; i < size; i++) {
//        printf("%d ", buffer[i]); // Print the received array
//    }
//    printf("\n");

    int sum = arraySum(buffer, size);

    fd = open(pipeName, O_WRONLY);
    write(fd, &sum, sizeof(int));
    close(fd);
    return 0;
}
