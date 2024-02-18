# Windows Sandbox Configuration Tool Documentation
## Introduction

The Windows Sandbox Configuration Tool is a graphical user interface (GUI) application designed to facilitate the generation of configuration files for Windows Sandbox. It provides an easy-to-use interface for specifying various settings such as folder mappings, networking, audio and video input, vGPU usage, logon commands, and execution of the sandbox configuration file.
Getting Started

    Launching the Application: Double-click the executable file wsb to launch the Windows Sandbox Configuration Tool. Or Just Launch it from your powershell terminal.

    User Interface Overview: The application interface consists of input fields, checkboxes, and buttons organized into sections for configuring sandbox settings, specifying logon commands, and executing sandbox configurations.

Configuration Settings
Folder Mapping

    Host Folder: Specify the path of the folder on the host system to be mapped into the sandbox.
    Sandbox Folder: Enter the destination path within the sandbox where the host folder will be mapped.
    Read Only: Check this box if you want the mapped folder to be read-only within the sandbox.

Sandbox Settings

    Networking Enabled: Check this box to enable networking within the sandbox.
    Audio Input Enabled: Check this box to enable audio input within the sandbox.
    Video Input Enabled: Check this box to enable video input within the sandbox.
    vGPU Enabled: Check this box to enable virtual GPU (vGPU) within the sandbox.

Logon Command

    Logon Command: Enter the command that should be executed upon logging into the sandbox.

Execution

    Generate Configuration: Click this button to generate the sandbox configuration file based on the specified settings.
    Path to .wsb file: Specify the path to the generated sandbox configuration file (.wsb).
    Arguments: Enter any arguments to be passed when executing the sandbox configuration file.
    Run MyConfigFile.wsb: Click this button to execute the generated sandbox configuration file with the specified arguments.

Usage

    Fill in the required information in the input fields and checkboxes according to your sandbox configuration requirements.
    Click the "Generate Configuration" button to create the sandbox configuration file.
    Specify the path to the generated configuration file (.wsb) in the "Path to .wsb file" field.
    Optionally, enter any arguments in the "Arguments" field.
    Click the "Run MyConfigFile.wsb" button to execute the sandbox configuration file with the specified settings.

Notes

    Make sure to review the generated sandbox configuration file before execution to ensure it reflects your desired settings.
    Exercise caution when executing sandbox configurations, especially if they involve network access or system resources.

Path: C:\Users\WDAGUtilityAccount\Desktop