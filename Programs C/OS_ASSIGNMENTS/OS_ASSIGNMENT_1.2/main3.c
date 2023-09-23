#include<iostream>
#include<stdio.h>
#include<sys/types.h>
#include<sys/wait.h>
#include<unistd.h> //for fork
#include<stdlib.h>  //for atoi

using namespace std;

int main(int argc,char**argv){
    // cout<<"Parent ID ........"<<getppid()<<endl;
    if(argc==1){
        cerr<<"Please enter number between 1 to 10 for making processes\n";
        return 1;
    }
    if(argc>2){
        cerr<<"Please enter only one argument\n";
        return 1;
    }
    if(atoi(argv[1])<1 || atoi(argv[1])>10){
        cerr<<"Error: Please enter a number between 1 to 10\n";
        return 1;
    }
    int total = atoi(argv[1]);

    pid_t pid1;

    // cout<<"Very first"<<getppid()<<endl;
    pid1 = fork();
    total--;

    if(pid1==0){
        pid_t pid2 = 0;  // for 1st iteration
        while(total>0 && pid2==0){
            pid2 = fork();
            total--;
        }
        if(pid2==0){
            cout<<"Child process ID ("<<getpid()<<") Parent ID ("<<getppid()<<")"<<endl;
        }
        else if(pid2>0){
            wait(NULL);
            cout<<"Parent Process ID ("<<getpid()<<") Parent ID ("<<getppid()<<")"<<endl;
        }
        else if(pid2<0){
            cerr<<"Sorry unable to create child process\n";
        }
    }
    else if(pid1>0){
        wait(NULL);
        cout<<"First Parent ID ("<<getpid()<<endl;
    }
    else if(pid1<0){
        cerr<<"Sorry unable to create very first child\n";
    }

    return 0;
}

