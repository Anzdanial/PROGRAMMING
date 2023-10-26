#include <stdio.h>
#include <pthread.h>
#include <unistd.h>
#include <sys/syscall.h>

void *threadFunction(void *arg) {
	int threadNum = (intptr_t)arg;
	pid_t tid = syscall(SYS_gettid);
	printf("I am in thread no: %d with Thread ID: %d\n", threadNum, (int)tid);
//    pthread_exit((void *) (intptr_t )threadNum);
}

int main() {
	int N;
	printf("Enter the number of threads to create: ");
	scanf("%d", &N);
	pthread_t thread[N];
	for (int i = 0; i < N; i++) {
		pthread_create(&thread[i], NULL, threadFunction, (void *)(intptr_t )i);
	}
	for (int i = 0; i < N; i++) {
		pthread_join(thread[i], NULL);
	}
	return 0;
}