@ECHO OFF

dotnet build
IF ERRORLEVEL 1 EXIT 1


type "test-data\test-clean.xml" | "bin\Debug\net6.0\Febe.ConfigFilter.exe" clean
