#include <stdio.h>
#include <semaphore.h>
#include <pthread.h>

int X, Y, W, Z;
int X1, Y1, W1, Z1, S1, S2;

sem_t input1, input2;

void *Input1(void *arg){
	sem_wait(&input1);
	printf("Enter the Value of X: ");
	scanf("%d",&X);
	printf("Enter the Value of Y: ");
	scanf("%d",&Y);
	sem_post(&input2);
	sem_wait(&input1);
	X1 = Z+2;
	sem_post(&input2);
	sem_wait(&input1);
	Y1 = Z1*5;
	sem_post(&input2);
	sem_wait(&input1);
	S1 = X1+Y1;
	sem_post(&input2);
	sem_wait(&input1);
	printf("x = %d\n",S1);
	sem_post(&input2);
	return NULL;
}

void *Input2(void *arg){
	sem_wait(&input2);
	printf("Enter the Value of Z: ");
	scanf("%d",&Z);
	printf("Enter the Value of W: ");
	scanf("%d",&W);
	sem_post(&input1);
	sem_wait(&input2);
	Z1= X1*2;
	sem_post(&input1);
	sem_wait(&input2);
	W1=Y1+5;
	sem_post(&input1);
	sem_wait(&input2);
	S2=Z1+W1;
	sem_post(&input1);
	sem_wait(&input2);
	printf("x = %d\n",S2);
	return NULL;
}


int main(int argc, char **argv) {
	sem_init(&input1, 0, 1);
	sem_init(&input2, 0,0);
	pthread_t thread1, thread2;
	pthread_create(&thread1, NULL, Input1, NULL);
	pthread_create(&thread2, NULL, Input2, NULL);
	pthread_join(thread1, NULL);
	pthread_join(thread2, NULL);

	return 0;
}
