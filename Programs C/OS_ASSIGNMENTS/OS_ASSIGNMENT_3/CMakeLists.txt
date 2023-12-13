cmake_minimum_required(VERSION 3.22)
project(OS_ASSIGNMENT_3 C)

set(CMAKE_C_STANDARD 11)

add_executable(OS_ASSIGNMENT_3 main.c
        main.cpp
        client.c server.c)
