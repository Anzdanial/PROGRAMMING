# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.22

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

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/bin/cmake

# The command to remove a file.
RM = /usr/bin/cmake -E rm -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6"

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug"

# Include any dependencies generated for this target.
include CMakeFiles/CN_LABS_6.dir/depend.make
# Include any dependencies generated by the compiler for this target.
include CMakeFiles/CN_LABS_6.dir/compiler_depend.make

# Include the progress variables for this target.
include CMakeFiles/CN_LABS_6.dir/progress.make

# Include the compile flags for this target's objects.
include CMakeFiles/CN_LABS_6.dir/flags.make

CMakeFiles/CN_LABS_6.dir/main.c.o: CMakeFiles/CN_LABS_6.dir/flags.make
CMakeFiles/CN_LABS_6.dir/main.c.o: ../main.c
CMakeFiles/CN_LABS_6.dir/main.c.o: CMakeFiles/CN_LABS_6.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir="/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug/CMakeFiles" --progress-num=$(CMAKE_PROGRESS_1) "Building C object CMakeFiles/CN_LABS_6.dir/main.c.o"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -MD -MT CMakeFiles/CN_LABS_6.dir/main.c.o -MF CMakeFiles/CN_LABS_6.dir/main.c.o.d -o CMakeFiles/CN_LABS_6.dir/main.c.o -c "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/main.c"

CMakeFiles/CN_LABS_6.dir/main.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/CN_LABS_6.dir/main.c.i"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/main.c" > CMakeFiles/CN_LABS_6.dir/main.c.i

CMakeFiles/CN_LABS_6.dir/main.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/CN_LABS_6.dir/main.c.s"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/main.c" -o CMakeFiles/CN_LABS_6.dir/main.c.s

CMakeFiles/CN_LABS_6.dir/server.c.o: CMakeFiles/CN_LABS_6.dir/flags.make
CMakeFiles/CN_LABS_6.dir/server.c.o: ../server.c
CMakeFiles/CN_LABS_6.dir/server.c.o: CMakeFiles/CN_LABS_6.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir="/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug/CMakeFiles" --progress-num=$(CMAKE_PROGRESS_2) "Building C object CMakeFiles/CN_LABS_6.dir/server.c.o"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -MD -MT CMakeFiles/CN_LABS_6.dir/server.c.o -MF CMakeFiles/CN_LABS_6.dir/server.c.o.d -o CMakeFiles/CN_LABS_6.dir/server.c.o -c "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/server.c"

CMakeFiles/CN_LABS_6.dir/server.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/CN_LABS_6.dir/server.c.i"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/server.c" > CMakeFiles/CN_LABS_6.dir/server.c.i

CMakeFiles/CN_LABS_6.dir/server.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/CN_LABS_6.dir/server.c.s"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/server.c" -o CMakeFiles/CN_LABS_6.dir/server.c.s

CMakeFiles/CN_LABS_6.dir/client.c.o: CMakeFiles/CN_LABS_6.dir/flags.make
CMakeFiles/CN_LABS_6.dir/client.c.o: ../client.c
CMakeFiles/CN_LABS_6.dir/client.c.o: CMakeFiles/CN_LABS_6.dir/compiler_depend.ts
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --progress-dir="/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug/CMakeFiles" --progress-num=$(CMAKE_PROGRESS_3) "Building C object CMakeFiles/CN_LABS_6.dir/client.c.o"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -MD -MT CMakeFiles/CN_LABS_6.dir/client.c.o -MF CMakeFiles/CN_LABS_6.dir/client.c.o.d -o CMakeFiles/CN_LABS_6.dir/client.c.o -c "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/client.c"

CMakeFiles/CN_LABS_6.dir/client.c.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing C source to CMakeFiles/CN_LABS_6.dir/client.c.i"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -E "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/client.c" > CMakeFiles/CN_LABS_6.dir/client.c.i

CMakeFiles/CN_LABS_6.dir/client.c.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling C source to assembly CMakeFiles/CN_LABS_6.dir/client.c.s"
	/usr/bin/cc $(C_DEFINES) $(C_INCLUDES) $(C_FLAGS) -S "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/client.c" -o CMakeFiles/CN_LABS_6.dir/client.c.s

# Object files for target CN_LABS_6
CN_LABS_6_OBJECTS = \
"CMakeFiles/CN_LABS_6.dir/main.c.o" \
"CMakeFiles/CN_LABS_6.dir/server.c.o" \
"CMakeFiles/CN_LABS_6.dir/client.c.o"

# External object files for target CN_LABS_6
CN_LABS_6_EXTERNAL_OBJECTS =

CN_LABS_6: CMakeFiles/CN_LABS_6.dir/main.c.o
CN_LABS_6: CMakeFiles/CN_LABS_6.dir/server.c.o
CN_LABS_6: CMakeFiles/CN_LABS_6.dir/client.c.o
CN_LABS_6: CMakeFiles/CN_LABS_6.dir/build.make
CN_LABS_6: CMakeFiles/CN_LABS_6.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green --bold --progress-dir="/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug/CMakeFiles" --progress-num=$(CMAKE_PROGRESS_4) "Linking C executable CN_LABS_6"
	$(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/CN_LABS_6.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
CMakeFiles/CN_LABS_6.dir/build: CN_LABS_6
.PHONY : CMakeFiles/CN_LABS_6.dir/build

CMakeFiles/CN_LABS_6.dir/clean:
	$(CMAKE_COMMAND) -P CMakeFiles/CN_LABS_6.dir/cmake_clean.cmake
.PHONY : CMakeFiles/CN_LABS_6.dir/clean

CMakeFiles/CN_LABS_6.dir/depend:
	cd "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug" && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6" "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6" "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug" "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug" "/mnt/c/Anas's Data/REPOS/PROGRAMMING/Programs C/CN_LABS/CN_LABS_6/cmake-build-debug/CMakeFiles/CN_LABS_6.dir/DependInfo.cmake" --color=$(COLOR)
.PHONY : CMakeFiles/CN_LABS_6.dir/depend

