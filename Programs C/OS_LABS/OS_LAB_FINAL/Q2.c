#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <semaphore.h>
#include <unistd.h>

#define MAX_MONKEYS 5
#define NUM_MONKEYS 10

sem_t mutex, rope, turn;
int monkeys_on_rope = 0;
int direction_count[2] = {0}; // 0 for eastward, 1 for westward

void waitforTurn(char *direction) {
    sem_wait(&mutex);
    sem_post(&mutex);

    printf("Monkey %s waiting for its turn\n", direction);
}

void crossRavine(char *direction) {
    sem_wait(&rope);

    sem_wait(&mutex);
    monkeys_on_rope++;

    if (monkeys_on_rope == 1) {
        sem_wait(&turn);
    }

    sem_post(&mutex);

    printf("Monkey %s is crossing the ravine. Monkeys on rope: %d\n", direction, monkeys_on_rope);
    sleep(1);

    sem_wait(&mutex);
    monkeys_on_rope--;

    if (monkeys_on_rope == 0) {
        sem_post(&turn);
    }
    sem_post(&mutex);

    sem_post(&rope);
}

void finishedRoping(char *direction) {
    printf("Monkey %s finished roping. Monkeys on rope: %d\n", direction, monkeys_on_rope);
}

void *monkeyThread(void *arg) {
    char *direction = *((char **)arg);

    waitforTurn(direction);
    crossRavine(direction);
    finishedRoping(direction);

    return NULL;
}

int main() {
    pthread_t monkeys[NUM_MONKEYS];
    char *directions[NUM_MONKEYS] = {"Eastward", "Westward"};

    sem_init(&mutex, 0, 1);
    sem_init(&rope, 0, MAX_MONKEYS);
    sem_init(&turn, 0, MAX_MONKEYS);

    for (int i = 0; i < NUM_MONKEYS; i++) {
        pthread_create(&monkeys[i], NULL, monkeyThread, &directions[i % 2]);
    }

    for (int i = 0; i < NUM_MONKEYS; i++) {
        pthread_join(monkeys[i], NULL);
    }

    sem_destroy(&mutex);
    sem_destroy(&rope);
    sem_destroy(&turn);

    return 0;
}
