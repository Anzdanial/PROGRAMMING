#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <semaphore.h>
#include <pthread.h>
#include <sys/shm.h>
#include <sys/mman.h>
#include <fcntl.h>

#define SIZE 1024
#define SEMAPHORE_2 "/sem2"
#define SEMAPHORE_3 "/sem3"

sem_t semaphore_1, semaphore_2;
sem_t *semaphore_3, *semaphore_4;
key_t key;
int shared_memory_id;
char *values[SIZE];

void *readFunction(void *arg) {
    key_t key2 = ftok("file2.txt", 66);
    int id2 = shmget(key2, SIZE, 0666 | IPC_CREAT);
    char *ptr2 = (char *)shmat(id2, NULL, 0);

    FILE *file;
    char data[SIZE];
    size_t fileSize;
    file = fopen(values[1], "rb");
    fseek(file, 0, SEEK_END);
    fileSize = ftell(file);
    rewind(file);
    fread(data, 1, fileSize, file);
    fclose(file);

    data[fileSize] = '\0';
    strncpy(ptr2, data, SIZE);
    shmdt(ptr2);
    sem_post(semaphore_3);
    return NULL;
}

void *writeFunction(void *arg) {
    int size = strlen(values[2]);
    char buffer[size + 1];
    strcpy(buffer, values[2]);
    buffer[size] = '\0';

    FILE *file;
    file = fopen(values[1], "wb");
    if (file == NULL) {
        perror("Error with file. ");
        exit(EXIT_FAILURE);
    }

    size_t total = fwrite(buffer, 1, strlen(buffer), file);
    fclose(file);
    return NULL;
}

int main() {
    semaphore_4 = sem_open(SEMAPHORE_2, O_CREAT, 0666, 0);
    semaphore_3 = sem_open(SEMAPHORE_3, O_CREAT, 0666, 0);

    sem_init(&semaphore_1, 1, 1);
    sem_init(&semaphore_2, 1, 1);

    key = ftok("file.txt", 65);
    shared_memory_id = shmget(key, SIZE, 0666 | IPC_CREAT);

    char *ptr = (char *)shmat(shared_memory_id, (void *)0, 0);
    printf("Waiting\n");
    sem_wait(semaphore_4);

    printf("Content of old shared memory: %s\n", ptr);
    char *token = strtok(ptr, " ");
    int index = 0;

    while (token != NULL) {
        values[index++] = strdup(token);
        token = strtok(NULL, " ");
    }
    values[index] = NULL;

    pthread_t thread1, thread2;
    sem_wait(&semaphore_1);

    if (strcmp(values[0], "read") == 0) {
        pthread_create(&thread1, NULL, readFunction, NULL);
        pthread_join(thread1, NULL);
    } else {
        sem_wait(&semaphore_2);
        pthread_create(&thread2, NULL, writeFunction, NULL);
        pthread_join(thread2, NULL);
        sem_post(&semaphore_2);
    }

    int unlink_status = shmdt(ptr);
    shmctl(shared_memory_id, IPC_RMID, NULL);
    sem_post(&semaphore_1);

    return 0;
}
