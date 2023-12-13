#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <semaphore.h>

#define STACK_SIZE 3

typedef struct
{
    int *array;
    int max_size;
    int top;
    sem_t full;
    sem_t empty;
    sem_t mutex;
} Stack;

const int stack_size = STACK_SIZE;
Stack stack;

void initializeStack(Stack *stack, int max_size)
{
    stack->array = (int *)malloc(max_size * sizeof(int));
    stack->max_size = max_size;
    stack->top = 0;
    sem_init(&stack->full, 0, 0);
    sem_init(&stack->empty, 0, max_size);
    sem_init(&stack->mutex, 0, 1);
}

void pushToStack(Stack *stack, int value)
{
    sem_wait(&stack->empty);
    sem_wait(&stack->mutex);

    stack->array[stack->top] = value;
    printf("Pushing: %d\n", value);
    ++stack->top;

    sem_post(&stack->mutex);
    sem_post(&stack->full);
}

int popFromStack(Stack *stack)
{
    sem_wait(&stack->full);
    sem_wait(&stack->mutex);

    --stack->top;
    int popped_value = stack->array[stack->top];
    printf("Popping: %d\n", popped_value);

    sem_post(&stack->mutex);
    sem_post(&stack->empty);

    return popped_value;
}

void cleanupStack(Stack *stack)
{
    sem_destroy(&stack->full);
    sem_destroy(&stack->empty);
    sem_destroy(&stack->mutex);
    free(stack->array);
}

void *pushToStackThread(void *args)
{
    int value = *((int *)args);
    pushToStack(&stack, value);
    free(args);
    pthread_exit(NULL);
    return NULL;
}

void *popFromStackThread(void *args)
{
    popFromStack(&stack);
    pthread_exit(NULL);
    return NULL;
}

int main()
{
    initializeStack(&stack, stack_size);

    pthread_t threads[stack_size * 2];

    for (int i = 0; i < stack_size; ++i)
    {
        int *value = (int *)malloc(sizeof(int));
        *value = i;
        pthread_create(&threads[i], NULL, pushToStackThread, (void *)value);
    }

    for (int i = stack_size; i < stack_size * 2; ++i)
    {
        pthread_create(&threads[i], NULL, popFromStackThread, NULL);
    }

    for (int i = 0; i < stack_size * 2; ++i)
    {
        pthread_join(threads[i], NULL);
    }

    cleanupStack(&stack);

    return 0;
}
