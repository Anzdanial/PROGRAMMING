# Sandbox Configurator Documentation Overview

## Introduction

The Sandbox Configurator is a user-friendly graphical interface designed to simplify the creation of configuration files for Windows Sandbox. This tool streamlines the process of defining various parameters, including folder mappings, networking preferences, audio and video input options, vGPU utilization, and execution directives for sandbox configurations.


## Getting Started

### Launching the Application

To initiate the Sandbox Configurator, either execute the 'wsb' executable file or launch it directly from your PowerShell terminal.


### User Interface Overview

The application interface is intuitively structured, featuring input fields, toggle switches, and action buttons. These elements are logically organized into sections for configuring sandbox settings, specifying logon commands, and executing sandbox configurations.

  
## Configuration Settings

### Folder Mapping

Host Folder: Indicate the directory path on the host system to be integrated into the sandbox environment.

Sandbox Folder: Define the destination path within the sandbox where the host folder will be mapped.

Note: Use C:\Users\WDAGUtilityAccount\Desktop\TestingFolder

Read Only: Enable this setting to designate the mapped folder as read-only within the sandbox environment.

### Sandbox Settings

Networking Enabled: Activate to allow network connectivity within the sandbox.

Audio Input Enabled: Enable to facilitate audio input functionalities within the sandbox environment.

Video Input Enabled: Toggle to grant access to video input capabilities within the sandbox.

vGPU Enabled: Activate to utilize virtual GPU (vGPU) resources within the sandbox environment.

### Execution

Generate Configuration: Initiate the creation of the sandbox configuration file based on the specified settings.

Path to .wsb file: Specify the file path to the generated sandbox configuration file (.wsb).

Arguments: Enter any supplementary arguments required for executing the sandbox configuration file.

Run SandboxConfig.wsb: Execute the generated sandbox configuration file with the specified settings.

### Usage

Provide the necessary details in the input fields and toggle switches based on your sandbox configuration preferences.

Click the "Generate Configuration" button to generate the sandbox configuration file.

Input the path to the generated configuration file (.wsb) in the designated field.

Optionally, include any relevant arguments in the provided field.

Click the "Run SandboxConfig.wsb" button to execute the sandbox configuration file with the defined settings.

### Notes

Thoroughly review the generated sandbox configuration file before execution to verify alignment with your intended settings.

Exercise caution when executing sandbox configurations, particularly those involving network access or system resource allocation.

