@echo off
SET "ELAPATH=..\"

IF EXIST c:\ela-platform\ela\ (
@echo Copy Ela files
copy "%ELAPATH%ela\bin\ela.dll" c:\ela-platform\ela\
copy "%ELAPATH%ela\bin\elac.exe" c:\ela-platform\ela\
copy "%ELAPATH%ela\elaconsole\elac.exe.config" c:\ela-platform\ela\
) ELSE (
@echo Directory c:\ela-platform\ela not found
@echo Skipping: Copy Ela files
)

@echo Done
pause