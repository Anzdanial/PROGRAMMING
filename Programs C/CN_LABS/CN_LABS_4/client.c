#include <stdio.h>
#include <sys/socket.h>
#include <arpa/inet.h>


int main(){
    int socket_stat = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    struct sockaddr_in server_addr;
    return 0;
}