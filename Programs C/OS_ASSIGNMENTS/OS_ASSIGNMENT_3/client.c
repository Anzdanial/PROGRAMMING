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

sem_t semaphore_1;

int main(int argc, char *argv[]) {
    char *command = argv[1];
    char *data;
    char *fileName;

    sem_init(&semaphore_1, 0, 1);

    sem_t *semaphore_2 = sem_open(SEMAPHORE_2, 0);
    sem_t *semaphore_3 = sem_open(SEMAPHORE_3, 0);

    if (strcmp(command, "read") == 0 && argc == 3) {
        fileName = argv[2];
        sem_wait(&semaphore_1);

        size_t l1 = strlen(fileName);
        size_t l2 = strlen(command);

        char buffer[l1 + l2 + 1];
        snprintf(buffer, SIZE, "%s %s", command, fileName);

        key_t key = ftok("file.txt", 65);
        int id = shmget(key, SIZE, 0666 | IPC_CREAT);
        char *ptr = (char *)shmat(id, (void *)0, 0);
        strncpy(ptr, buffer, SIZE);

        sem_post(semaphore_2);

        int unlink_status = shmdt(ptr);
        shmctl(key, IPC_RMID, NULL);
        sem_wait(semaphore_3);

        key_t key2 = ftok("file2.txt", 66);
        int id2 = shmget(key2, SIZE, 0666 | IPC_CREAT);
        char *ptr2 = (char *)shmat(id2, (void *)0, 0);
        printf("Content from file:  %s\n", ptr2);
        sem_post(&semaphore_1);
    } else if (strcmp(command, "write") == 0 && argc == 4) {
        fileName = argv[2];
        data = argv[3];
        sem_wait(&semaphore_1);

        size_t l1 = strlen(fileName);
        size_t l2 = strlen(command);
        size_t l3 = strlen(data);
        char buffer[l1 + l2 + l3 + 1];
        snprintf(buffer, SIZE, "%s %s %s", command, fileName, data);

        key_t key = ftok("file.txt", 65);
        int id = shmget(key, SIZE, 0666 | IPC_CREAT);
        char *ptr = (char *)shmat(id, (void *)0, 0);
        strncpy(ptr, buffer, SIZE);

        sem_post(&semaphore_1);
        sem_post(semaphore_2);
    } else {
        printf("Error.");
    }
}
