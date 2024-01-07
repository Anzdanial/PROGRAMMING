#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <ctype.h>

int compare_int(const void* a, const void* b) {
    return (*(int*)a - *(int*)b);
}

struct ThreadData {
    const char* filename;
    int* numbers;
    int count;
};

void* process_data(void* arg) {
    struct ThreadData* thread_data = (struct ThreadData*)arg;

    FILE* file = fopen(thread_data->filename, "r");
    if (file == NULL) {
        perror("Error opening file");
        exit(EXIT_FAILURE);
    }

    int num;
    int count = 0;

    while (fscanf(file, "%d", &num) == 1) {
        thread_data->numbers[count++] = num;
    }

    int c;
    while ((c = fgetc(file)) != EOF) {
        if (isdigit(c)) {
            ungetc(c, file);
            if (fscanf(file, "%d", &num) == 1) {
                thread_data->numbers[count++] = num;
            }
        }
    }

    fclose(file);
    thread_data->count = count;

    //using built in qsort function for always ascending
    qsort(thread_data->numbers, count, sizeof(int), compare_int);
    pthread_exit(NULL);
}

void merge_arrays(int* arr1, int size1, int* arr2, int size2, int* result) {
    int i = 0, j = 0, k = 0;
    while (i < size1 && j < size2) {
        if (arr1[i] < arr2[j]) {
            result[k++] = arr1[i++];
        } else {
            result[k++] = arr2[j++];
        }
    }
    while (i < size1) {
        result[k++] = arr1[i++];
    }
    while (j < size2) {
        result[k++] = arr2[j++];
    }
}

int main() {
    pthread_t thread1, thread2;
    struct ThreadData data1, data2;

    const char* filename1 = "datafile1.txt";
    const char* filename2 = "datafile2.txt";
    const int buffer_size = 100;

    data1.filename = filename1;
    data2.filename = filename2;

    data1.numbers = (int*)malloc(buffer_size * sizeof(int));
    data2.numbers = (int*)malloc(buffer_size * sizeof(int));

    pthread_create(&thread1, NULL, process_data, (void*)&data1);
    pthread_create(&thread2, NULL, process_data, (void*)&data2);

    pthread_join(thread1, NULL);
    pthread_join(thread2, NULL);

    int* merged_result = (int*)malloc((data1.count + data2.count) * sizeof(int));
    merge_arrays(data1.numbers, data1.count, data2.numbers, data2.count, merged_result);

    printf("DataFile 1: ");
    for (int i = 0; i < data1.count; ++i) {
        printf("%d ", data1.numbers[i]);
    }
    printf("\n");

    printf("DataFile 2: ");
    for (int i = 0; i < data2.count; ++i) {
        printf("%d ", data2.numbers[i]);
    }
    printf("\n");

    printf("Merged Result: ");
    for (int i = 0; i < data1.count + data2.count; ++i) {
        printf("%d ", merged_result[i]);
    }
    printf("\n");

    free(data1.numbers);
    free(data2.numbers);
    free(merged_result);

    return 0;
}
