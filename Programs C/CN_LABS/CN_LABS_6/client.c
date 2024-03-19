#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>

#define MAX_SIZE 2000

int main(void) {
    int socket_desc;
    struct sockaddr_in server_addr;
    char server_message[MAX_SIZE], client_message[MAX_SIZE];

    memset(server_message, '\0', sizeof(server_message));
    memset(client_message, '\0', sizeof(client_message));

    socket_desc = socket(AF_INET, SOCK_STREAM, 0);
    if (socket_desc < 0) {
        printf("Could not create socket. Error!!!!!\n");
        return -1;
    }

    printf("Socket created.\n");

    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(2000);
    server_addr.sin_addr.s_addr = inet_addr("127.0.0.1");

    if (connect(socket_desc, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0) {
        printf("Connection failed. Error!!!!!\n");
        return -1;
    }

    printf("Connected.\n");

    printf("Enter location: ");
    fgets(client_message, sizeof(client_message), stdin);
    // Remove trailing newline character
    client_message[strcspn(client_message, "\n")] = 0;

    if (send(socket_desc, client_message, strlen(client_message), 0) < 0) {
        printf("Send failed. Error!!!!\n");
        return -1;
    }

    if (recv(socket_desc, server_message, sizeof(server_message), 0) < 0) {
        printf("Receive failed. Error!!!!!\n");
        return -1;
    }

    printf("Server Message: %s\n", server_message);

    close(socket_desc);

    return 0;
}
