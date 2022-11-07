Get-ChildItem .\Raw -Filter *.html | ForEach-Object {
    wkhtmltoimage.exe $_.FullName ".\Rendered\$($_.Name).png"
}