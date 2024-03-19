#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <arpa/inet.h>

#define PORT 5000
#define BUFFER_SIZE 1024

char* invert_words_with_vowels(char* text) {
    char* inverted_text = (char*)malloc(strlen(text) + 1);
    char* token = strtok(text, " ");
    int index = 0;
    while (token != NULL) {
        int contains_vowel = 0;
        char* temp = token;
        while (*temp) {
            if (strchr("aeiouAEIOU", *temp)) {
                contains_vowel = 1;
                break;
            }
            temp++;
        }
        if (contains_vowel) {
            int len = strlen(token);
            for (int i = len - 1; i >= 0; i--) {
                inverted_text[index++] = token[i];
            }
            inverted_text[index++] = ' ';
        } else {
            strcpy(&inverted_text[index], token);
            index += strlen(token);
            inverted_text[index++] = ' ';
        }
        token = strtok(NULL, " ");
    }
    inverted_text[index - 1] = '\0'; // Ensure proper null termination
    return inverted_text;
}

int main() {
    int server_socket, client_socket;
    struct sockaddr_in server_addr, client_addr;
    char buffer[BUFFER_SIZE];

    if ((server_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) == -1) {
        perror("Socket creation failed");
        exit(EXIT_FAILURE);
    }

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = INADDR_ANY;
    server_addr.sin_port = htons(PORT);

    if (bind(server_socket, (struct sockaddr *)&server_addr, sizeof(server_addr)) == -1) {
        perror("Socket bind failed");
        exit(EXIT_FAILURE);
    }

    if (listen(server_socket, 5) == -1) {
        perror("Socket listen failed");
        exit(EXIT_FAILURE);
    }

    printf("Server listening...\n");

    while (1) {
        socklen_t client_addr_len = sizeof(client_addr);
        if ((client_socket = accept(server_socket, (struct sockaddr *)&client_addr, &client_addr_len)) == -1) {
            perror("Socket accept failed");
            exit(EXIT_FAILURE);
        }

        printf("Connection established with client\n");

        if (recv(client_socket, buffer, BUFFER_SIZE, 0) == -1) {
            perror("Receive failed");
            exit(EXIT_FAILURE);
        }

        printf("Client sent: %s\n", buffer);

        // Display the server's inversion
        char* inverted_text_server = invert_words_with_vowels(buffer);
        printf("Server inversion: %s\n", inverted_text_server);
        free(inverted_text_server);

        // Perform inversion on the received string and send it back to the client
        char* inverted_text = invert_words_with_vowels(buffer);
        if (send(client_socket, inverted_text, strlen(inverted_text), 0) == -1) {
            perror("Send failed");
            exit(EXIT_FAILURE);
        }

        free(inverted_text);
        close(client_socket);
    }

    close(server_socket);
    return 0;
}
