#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>

int main() {
    const char* pipeName = "/tmp/lab_5.1";
    char buffer[1024];
    mkfifo(pipeName, 0666);
    int fd = open(pipeName, O_WRONLY);

    printf("Enter the Integer Array: ");
    fgets(buffer, sizeof(buffer), stdin);

    int size = 0;
    for (int i = 0; buffer[i] != '\0'; i++) {
        if (buffer[i] >= '0' && buffer[i] <= '9') {
            size++;
        }
    }

    int array[size];
    int index = 0;
    for (int i = 0; buffer[i] != '\0'; i++) {
        if (buffer[i] >= '0' && buffer[i] <= '9') {
            array[index] = buffer[i] - '0';
            printf("%d \n", array[index]);
            index++;
        }
    }

    write(fd, &size, sizeof(int)); // Sending the size of the array
    write(fd, array, sizeof(int) * size); // Sending the array
    close(fd);


    fd = open(pipeName, O_RDONLY);
    int sum;
    read(fd, &sum, sizeof(sum));
    close(fd);

    printf("Sum received from calculator: %d\n", sum);

    return 0;
}
