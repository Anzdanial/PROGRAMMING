cmake_minimum_required(VERSION 3.22)
project(CN_LABS_6 C)

set(CMAKE_C_STANDARD 11)

add_executable(CN_LABS_6 main.c
        server.c
        client.c)
