#include <stdio.h>
#include <pthread.h>
#include <unistd.h>
#include <sys/syscall.h>
#include <stdlib.h>

int N = 0;
int even = 0;
int odd = 0;
int sum = 0;

void *summation(void *arg) {
	pid_t tid = syscall(SYS_gettid);
	int *series;
	series = (int*)(intptr_t)arg;
	for(int i = 0; i < N; i++){
		sum += series[i];
	}
	fprintf(stderr, "Thread ID %d for summation \n", tid);
	return(NULL);
}

void *countOdd(void *arg) {
	pid_t tid = syscall(SYS_gettid);
	int *series;
	series = (int*)(intptr_t)arg;
	for(int i = 0; i < N; i++){
		if(series[i] % 2 != 0){
			odd++;
		}
	}
	fprintf(stderr, "Thread ID %d for countOdd \n", tid);
	return(NULL);
}

void *countEven(void *arg) {
	pid_t tid = syscall(SYS_gettid);
	int *series;
	series = (int*)(intptr_t)arg;
	for(int i = 0; i < N; i++){
		if(series[i] % 2 == 0){
			even++;
		}
	}
	fprintf(stderr, "Thread ID %d for countEven \n", tid);
	return(NULL);
}

void *fibonacciGenerator(void *arg) {
	pid_t tid = syscall(SYS_gettid);
	int *series;
	series = (int*)(intptr_t)arg;
	int t1 = 0, t2 = 1;
	series[0] = t1; series[1] = t2;
	int nextTerm = t1 + t2;
	for (int i = 2; i <= N; ++i) {
		series[i] = nextTerm;
		t1 = t2;
		t2 = nextTerm;
		nextTerm = t1 + t2;
	}
	fprintf(stderr, "Thread ID %d for Fibonacci Generator \n", tid);
	return(NULL);
}

int main(int argc, char** argv) {
	if(argc < 2){
		fprintf(stderr,"No Input for N Given \n");
		return 1;
	}
	N = atoi(argv[1]);
	int *series = (int*)malloc(N*sizeof(int));
	pthread_t thread[4];
	pthread_create(&thread[0], NULL, fibonacciGenerator, (void*)series);
	pthread_join(thread[0],NULL);
	for(int i = 0; i < N; i++){
		printf("%d, ", series[i]);
	}
	printf("\n\n");

	pthread_create(&thread[1], NULL, countEven, (void*)series);
	pthread_join(thread[1],NULL);
	printf("The number of Even Numbers in the Series is: %d \n", even);
	printf("\n\n");

	pthread_create(&thread[2], NULL, countOdd, (void*)series);
	pthread_join(thread[2],NULL);
	printf("The number of Odd Numbers in the Series is: %d \n", odd);
	printf("\n\n");

	pthread_create(&thread[3], NULL, summation, (void*)series);
	pthread_join(thread[3],NULL);
	printf("The sum of Numbers in the Series is: %d \n", sum);
	printf("\n");

	FILE *file = fopen("sum.txt", "w");
	if (file == NULL) {
		printf("Error opening file!\n");
		exit(1);
	}
	fprintf(file, "%d", sum);
	fclose(file);

	return 0;
}