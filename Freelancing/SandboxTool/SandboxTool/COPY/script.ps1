Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# Function to generate the sandbox configuration
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

    # Check if any parameter is enabled
    if (-not ($readOnly -or $networkingEnabled -or $audioInputEnabled -or $videoInputEnabled -or $vGpuEnabled)) {
        [System.Windows.Forms.MessageBox]::Show("Please select at least one parameter.", "Error", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error)
        return
    }

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
</Configuration>
"@

    # Save the configuration XML to a file
    $configXml | Out-File -FilePath "SandboxConfig.wsb" -Encoding UTF8

    # Show confirmation message
    [System.Windows.Forms.MessageBox]::Show("Configuration successfully generated.", "Success", [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Information)
}

# Create a new form
$form = New-Object System.Windows.Forms.Form
$form.Text = "Sandbox Configuration Tool"
$form.Size = New-Object System.Drawing.Size(400, 400)
$form.StartPosition = "CenterScreen"

# Labels and text boxes
$labelHostFolder = New-Object System.Windows.Forms.Label
$labelHostFolder.Location = New-Object System.Drawing.Point(20, 20)
$labelHostFolder.Size = New-Object System.Drawing.Size(100, 20)
$labelHostFolder.Text = "Host Folder:"
$form.Controls.Add($labelHostFolder)

$textBoxHostFolder = New-Object System.Windows.Forms.TextBox
$textBoxHostFolder.Location = New-Object System.Drawing.Point(130, 20)
$textBoxHostFolder.Size = New-Object System.Drawing.Size(200, 20)
$form.Controls.Add($textBoxHostFolder)

$labelSandboxFolder = New-Object System.Windows.Forms.Label
$labelSandboxFolder.Location = New-Object System.Drawing.Point(20, 50)
$labelSandboxFolder.Size = New-Object System.Drawing.Size(100, 20)
$labelSandboxFolder.Text = "Sandbox Folder:"
$form.Controls.Add($labelSandboxFolder)

$textBoxSandboxFolder = New-Object System.Windows.Forms.TextBox
$textBoxSandboxFolder.Location = New-Object System.Drawing.Point(130, 50)
$textBoxSandboxFolder.Size = New-Object System.Drawing.Size(200, 20)
$form.Controls.Add($textBoxSandboxFolder)

# Checkboxes for parameters
$checkBoxReadOnly = New-Object System.Windows.Forms.CheckBox
$checkBoxReadOnly.Location = New-Object System.Drawing.Point(130, 80)
$checkBoxReadOnly.Size = New-Object System.Drawing.Size(150, 20)
$checkBoxReadOnly.Text = "Read Only"
$form.Controls.Add($checkBoxReadOnly)

$checkBoxNetworking = New-Object System.Windows.Forms.CheckBox
$checkBoxNetworking.Location = New-Object System.Drawing.Point(130, 110)
$checkBoxNetworking.Size = New-Object System.Drawing.Size(150, 20)
$checkBoxNetworking.Text = "Networking Enabled"
$form.Controls.Add($checkBoxNetworking)

$checkBoxAudioInput = New-Object System.Windows.Forms.CheckBox
$checkBoxAudioInput.Location = New-Object System.Drawing.Point(130, 140)
$checkBoxAudioInput.Size = New-Object System.Drawing.Size(150, 20)
$checkBoxAudioInput.Text = "Audio Input Enabled"
$form.Controls.Add($checkBoxAudioInput)

$checkBoxVideoInput = New-Object System.Windows.Forms.CheckBox
$checkBoxVideoInput.Location = New-Object System.Drawing.Point(130, 170)
$checkBoxVideoInput.Size = New-Object System.Drawing.Size(150, 20)
$checkBoxVideoInput.Text = "Video Input Enabled"
$form.Controls.Add($checkBoxVideoInput)

$checkBoxVGpu = New-Object System.Windows.Forms.CheckBox
$checkBoxVGpu.Location = New-Object System.Drawing.Point(130, 200)
$checkBoxVGpu.Size = New-Object System.Drawing.Size(150, 20)
$checkBoxVGpu.Text = "vGPU Enabled"
$form.Controls.Add($checkBoxVGpu)

# Button to generate configuration
$buttonGenerateConfig = New-Object System.Windows.Forms.Button
$buttonGenerateConfig.Location = New-Object System.Drawing.Point(130, 250)
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
    # Call function to generate configuration
    Generate-SandboxConfig -hostFolder $hostFolder -sandboxFolder $sandboxFolder -readOnly $readOnly -networkingEnabled $networkingEnabled -audioInputEnabled $audioInputEnabled -videoInputEnabled $videoInputEnabled -vGpuEnabled $vGpuEnabled
})
$form.Controls.Add($buttonGenerateConfig)

# Show the form
$form.ShowDialog() | Out-Null
