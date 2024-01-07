#include <stdio.h>
#include <pthread.h>
#include <semaphore.h>


//Dependent Variables:
//X1, Y1, S1, Z1, W1, S2: These matrices are dependent variables as they are modified within the threads based on the computations performed.

// Independent Variables:
//two, fives, Z: These matrices are independent variables as they are not modified within the threads.

//CRITICAL SECTIONS:
//addMatrices(Z, two, X1, 3, 3): The critical section starts with sem_wait(&sem_M1) and ends with sem_post(&sem_M2).
//In this section, the matrix X1 is modified based on the addition of matrices Z and two.

//multiplyMatrices(Z1, fives, Y1, 3, 3, 3, 3): The critical section starts with sem_wait(&sem_M1) and ends with sem_post(&sem_M2).
//In this section, the matrix Y1 is modified based on the multiplication of matrices Z1 and fives.

//subtractMatrices(X1, Y1, S1, 3, 3): The critical section starts with sem_wait(&sem_M1) and ends with sem_post(&sem_M2).
//In this section, the matrix S1 is modified based on the subtraction of matrices X1 and Y1.

//multiplyMatrices(X1, two, Z1, 3, 3, 3, 3): The critical section starts with sem_wait(&sem_M2) and ends with sem_post(&sem_M1).
//In this section, the matrix Z1 is modified based on the multiplication of matrices X1 and two.

//addMatrices(Y1, fives, W1, 3, 3): The critical section starts with sem_wait(&sem_M2) and ends with sem_post(&sem_M1).
//In this section, the matrix W1 is modified based on the addition of matrices Y1 and fives.

//addMatrices(Z1, two, S2, 3, 3): The critical section starts with sem_wait(&sem_M2) and ends with sem_post(&sem_M1).
//In this section, the matrix S2 is modified based on the addition of matrices Z1 and two.



int two[3][3] = {{2, 2, 2}, {2, 2, 2}, {2, 2, 2}};
int fives[3][3] = {{5, 5, 5}, {5, 5, 5}, {5, 5, 5}};
int Z[3][3] = {{1, 1, 1}, {0, 4, 0}, {-1, -1, -1}};

int X1[3][3] = {0};
int Y1[3][3] = {0};
int S1[3][3] = {0};
int Z1[3][3] = {0};
int W1[3][3] = {0};
int S2[3][3] = {0};

sem_t sem_M1;
sem_t sem_M2;

void addMatrices(int matrix1[][3], int matrix2[][3], int result[][3], int rows, int columns) {
    for (int i = 0; i < rows; ++i) {
        for (int j = 0; j < columns; ++j) {
            result[i][j] = matrix1[i][j] + matrix2[i][j];
        }
    }
}

void multiplyMatrices(int firstMatrix[][3], int secondMatrix[][3], int result[][3], int rowFirst, int columnFirst, int rowSecond, int columnSecond) {
    for (int i = 0; i < rowFirst; ++i) {
        for (int j = 0; j < columnSecond; ++j) {
            result[i][j] = 0;
        }
    }

    for (int i = 0; i < rowFirst; ++i) {
        for (int j = 0; j < columnSecond; ++j) {
            for (int k = 0; k < columnFirst; ++k) {
                result[i][j] += firstMatrix[i][k] * secondMatrix[k][j];
            }
        }
    }
}

void subtractMatrices(int matrix1[][3], int matrix2[][3], int result[][3], int rows, int columns) {
    for (int i = 0; i < rows; ++i) {
        for (int j = 0; j < columns; ++j) {
            result[i][j] = matrix1[i][j] - matrix2[i][j];
        }
    }
}

void displayMatrix(int matrix[][3], int rows, int columns) {
    for (int i = 0; i < rows; ++i) {
        for (int j = 0; j < columns; ++j) {
            printf("%d\t", matrix[i][j]);
        }
        printf("\n");
    }
}

void *methodOne(void *arg){
    sem_wait(&sem_M1);
    addMatrices(Z,two,X1,3,3);
    sem_post(&sem_M2);
    sem_wait(&sem_M1);
    multiplyMatrices(Z1,fives,Y1,3,3,3,3);
    sem_post(&sem_M2);
    sem_wait(&sem_M1);
    subtractMatrices(X1,Y1,S1,3,3);
    sem_post(&sem_M2);
    sem_wait(&sem_M1);
    printf("The Matrix S1 is: \n");
    displayMatrix(S1,3,3);
    printf("\n");
    sem_post(&sem_M2);
    return NULL;
}

void *methodTwo(void *arg){
    sem_wait(&sem_M2);
    multiplyMatrices(X1,two,Z1,3,3,3,3);
    sem_post(&sem_M1);
    sem_wait(&sem_M2);
    addMatrices(Y1,fives,W1,3,3);
    sem_post(&sem_M1);
    sem_wait(&sem_M2);
    addMatrices(Z1,two,S2,3,3);
    sem_post(&sem_M1);
    sem_wait(&sem_M2);
    printf("The Matrix S2 is: \n");
    displayMatrix(S2,3,3);
    return NULL;
}

int main(){
    sem_init(&sem_M1, 0, 1);
    sem_init(&sem_M2, 0, 0);
    pthread_t threadOne, threadTwo;
    pthread_create(&threadOne, NULL, methodOne, NULL);
    pthread_create(&threadTwo, NULL, methodTwo, NULL);
    pthread_join(threadOne, NULL);
    pthread_join(threadTwo, NULL);
    return 0;
}