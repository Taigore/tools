@ECHO OFF

dotnet build ^
	--configuration Release ^
	--no-incremental ^
	--output C:\Tools\config-filter
