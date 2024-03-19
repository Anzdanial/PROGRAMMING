#include <stdio.h>
#include <stdlib.h>
#include <ctype.h>

void swap(int *a, int *b) {
    int temp = *a;
    *a = *b;
    *b = temp;
}

int partition(int arr[], int low, int high) {
    int pivot = arr[high];
    int i = (low - 1);

    for (int j = low; j <= high - 1; j++) {
        if (arr[j] < pivot) {
            i++;
            swap(&arr[i], &arr[j]);
        }
    }
    swap(&arr[i + 1], &arr[high]);
    return (i + 1);
}

void quickSort(int arr[], int low, int high) {
    if (low < high) {
        int pi = partition(arr, low, high);
        quickSort(arr, low, pi - 1);
        quickSort(arr, pi + 1, high);
    }
}

int main() {
    FILE *input_file, *output_file;
    int numbers[1000];
    int num_count = 0;
    char ch;

    input_file = fopen("input.txt", "r");
    if (input_file == NULL) {
        printf("Error opening input file.\n");
        return 1;
    }

    while ((ch = fgetc(input_file)) != EOF) {
        if (isdigit(ch)) {
            numbers[num_count++] = ch - '0';
        }
    }

    fclose(input_file);

    quickSort(numbers, 0, num_count - 1);

    output_file = fopen("output.txt", "w");
    if (output_file == NULL) {
        printf("Error opening output file.\n");
        return 1;
    }

    for (int i = 0; i < num_count; i++) {
        fprintf(output_file, "%d ", numbers[i]);
    }

    fclose(output_file);

    printf("Digits sorted and saved to output.txt successfully.\n");

    return 0;
}
