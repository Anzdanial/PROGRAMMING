#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>

int main() {
	const char* pipeName = "/tmp/story_pipe";
	mkfifo(pipeName, 0666);
	char buffer[1024] = "A computer would deserve to be called intelligent if it could deceive a human into believing that it was human";
	int fd = open(pipeName, O_WRONLY);

	if (fd == -1) {
		perror("open");
		exit(EXIT_FAILURE);
	}

	write(fd, buffer, sizeof(buffer));
	close(fd);

	return 0;
}