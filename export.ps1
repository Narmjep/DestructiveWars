# Set the source folder to the current directory
$source = Get-Location

# Set the destination folder to the desired directory
$destination = "E:\Program Files (x86)\Steam\steamapps\common\worldbox\Mods"

# Set the filename of the ZIP archive to be created
$filename = "DestructiveWars.zip"

# Set the list of files to include in the zip archive
$include = @("Code" , "fire.png" , "mod.json")


# Create the ZIP archive from the temporary folder
$OutputPath = Join-Path $destination $filename
Compress-Archive -Path $include -DestinationPath $OutputPath -CompressionLevel Optimal -Force


# Print a message to indicate that the operation is complete
Write-Host "ZIP archive created: $OutputPath"
