#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/wait.h>

void total_time_calculator(int laps, int lap_time) {
    int total_seconds = 0;
    for (int i = 0; i < laps * lap_time; i++) {
        sleep(1);
        total_seconds++;
    }
    printf("Total Time: %d seconds\n", total_seconds);
}

void lap_time_calculator(int laps, int lap_time) {
    for (int i = 0; i < laps; i++) {
        int lap_seconds = 0;
        for (int j = 0; j < lap_time; j++) {
            sleep(1);
            lap_seconds++;
        }
        printf("Lap %d Time: %d seconds\n", i + 1, lap_seconds);
    }
}

int main(int argc, char *argv[]) {
    if (argc != 3) {
        printf("Usage: %s <number_of_laps> <lap_time>\n", argv[0]);
        return 1;
    }

    int laps = atoi(argv[1]);
    int lap_time = atoi(argv[2]);

    pid_t pid1, pid2;
    pid1 = fork();

    if (pid1 < 0) {
        fprintf(stderr, "Fork Failed");
        return 1;
    } else if (pid1 == 0) {
        total_time_calculator(laps, lap_time);
        exit(0);
    } else {
        pid2 = fork();
        if (pid2 < 0) {
            fprintf(stderr, "Fork Failed");
            return 1;
        } else if (pid2 == 0) {
            lap_time_calculator(laps, lap_time);
            exit(0);
        } else {
            wait(NULL);
            wait(NULL);
            printf("Both processes have finished\n");
        }
    }

    return 0;
}
