#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>

void fileRedirection(){
	char data[1024];
	int fdw1, fdw2, fdr1, fdr2;
	fdr1 = open("file.txt",O_RDONLY);
	fdr2 = dup2(fdr1,0);
	int size = read(fdr1,data, sizeof(data));
	close(fdr1);
	close(fdr2);
	data[size] = '\0';
	fdw1 = open("output.txt", O_WRONLY);
//	fdw2 = dup2(fdw1,1);
	dup2(fdw1,1);
	printf("%s", data);
//	write(fdw2, data, size);
//	close(fdw1);
//	close(fdw2);
}

void shellCommandCall() {
	int fd = open("output.txt", O_WRONLY | O_CREAT | O_TRUNC, 0666);

	if (fd == -1) {
		perror("open");
		return;
	}

	dup2(fd, STDOUT_FILENO); // Redirect stdout to the file
	close(fd);

	execlp("/bin/cat", "cat", "input.txt", NULL);

	// If execlp fails
	perror("execlp");
}

int main() {
//	fileRedirection();
	shellCommandCall();
	return 0;
}
