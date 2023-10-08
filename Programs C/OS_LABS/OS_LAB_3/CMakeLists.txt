cmake_minimum_required(VERSION 3.22)
project(OS_LAB_3 C)

set(CMAKE_C_STANDARD 11)

add_executable(OS_LAB_3 main.c EXEC_QUESTIONS/my_info.c EXEC_QUESTIONS/fibonacci.c functions.c header.h)
