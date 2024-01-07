#include <stdio.h>
#include <unistd.h>
#include <signal.h>
#include <stdlib.h>
#include <sys/wait.h>

int timeCounter = 0;  // Declare timeCounter as a global variable

void signalHandler(int signalNumber) {
    if (signalNumber == SIGCHLD) {
        printf("Received SIGCHLD\n");
        printf("Total Time in Seconds: %d\n", timeCounter);
        exit(0);
    }
}

int main() {
    int numLaps, lapTime;
    printf("Enter the Number of Laps: ");
    scanf("%d", &numLaps);
    printf("Enter the Lap Time: ");
    scanf("%d", &lapTime);

    int val = fork();

    if (val == 0) {
        int counter = 0;
        while (counter != numLaps) {
            printf("Lap: %d Completed\n", counter + 1);
            sleep(lapTime);
            timeCounter += lapTime;
            counter++;
        }
        exit(0);
    } else if (val > 0) {
        if (signal(SIGCHLD, signalHandler) == SIG_ERR) {
            printf("Couldn't Catch SIGCHLD\n");
            return 1;  // Exit the program with an error code
        }

        wait(NULL);
    } else {
        // Fork failed
        printf("Fork failed\n");
        return 1;  // Exit the program with an error code
    }

    return 0;
}
