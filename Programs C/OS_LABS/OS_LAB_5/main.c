#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <fcntl.h>
#include <string.h>
#include <sys/stat.h>

//main file hai ye

double add(double val1, double val2){
	return val1+val2;
}
double subtract(double val1, double val2){
	return val1-val2;
}
double divide(double val1, double val2){
	if(val2 == 0)
		return 0;
	return val1/val2;
}
double multiply(double val1, double val2){
	return val1*val2;
}

//READ FILE
int main() {
	const char* pipeName = "/tmp/ass_2.1";
	char buffer[1024];
	int fd = open(pipeName, O_RDONLY);

	if (fd == -1) {
		perror("open");
		exit(EXIT_FAILURE);
	}

	read(fd, buffer, sizeof(buffer));
	close(fd);

	int size;
	for(size = 0; buffer[size] != '\0'; size++);
	size = size - 1;
	char operator;
	char operand1[1024], operand2[1024];
	int spaceCounter = 0;
	int indexStart, indexEnd;
	for(int i = 0; i < size; i++){
		if(buffer[i+1] == ' ' && spaceCounter == 0){
			operator = buffer[i];
			spaceCounter++;
			indexStart = i+2;
		}
		else if(buffer[i+1] == ' ' && spaceCounter == 1){
			indexEnd = i+1;
			for(int j = indexStart, k = 0; j < indexEnd; j++, k++) {
				operand1[k] = buffer[j];
			}
			spaceCounter++;
			indexStart = indexEnd+1;
		}
		else if(i+1 == size && spaceCounter == 2){
			indexEnd = i+1;
			for(int j = indexStart, k = 0; j < indexEnd; j++, k++) {
				operand2[k] = buffer[j];
			}
		}
	}


	int intOperand1 = atoi(operand1);
	int intOperand2 = atoi(operand2);
	double result = 0;
	if(operator == '+'){
		result = add(intOperand1,intOperand2);
	}
	else if(operator == '-'){
		result = subtract(intOperand1,intOperand2);
	}
	else if(operator == '*'){
		result = multiply(intOperand1,intOperand2);
	}
	else if(operator == '/'){
		result = divide(intOperand1,intOperand2);
	}

	const char* pipeName1 = "/tmp/ass_2.2";
	mkfifo(pipeName1, 0666);

	int fd1 = open(pipeName1, O_WRONLY);
	if (fd1 == -1) {
		perror("open");
		exit(EXIT_FAILURE);
	}

	write(fd1, &result, sizeof(result));
	close(fd1);

	return 0;
}
