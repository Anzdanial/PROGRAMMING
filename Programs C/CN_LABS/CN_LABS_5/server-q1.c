#include <stdio.h>
#include <string.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>

#define SERVER_PORT 2000
#define MAX_CLIENTS 10

void handle_client(int client_socket);

int main(void)
{
    int server_socket, client_socket;
    struct sockaddr_in server_addr, client_addr;
    socklen_t client_struct_length = sizeof(client_addr);

    // Create TCP socket
    server_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (server_socket == -1)
    {
        printf("Could not create socket. Error!!!!!\n");
        return -1;
    }

    printf("Socket created\n");

    // Prepare the sockaddr_in structure
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = INADDR_ANY;
    server_addr.sin_port = htons(SERVER_PORT);

    // Bind
    if (bind(server_socket, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0)
    {
        perror("Bind failed. Error!!!!!");
        return -1;
    }

    printf("Bind done\n");

    // Listen
    listen(server_socket, MAX_CLIENTS);

    // Accept and incoming connection
    puts("Waiting for incoming connections...");

    while (1)
    {
        client_socket = accept(server_socket, (struct sockaddr *)&client_addr, &client_struct_length);
        if (client_socket < 0)
        {
            perror("Accept failed. Error!!!!!\n");
            return -1;
        }

        printf("Connection accepted\n");

        // Handle the connection in a separate function
        handle_client(client_socket);
    }

    return 0;
}

void handle_client(int client_socket)
{
    char client_message[2000], server_message[2048]; // Increased size for server_message
    int read_size;

    memset(client_message, '\0', sizeof(client_message));
    memset(server_message, '\0', sizeof(server_message));

    // Receive a message from client
    if ((read_size = recv(client_socket, client_message, sizeof(client_message), 0)) > 0)
    {
        // Respond to the client
        sprintf(server_message, "Hello I am server. Your received id is %s", client_message);
        write(client_socket, server_message, strlen(server_message));
    }

    if (read_size == 0)
    {
        puts("Client disconnected");
        fflush(stdout);
    }
    else if (read_size == -1)
    {
        perror("Receive failed. Error!!!!!");
    }

    // Close client socket
    close(client_socket);
}

