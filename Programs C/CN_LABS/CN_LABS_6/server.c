#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>

#define MAX_SIZE 2000

// Function to search for location data in the database
int searchLocation(char *location, char *data) {
    FILE *file = fopen("database.txt", "r");
    if (file == NULL) {
        printf("Error opening file.\n");
        return -1;
    }

    char line[MAX_SIZE];
    while (fgets(line, sizeof(line), file)) {
        char tempLocation[MAX_SIZE];
        strcpy(tempLocation, line);
        strtok(tempLocation, " "); // Extract the location

        if (strcmp(tempLocation, location) == 0) {
            strcpy(data, line);
            fclose(file);
            return 0;
        }
    }

    fclose(file);
    return -1; // Location not found
}

int main(void) {
    int socket_desc, client_sock, client_size;
    struct sockaddr_in server_addr, client_addr;
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
    server_addr.sin_addr.s_addr = INADDR_ANY;

    if (bind(socket_desc, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0) {
        printf("Bind failed. Error!!!!!\n");
        return -1;
    }

    printf("Bind done.\n");

    if (listen(socket_desc, 1) < 0) {
        printf("Listening failed. Error!!!!!\n");
        return -1;
    }

    printf("Listening for incoming connections...\n");

    while (1) { // Infinite loop to continuously accept connections
        client_size = sizeof(client_addr);
        client_sock = accept(socket_desc, (struct sockaddr *)&client_addr, (socklen_t *)&client_size);

        if (client_sock < 0) {
            printf("Accept failed. Error!!!!!\n");
            continue; // Move to the next iteration of the loop
        }

        printf("Client connected with IP: %s and Port No: %d\n", inet_ntoa(client_addr.sin_addr), ntohs(client_addr.sin_port));

        if (recv(client_sock, client_message, sizeof(client_message), 0) < 0) {
            printf("Receive failed. Error!!!!!\n");
            close(client_sock);
            continue; // Move to the next iteration of the loop
        }

        printf("Client Message: %s\n", client_message);

        char locationData[MAX_SIZE];
        if (searchLocation(client_message, locationData) == 0) {
            if (send(client_sock, locationData, strlen(locationData), 0) < 0) {
                printf("Send failed. Error!!!!!\n");
            }
        } else {
            if (send(client_sock, "Location not found.", strlen("Location not found."), 0) < 0) {
                printf("Send failed. Error!!!!!\n");
            }
        }

        memset(server_message, '\0', sizeof(server_message));
        memset(client_message, '\0', sizeof(client_message));

        close(client_sock);
    }

    close(socket_desc);

    return 0;
}
