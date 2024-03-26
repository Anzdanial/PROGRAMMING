cmake_minimum_required(VERSION 3.27)
project(CN_LABS_7)

set(CMAKE_CXX_STANDARD 17)

add_executable(CN_LABS_7 main.cpp
        client.c
        server.c
        serverVoting.c
        clientVoting.c
)
