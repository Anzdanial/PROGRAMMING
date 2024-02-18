Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

#C:\Users\WDAGUtilityAccount\Desktop

# Function to generate the configuration file
function Generate-SandboxConfig {
    param(
        [string]$hostFolder,
        [string]$sandboxFolder,
        [bool]$readOnly,
        [bool]$networkingEnabled,
        [bool]$audioInputEnabled,
        [bool]$videoInputEnabled,
        [bool]$vGpuEnabled,
        [string]$logonCommand
    )

    $configXml = @"
<Configuration>
    <MappedFolders>
        <MappedFolder>
            <HostFolder>$hostFolder</HostFolder>
            <SandboxFolder>$sandboxFolder</SandboxFolder>
            <ReadOnly>$readOnly</ReadOnly>
        </MappedFolder>
    </MappedFolders>
    <Networking>$networkingEnabled</Networking>
    <AudioInput>$audioInputEnabled</AudioInput>
    <VideoInput>$videoInputEnabled</VideoInput>
    <VGpu>$vGpuEnabled</VGpu>
    <LogonCommand>$logonCommand</LogonCommand>
</Configuration>
"@

    # Save the configuration XML to a file
    $configXml | Out-File -FilePath "MyConfigFile.wsb" -Encoding UTF8
}

# Create a new form
$form = New-Object System.Windows.Forms.Form
$form.Text = "Windows Sandbox Configuration"
$form.Size = New-Object System.Drawing.Size(400, 500)
$form.StartPosition = "CenterScreen"

# Create labels and text boxes
$textBoxHostFolder = New-Object System.Windows.Forms.TextBox
$textBoxHostFolder.Location = New-Object System.Drawing.Point(120, 20)
$textBoxHostFolder.Size = New-Object System.Drawing.Size(250, 20)
$form.Controls.Add($textBoxHostFolder)

$textBoxSandboxFolder = New-Object System.Windows.Forms.TextBox
$textBoxSandboxFolder.Location = New-Object System.Drawing.Point(120, 50)
$textBoxSandboxFolder.Size = New-Object System.Drawing.Size(250, 20)
$form.Controls.Add($textBoxSandboxFolder)

$checkBoxReadOnly = New-Object System.Windows.Forms.CheckBox
$checkBoxReadOnly.Location = New-Object System.Drawing.Point(120, 80)
$checkBoxReadOnly.Size = New-Object System.Drawing.Size(200, 20)
$checkBoxReadOnly.Text = "Read Only"
$form.Controls.Add($checkBoxReadOnly)

$checkBoxNetworking = New-Object System.Windows.Forms.CheckBox
$checkBoxNetworking.Location = New-Object System.Drawing.Point(120, 110)
$checkBoxNetworking.Size = New-Object System.Drawing.Size(200, 20)
$checkBoxNetworking.Text = "Networking Enabled"
$form.Controls.Add($checkBoxNetworking)

$checkBoxAudioInput = New-Object System.Windows.Forms.CheckBox
$checkBoxAudioInput.Location = New-Object System.Drawing.Point(120, 140)
$checkBoxAudioInput.Size = New-Object System.Drawing.Size(200, 20)
$checkBoxAudioInput.Text = "Audio Input Enabled"
$form.Controls.Add($checkBoxAudioInput)

$checkBoxVideoInput = New-Object System.Windows.Forms.CheckBox
$checkBoxVideoInput.Location = New-Object System.Drawing.Point(120, 170)
$checkBoxVideoInput.Size = New-Object System.Drawing.Size(200, 20)
$checkBoxVideoInput.Text = "Video Input Enabled"
$form.Controls.Add($checkBoxVideoInput)

$checkBoxVGpu = New-Object System.Windows.Forms.CheckBox
$checkBoxVGpu.Location = New-Object System.Drawing.Point(120, 200)
$checkBoxVGpu.Size = New-Object System.Drawing.Size(200, 20)
$checkBoxVGpu.Text = "vGPU Enabled"
$form.Controls.Add($checkBoxVGpu)

# Create a label and text box for logon command
$labelLogonCommand = New-Object System.Windows.Forms.Label
$labelLogonCommand.Location = New-Object System.Drawing.Point(10, 230)
$labelLogonCommand.Size = New-Object System.Drawing.Size(100, 20)
$labelLogonCommand.Text = "Logon Command:"
$form.Controls.Add($labelLogonCommand)

$textBoxLogonCommand = New-Object System.Windows.Forms.TextBox
$textBoxLogonCommand.Location = New-Object System.Drawing.Point(120, 230)
$textBoxLogonCommand.Size = New-Object System.Drawing.Size(250, 20)
$form.Controls.Add($textBoxLogonCommand)

# Create a button to generate the configuration
$buttonGenerateConfig = New-Object System.Windows.Forms.Button
$buttonGenerateConfig.Location = New-Object System.Drawing.Point(120, 260)
$buttonGenerateConfig.Size = New-Object System.Drawing.Size(150, 30)
$buttonGenerateConfig.Text = "Generate Configuration"
$buttonGenerateConfig.Add_Click({
    # Get input values
    $hostFolder = $textBoxHostFolder.Text
    $sandboxFolder = $textBoxSandboxFolder.Text
    $readOnly = $checkBoxReadOnly.Checked
    $networkingEnabled = $checkBoxNetworking.Checked
    $audioInputEnabled = $checkBoxAudioInput.Checked
    $videoInputEnabled = $checkBoxVideoInput.Checked
    $vGpuEnabled = $checkBoxVGpu.Checked
    $logonCommand = $textBoxLogonCommand.Text
    # Call the function to generate the configuration with the specified parameters
    Generate-SandboxConfig -hostFolder $hostFolder -sandboxFolder $sandboxFolder -readOnly $readOnly -networkingEnabled $networkingEnabled -audioInputEnabled $audioInputEnabled -videoInputEnabled $videoInputEnabled -vGpuEnabled $vGpuEnabled -logonCommand $logonCommand
    # Optionally, you can add code here to run the sandbox with the generated configuration
})
$form.Controls.Add($buttonGenerateConfig)

# Create labels for text boxes
$labelHostFolder = New-Object System.Windows.Forms.Label
$labelHostFolder.Location = New-Object System.Drawing.Point(10, 20)
$labelHostFolder.Size = New-Object System.Drawing.Size(100, 20)
$labelHostFolder.Text = "Host Folder:"
$form.Controls.Add($labelHostFolder)

$labelSandboxFolder = New-Object System.Windows.Forms.Label
$labelSandboxFolder.Location = New-Object System.Drawing.Point(10, 50)
$labelSandboxFolder.Size = New-Object System.Drawing.Size(100, 20)
$labelSandboxFolder.Text = "Sandbox Folder:"
$form.Controls.Add($labelSandboxFolder)

# Create labels for running MyConfigFile.wsb directly
$labelRunPath = New-Object System.Windows.Forms.Label
$labelRunPath.Location = New-Object System.Drawing.Point(10, 300)
$labelRunPath.Size = New-Object System.Drawing.Size(100, 20)
$labelRunPath.Text = "Path to .wsb file:"
$form.Controls.Add($labelRunPath)

$textBoxRunPath = New-Object System.Windows.Forms.TextBox
$textBoxRunPath.Location = New-Object System.Drawing.Point(120, 300)
$textBoxRunPath.Size = New-Object System.Drawing.Size(250, 20)
$form.Controls.Add($textBoxRunPath)

$labelArguments = New-Object System.Windows.Forms.Label
$labelArguments.Location = New-Object System.Drawing.Point(10, 330)
$labelArguments.Size = New-Object System.Drawing.Size(100, 20)
$labelArguments.Text = "Arguments:"
$form.Controls.Add($labelArguments)

$textBoxArguments = New-Object System.Windows.Forms.TextBox
$textBoxArguments.Location = New-Object System.Drawing.Point(120, 330)
$textBoxArguments.Size = New-Object System.Drawing.Size(250, 20)
$form.Controls.Add($textBoxArguments)

# Add button to run MyConfigFile.wsb
$buttonRunConfig = New-Object System.Windows.Forms.Button
$buttonRunConfig.Location = New-Object System.Drawing.Point(120, 360)
$buttonRunConfig.Size = New-Object System.Drawing.Size(150, 30)
$buttonRunConfig.Text = "Run MyConfigFile.wsb"
$form.Controls.Add($buttonRunConfig)

# Add event handler for running MyConfigFile.wsb
$buttonRunConfig.Add_Click({
    # Get input values
    $runPath = $textBoxRunPath.Text
    $arguments = $textBoxArguments.Text
    # If argument field is empty, add default values
    if ([string]::IsNullOrEmpty($arguments)) {
        $arguments = "DefaultArguments"  # Replace "DefaultArguments" with actual default arguments
    }
    # Run MyConfigFile.wsb
    Start-Process -FilePath $runPath -ArgumentList $arguments
})

# Show the form
$form.ShowDialog() | Out-Null

