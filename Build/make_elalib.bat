@echo off
SET "ELAPATH=..\"

IF EXIST c:\ela-platform\ela\ (
@echo Copy Ela Library files
xcopy "%ELAPATH%ela\bin\elalib.dll" c:\ela-platform\lib\ /E /Y
xcopy "%ELAPATH%ela\elalibrary\_ela\*.ela" c:\ela-platform\lib\ /E /Y
) ELSE (
@echo Directory c:\ela-platform\lib not found
@echo Skipping: Copy Ela Library files
)

@echo Done