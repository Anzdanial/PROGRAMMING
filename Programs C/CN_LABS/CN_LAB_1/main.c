#include <stdio.h>

int main() {
	FILE *file;
	file = fopen("input.txt", "r");

	if(file == NULL){
		perror("Error opening file");
		return 1;
	}

	if (file != NULL) {
		char buffer[1024];
		while (fgets(buffer, sizeof(buffer), file) != NULL) {
			if(buffer > 47 && buffer)
		}
		fclose(file);
	}
	return 0;
}
