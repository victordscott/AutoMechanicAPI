# PowerShell script to add TimeZone using statement to AutoMechanicDbContextGenerated.cs
# This script adds the using statement as the first line in the file

$filePath = "Context\AutoMechanicDbContextGenerated.cs"
$usingStatement = "using TimeZone = AutoMechanic.DataAccess.EF.Models.TimeZone;"

# Check if the file exists
if (Test-Path $filePath) {
    # Read the current content of the file
    $content = Get-Content $filePath -Raw
    
    # Check if the using statement already exists
    if ($content -notmatch [regex]::Escape($usingStatement)) {
        # Add the using statement as the first line
        $newContent = $usingStatement + [Environment]::NewLine + $content
        
        # Write the modified content back to the file
        Set-Content -Path $filePath -Value $newContent -NoNewline
        
        Write-Host "Successfully added using statement to $filePath" -ForegroundColor Green
    } else {
        Write-Host "Using statement already exists in $filePath" -ForegroundColor Yellow
    }
} else {
    Write-Host "File not found: $filePath" -ForegroundColor Red
}