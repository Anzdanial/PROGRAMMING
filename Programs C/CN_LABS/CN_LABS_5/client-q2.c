#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <arpa/inet.h>

#define PORT 5000
#define BUFFER_SIZE 1024

char* invert_words_without_vowels(char* text) {
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
        if (!contains_vowel) {
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
    inverted_text[index] = '\0';
    return inverted_text;
}

int main() {
    int client_socket;
    struct sockaddr_in server_addr;
    char buffer[BUFFER_SIZE];

    if ((client_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP)) == -1) {
        perror("Socket creation failed");
        exit(EXIT_FAILURE);
    }

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = inet_addr("127.0.0.1");
    server_addr.sin_port = htons(PORT);

    if (connect(client_socket, (struct sockaddr *)&server_addr, sizeof(server_addr)) == -1) {
        perror("Connection failed");
        exit(EXIT_FAILURE);
    }

    printf("Connected to server\n");

    printf("Enter message: ");
    fgets(buffer, BUFFER_SIZE, stdin);

    if (send(client_socket, buffer, strlen(buffer), 0) == -1) {
        perror("Send failed");
        exit(EXIT_FAILURE);
    }

    if (recv(client_socket, buffer, BUFFER_SIZE, 0) == -1) {
        perror("Receive failed");
        exit(EXIT_FAILURE);
    }

    printf("From server: %s\n", buffer);

    char* inverted_text = invert_words_without_vowels(buffer);
    printf("Client modified: %s\n", inverted_text);
    free(inverted_text);

    close(client_socket);
    return 0;
}
