#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <pthread.h>
#include <unistd.h>
#include <ctype.h>

#define BUFFER_SIZE 2000
#define MAX_VOTERS 100
#define MAX_CANDIDATES 100

struct Voter {
    char name[100];
    char cnic[20];
};

struct Candidate {
    char name[100];
    char symbol[20];
};

void *handle_client(void *arg) {
    int client_sock = *((int *)arg);
    char buffer[BUFFER_SIZE];
    struct Voter voters[MAX_VOTERS];
    struct Candidate candidates[MAX_CANDIDATES];

    FILE *voters_file = fopen("Voters.txt", "r");
    if (!voters_file) {
        perror("Error opening Voters.txt");
        close(client_sock);
        free(arg);
        pthread_exit(NULL);
    }

    int num_voters = 0;
    while (fscanf(voters_file, "%[^/]/%s\n", voters[num_voters].name, voters[num_voters].cnic) != EOF && num_voters < MAX_VOTERS) {
        num_voters++;
    }
    fclose(voters_file);

    FILE *candidates_file = fopen("Candidates.txt", "r");
    if (!candidates_file) {
        perror("Error opening Candidates.txt");
        close(client_sock);
        free(arg);
        pthread_exit(NULL);
    }

    int num_candidates = 0;
    while (fscanf(candidates_file, "%s %[^\n]\n", candidates[num_candidates].name, candidates[num_candidates].symbol) != EOF && num_candidates < MAX_CANDIDATES) {
        num_candidates++;
    }
    fclose(candidates_file);

    send(client_sock, "Enter your name: ", strlen("Enter your name: "), 0);
    recv(client_sock, buffer, BUFFER_SIZE, 0);
    char name[100], cnic[20];
    sscanf(buffer, "%s", name);

    send(client_sock, "Enter your CNIC: ", strlen("Enter your CNIC: "), 0);
    recv(client_sock, buffer, BUFFER_SIZE, 0);
    sscanf(buffer, "%s", cnic);

    // Validate name format
    if (strlen(name) < 3 || strlen(name) > 50) {
        send(client_sock, "Invalid name format. Name length should be between 3 and 50 characters.\n", strlen("Invalid name format. Name length should be between 3 and 50 characters.\n"), 0);
        close(client_sock);
        free(arg);
        pthread_exit(NULL);
    }

    // Validate CNIC format
    if (strlen(cnic) != 15 || cnic[5] != '-' || cnic[13] != '-') {
        send(client_sock, "Invalid CNIC format. Correct format is XXXXX-XXXXXXX-X.\n", strlen("Invalid CNIC format. Correct format is XXXXX-XXXXXXX-X.\n"), 0);
        close(client_sock);
        free(arg);
        pthread_exit(NULL);
    }
    for (int i = 0; i < 5; i++) {
        if (!isdigit(cnic[i])) {
            send(client_sock, "Invalid CNIC format. CNIC prefix should be numeric.\n", strlen("Invalid CNIC format. CNIC prefix should be numeric.\n"), 0);
            close(client_sock);
            free(arg);
            pthread_exit(NULL);
        }
    }
    for (int i = 6; i < 13; i++) {
        if (!isdigit(cnic[i])) {
            send(client_sock, "Invalid CNIC format. CNIC middle portion should be numeric.\n", strlen("Invalid CNIC format. CNIC middle portion should be numeric.\n"), 0);
            close(client_sock);
            free(arg);
            pthread_exit(NULL);
        }
    }
    if (!isdigit(cnic[14])) {
        send(client_sock, "Invalid CNIC format. CNIC suffix should be numeric.\n", strlen("Invalid CNIC format. CNIC suffix should be numeric.\n"), 0);
        close(client_sock);
        free(arg);
        pthread_exit(NULL);
    }

    int authenticated = 0;
    for (int i = 0; i < num_voters; i++) {
        if (strcmp(voters[i].name, name) == 0 && strcmp(voters[i].cnic, cnic) == 0) {
            authenticated = 1;
            break;
        }
    }

    if (authenticated) {
        send(client_sock, "Welcome! Here are the candidates:\n", strlen("Welcome! Here are the candidates:\n"), 0);
        for (int i = 0; i < num_candidates; i++) {
            send(client_sock, candidates[i].name, strlen(candidates[i].name), 0);
            send(client_sock, " - ", strlen(" - "), 0);
            send(client_sock, candidates[i].symbol, strlen(candidates[i].symbol), 0);
            send(client_sock, "\n", strlen("\n"), 0);
        }

        recv(client_sock, buffer, BUFFER_SIZE, 0);
        FILE *output_file = fopen("Votes.txt", "a");
        fprintf(output_file, "%s %s\n", name, buffer);
        fclose(output_file);
    } else {
        send(client_sock, "Authentication failed. You are not eligible to vote.\n", strlen("Authentication failed. You are not eligible to vote.\n"), 0);
    }

    close(client_sock);
    free(arg);
    pthread_exit(NULL);
}

int main() {
    int socket_desc, client_sock, c;
    struct sockaddr_in server, client;

    socket_desc = socket(AF_INET, SOCK_STREAM, 0);
    if (socket_desc == -1) {
        printf("Could not create socket");
        return 1;
    }
    printf("Socket created\n");

    server.sin_family = AF_INET;
    server.sin_addr.s_addr = INADDR_ANY;
    server.sin_port = htons(2000);

    if (bind(socket_desc, (struct sockaddr *)&server, sizeof(server)) < 0) {
        perror("Bind failed. Error");
        return 1;
    }
    printf("Bind done\n");

    listen(socket_desc, 3);

    printf("Waiting for incoming connections...\n");
    c = sizeof(struct sockaddr_in);

    while ((client_sock = accept(socket_desc, (struct sockaddr *)&client, (socklen_t *)&c))) {
        printf("Connection accepted\n");

        pthread_t sniffer_thread;
        int *new_sock = malloc(1);
        *new_sock = client_sock;

        if (pthread_create(&sniffer_thread, NULL, handle_client, (void *)new_sock) < 0) {
            perror("Could not create thread");
            return 1;
        }

        pthread_join(sniffer_thread, NULL);
        printf("Handler assigned\n");
    }

    if (client_sock < 0) {
        perror("Accept failed");
        return 1;
    }

    return 0;
}
