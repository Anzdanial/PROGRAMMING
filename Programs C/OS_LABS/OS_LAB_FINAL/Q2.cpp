#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

#define MONKEYS 10

int direction; // 0 for west, 1 for east
int waiting_monkeys = 0;
int rope_users = 0;
pthread_mutex_t rope_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_cond_t rope_cv = PTHREAD_COND_INITIALIZER;
pthread_mutex_t direction_mutex = PTHREAD_MUTEX_INITIALIZER;

void *waitForTurn(void *arg) {
    pthread_mutex_lock(&direction_mutex);
    int monkey_direction = *(int *)arg;
    direction = monkey_direction;
    pthread_mutex_unlock(&direction_mutex);

    pthread_mutex_lock(&rope_mutex);
    waiting_monkeys++;

    while (direction != monkey_direction) {
        pthread_cond_wait(&rope_cv, &rope_mutex);
    }

    if (rope_users == 0) {
        rope_users = MONKEYS;
    } else {
        rope_users--;
    }

    pthread_mutex_unlock(&rope_mutex);
    return NULL;
}

void *CrossRavine(void *arg) {
    // cross the ravine
    printf("cross \n");
    return NULL;
}

void *finishedRoping(void *arg) {
    pthread_mutex_lock(&rope_mutex);
    rope_users++;
    waiting_monkeys--;

    if (waiting_monkeys > 0) {
        pthread_cond_broadcast(&rope_cv);
    }

    pthread_mutex_unlock(&rope_mutex);
    return NULL;
}

int main() {
    pthread_t waitForTurnThreads[MONKEYS];
    pthread_t crossRavineThreads[MONKEYS];
    pthread_t finishedRopingThreads[MONKEYS];

    int i, d;

    for (i = 0; i < MONKEYS; i++) {
        d = rand() % 2;
        pthread_create(&waitForTurnThreads[i], NULL, waitForTurn, (void *)&d);
        pthread_create(&crossRavineThreads[i], NULL, CrossRavine, NULL);
        pthread_create(&finishedRopingThreads[i], NULL, finishedRoping, NULL);
    }

    for (i = 0; i < MONKEYS; i++) {
        pthread_join(waitForTurnThreads[i], NULL);
        pthread_join(crossRavineThreads[i], NULL);
        pthread_join(finishedRopingThreads[i], NULL);
    }

    return 0;
}
