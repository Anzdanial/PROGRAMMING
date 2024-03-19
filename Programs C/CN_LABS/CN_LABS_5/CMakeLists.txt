cmake_minimum_required(VERSION 3.22)
project(CN_LABS_5 C)

set(CMAKE_C_STANDARD 11)

add_executable(CN_LABS_5 main.c
        client-q1.c
        server-q1.c
        server-q2.c
        client-q2.c)
