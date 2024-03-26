#include <stdio.h>

int main(){
    printf("Enter your Name: ");
    char name[200];
    if(fgets(name, sizeof(name), stdin) != NULL){
        printf("%s",name);
    }
    printf("%c", name[3]);
    return 0;
}