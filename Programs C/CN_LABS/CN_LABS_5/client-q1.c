#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>

#define SERVER_IP "127.0.0.1"
#define SERVER_PORT 2000

int main(void)
{
    int socket_desc;
    struct sockaddr_in server_addr;
    char server_message[2000], client_message[2000];
    int server_struct_length = sizeof(server_addr);

    memset(server_message, '\0', sizeof(server_message));
    memset(client_message, '\0', sizeof(client_message));

    // Creating TCP Socket
    socket_desc = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

    if (socket_desc == -1)
    {
        printf("Could Not Create Socket. Error!!!!!\n");
        return -1;
    }

    printf("Socket Created\n");

    // Specifying the IP and Port of the server to connect
    server_addr.sin_family = AF_INET;
    server_addr.sin_port = htons(SERVER_PORT);
    server_addr.sin_addr.s_addr = inet_addr(SERVER_IP);

    // Connect to the server
    if (connect(socket_desc, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0)
    {
        printf("Connection Failed. Error!!!!!");
        return -1;
    }

    printf("Connected to Server\n");

    // Get Input from the User
    printf("Enter Client ID (0-9): ");
    fgets(client_message, sizeof(client_message), stdin);
    client_message[strcspn(client_message, "\n")] = '\0'; // remove newline character

    // Send the message to Server
    if (send(socket_desc, client_message, strlen(client_message), 0) < 0)
    {
        printf("Send Failed. Error!!!!\n");
        return -1;
    }

    // Receive the message back from the server
    if (recv(socket_desc, server_message, sizeof(server_message), 0) < 0)
    {
        printf("Receive Failed. Error!!!!!\n");
        return -1;
    }

    printf("Server Message: %s\n", server_message);

    // Closing the Socket
    close(socket_desc);

    return 0;
}
