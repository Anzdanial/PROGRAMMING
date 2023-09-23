#include <iostream>
// #include<stdio.h>
#include<sys/types.h>
#include<sys/stat.h>
#include<fcntl.h>
#include<sys/wait.h>
#include<unistd.h> //for fork
#include<stdlib.h>  //for atoi
#include<string.h>
#include <cstdlib> // for atoi

using namespace std;

int main(int argc, char** argv){
    int fd1,fd2,n;
    char buffer[100];
    if(argc==1){
        cout<<"Please enter the input file name\n";
        return 1;
    }
    fd1 = open(argv[1],O_RDONLY);
    fd2 = open("output.txt",O_WRONLY);
    bool flag1 = true,flag2 = true;
    char marks[4];
    int b=0;
    int i=0;
    cout<<"We are in parent process\n";
    int pid = fork();
    if(pid==-1){
        cerr<<"Error in forking child\n";
        return 1;
    }
    else if(pid==0){
        cout<<"We are in child process\n";
        while((n=read(fd1,buffer,1))!=0){
            write(1,buffer,n);
        }
        cout<<endl;
        return 0;
    }
    else{
        wait(NULL);
        pid = fork();
        if(pid==-1){
            cerr<<"Error in forking child\n";
            return 1;
        }
        else if(pid==0){
            cout<<"We are in second child\n";
            fd1 = open(argv[1],O_RDONLY);
            while((n=read(fd1,buffer,1)!=0)){
                if(buffer[0] == ' ' && flag1 == true){
                    flag1 = false;
                    write(fd2,buffer,n);
                    continue;
                }
                if(flag1 == false && buffer[0] != ' '){
                    marks[b] = buffer[0];
                    b++;
                }
                else if(flag1==false && buffer[0]==' '){
                    write(fd2,buffer,n);
                    flag1 = true;
                    continue;

                }
                else if(buffer[0]=='I' || buffer[0] == 'i'){
                    marks[b] = '\0';
                    b=0;
                    int Marks = atoi(marks);
                    if(Marks >= 80 && Marks <= 100){
                        buffer[0] = 'A';
                    }
                    else if(Marks>=70 && Marks<=79){
                        buffer[0] = 'B';
                    }
                    else if(Marks>=60 && Marks<=69){
                        buffer[0] = 'C';
                    }
                    else if(Marks>=50 && Marks<=59){
                        buffer[0] = 'D';
                    }
                    else if(Marks<50){
                        buffer[0] = 'F';
                    }

                }
                // cout<<buffer<<endl;
                // i++;
                write(fd2,buffer,n);
            }
            return 0;
        }
        wait(NULL);
        cout<<"Back in parent process\n";
    }


    return 0;
}