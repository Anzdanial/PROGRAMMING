# CMAKE generated file: DO NOT EDIT!
# Generated by "MinGW Makefiles" Generator, CMake Version 3.26

# Delete rule output on recipe failure.
.DELETE_ON_ERROR:

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Disable VCS-based implicit rules.
% : %,v

# Disable VCS-based implicit rules.
% : RCS/%

# Disable VCS-based implicit rules.
% : RCS/%,v

# Disable VCS-based implicit rules.
% : SCCS/s.%

# Disable VCS-based implicit rules.
% : s.%

.SUFFIXES: .hpux_make_needs_suffix_list

# Command-line flag to silence nested $(MAKE).
$(VERBOSE)MAKESILENT = -s

#Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

SHELL = cmd.exe

# The CMake executable.
CMAKE_COMMAND = C:\Users\anzda\AppData\Local\Programs\CLion\bin\cmake\win\x64\bin\cmake.exe

# The command to remove a file.
RM = C:\Users\anzda\AppData\Local\Programs\CLion\bin\cmake\win\x64\bin\cmake.exe -E rm -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2"

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\cmake-build-debug"

# Include any dependencies generated for this target.
include CMakeFiles/P1.dir/depend.make
# Include any dependencies generated by the compiler for this target.
include CMakeFiles/P1.dir/compiler_depend.make

# Include the progress variables for this target.
include CMakeFiles/P1.dir/progress.make

# Include the compile flags for this target's objects.
include CMakeFiles/P1.dir/flags.make

CMakeFiles/P1.dir/main.c.obj: CMakeFiles/P1.dir/flags.make
CMakeFiles/P1.dir/main.c.obj: D:/PROGRAMMING/Programs\ C/OS_LABS/OS_LAB_2/main.c
CMakeFiles/P1.dir/main.c.obj: CMakeFiles/P1.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir="D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\cmake-build-debug\CMakeFiles" --progress-num=$(CMAKE_PROGRESS_1) "Building C object CMakeFiles/P1.dir/main.c.obj"
	C:\Users\anzda\AppData\Local\Programs\CLion\bin\mingw\bin\gcc.exe $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -MD -MT CMakeFiles/P1.dir/main.c.obj -MF CMakeFiles\P1.dir\main.c.obj.d -o CMakeFiles\P1.dir\main.c.obj -c "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\main.c"

CMakeFiles/P1.dir/main.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/P1.dir/main.c.i"
	C:\Users\anzda\AppData\Local\Programs\CLion\bin\mingw\bin\gcc.exe $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\main.c" > CMakeFiles\P1.dir\main.c.i

CMakeFiles/P1.dir/main.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/P1.dir/main.c.s"
	C:\Users\anzda\AppData\Local\Programs\CLion\bin\mingw\bin\gcc.exe $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\main.c" -o CMakeFiles\P1.dir\main.c.s

# Object files for target P1
P1_OBJECTS = \
"CMakeFiles/P1.dir/main.c.obj"

# External object files for target P1
P1_EXTERNAL_OBJECTS =

P1.exe: CMakeFiles/P1.dir/main.c.obj
P1.exe: CMakeFiles/P1.dir/build.make
P1.exe: CMakeFiles/P1.dir/linkLibs.rsp
P1.exe: CMakeFiles/P1.dir/objects1.rsp
P1.exe: CMakeFiles/P1.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir="D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\cmake-build-debug\CMakeFiles" --progress-num=$(CMAKE_PROGRESS_2) "Linking C executable P1.exe"
	$(CMAKE_COMMAND) -E cmake_link_script CMakeFiles\P1.dir\link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
CMakeFiles/P1.dir/build: P1.exe
.PHONY : CMakeFiles/P1.dir/build

CMakeFiles/P1.dir/clean:
	$(CMAKE_COMMAND) -P CMakeFiles\P1.dir\cmake_clean.cmake
.PHONY : CMakeFiles/P1.dir/clean

CMakeFiles/P1.dir/depend:
	$(CMAKE_COMMAND) -E cmake_depends "MinGW Makefiles" "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2" "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2" "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\cmake-build-debug" "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\cmake-build-debug" "D:\PROGRAMMING\Programs C\OS_LABS\OS_LAB_2\cmake-build-debug\CMakeFiles\P1.dir\DependInfo.cmake" --color=$(COLOR)
.PHONY : CMakeFiles/P1.dir/depend
