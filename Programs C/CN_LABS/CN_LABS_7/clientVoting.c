// Client Side Code

#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <stdlib.h>
#include <unistd.h>

#define BUFFER_SIZE 2000

int main() {
    int socket_desc;
    struct sockaddr_in server;
    char buffer[BUFFER_SIZE];

    // Create socket
    socket_desc = socket(AF_INET, SOCK_STREAM, 0);
    if (socket_desc == -1) {
        printf("Could not create socket\n");
        return 1;
    }

    // Server configuration
    server.sin_addr.s_addr = inet_addr("127.0.0.1");
    server.sin_family = AF_INET;
    server.sin_port = htons(2000);

    // Connect to remote server
    if (connect(socket_desc, (struct sockaddr *)&server, sizeof(server)) < 0) {
        printf("Connection failed\n");
        return 1;
    }

    printf("Connected to server\n");

    // Receive welcome message from server
    if (recv(socket_desc, buffer, BUFFER_SIZE, 0) < 0) {
        printf("Receive failed\n");
        return 1;
    }
    printf("%s", buffer);

    // Enter name
    printf("Enter your name: ");
    fgets(buffer, BUFFER_SIZE, stdin);
    send(socket_desc, buffer, strlen(buffer), 0);

    // Receive prompt for CNIC
    if (recv(socket_desc, buffer, BUFFER_SIZE, 0) < 0) {
        printf("Receive failed\n");
        return 1;
    }
    printf("%s", buffer);

    // Enter CNIC
    printf("Enter your CNIC: ");
    fgets(buffer, BUFFER_SIZE, stdin);
    send(socket_desc, buffer, strlen(buffer), 0);

    // Receive response from server
    if (recv(socket_desc, buffer, BUFFER_SIZE, 0) < 0) {
        printf("Receive failed\n");
        return 1;
    }
    printf("%s", buffer);

    // Close the socket
    close(socket_desc);

    return 0;
}
