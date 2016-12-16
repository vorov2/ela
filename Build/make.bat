@echo off
@echo Make release platform
SET "ELAPATH=..\"

@echo.
@echo 1. Build Elide
"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe" "%ELAPATH%elide\elide.sln" /build Release

@echo.
@echo 2. Build Ela
"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe" "%ELAPATH%ela\ela.sln" /build Release

@echo.
@echo 3. Prepare target directory
rmdir c:\ela-platform\ /S /Q
mkdir c:\ela-platform\
mkdir c:\ela-platform\ela\
mkdir c:\ela-platform\lib\
mkdir c:\ela-platform\elide
mkdir c:\ela-platform\docs\
mkdir c:\ela-platform\docs\samples\

@echo.
@echo 4. Copy Ela files
copy "%ELAPATH%ela\bin\ela.dll" c:\ela-platform\ela\
copy "%ELAPATH%ela\bin\elac.exe" c:\ela-platform\ela\
copy "%ELAPATH%ela\elaconsole\elac.exe.config" c:\ela-platform\ela\
copy "%ELAPATH%ela\bin\elalib.dll" c:\ela-platform\lib\
xcopy "%ELAPATH%ela\elalibrary\_ela\*.ela" c:\ela-platform\lib\ /E
xcopy "%ELAPATH%ela\bin\ela.dll" c:\projects\elaplatform\build\ela.dll /Y

@echo. 
@echo 5. Copy Elide files
copy "%ELAPATH%elide\bin\*.dll" c:\ela-platform\elide\
copy "%ELAPATH%elide\bin\elide.exe" c:\ela-platform\elide\
copy "%ELAPATH%elide\bin\elide.xml" c:\ela-platform\elide\

@echo. 
@echo 6. Generate documentation files
xcopy "%ELAPATH%documentation\_dir.xml" c:\ela-platform\docs\
eladoc %ELAPATH%documentation\ c:\ela-platform\docs\

@echo.
@echo 7. Copy code samples
xcopy "%ELAPATH%documentation\samples\_dir.xml" c:\ela-platform\docs\samples\
xcopy "%ELAPATH%documentation\samples\*.ela" c:\ela-platform\docs\samples\ /E

@echo.
@echo 8. Copy change log files
copy "%ELAPATH%ela\ela\changelist.txt" c:\ela-platform\ela_log.txt
copy "%ELAPATH%ela\elalibrary\libchangelist.txt" c:\ela-platform\lib_log.txt
copy "%ELAPATH%ela\elaconsole\consolechangelist.txt" c:\ela-platform\elac_log.txt
copy "%ELAPATH%elide\elide.meta\elidechangelist.txt" c:\ela-platform\elide_log.txt
copy "%ELAPATH%documentation\docschangelist.txt" c:\ela-platform\docs_log.txt

@echo.
@echo 9. Clean
del c:\ela-platform\elide\ela.dll /Q

@echo.
@echo 10. Build version info
set rn=2016.7
for /f "delims=" %%a in ('pver.exe c:\ela-platform\lib_log.txt') do @set lib_v=%%a 
for /f "delims=" %%a in ('pver.exe c:\ela-platform\ela_log.txt') do @set ela_v=%%a
for /f "delims=" %%a in ('pver.exe c:\ela-platform\elac_log.txt') do @set elac_v=%%a 
for /f "delims=" %%a in ('pver.exe c:\ela-platform\elide_log.txt') do @set elide_v=%%a 
for /f "delims=" %%a in ('pver.exe c:\ela-platform\docs_log.txt') do @set docs_v=%%a 

@echo.
@echo 11. Generate Readme.htm
@echo ^<html^>^<head^>^<title^>Ela Platform Readme^</title^>^</head^> >> c:\ela-platform\readme.htm
@echo ^<body^>^<h1^>Ela Platform Release %rn% ^</h1^>^<h3^>Contents of the directory:^</h3^> >> c:\ela-platform\readme.htm
@echo ^<table border="1"^> >> c:\ela-platform\readme.htm
@echo ^<tr^>^<th^>Directory^</th^>^<th^>Description^</th^>^<th^>Platforms^</th^>^<th^>Version^</th^>^</tr^> >> c:\ela-platform\readme.htm
@echo ^<tr^>^<td^>docs^</td^>^<td^>Documentation and code samples^</td^>^<td^>All platforms^</td^>^<td^>%docs_v% ^</td^>^</tr^> >> c:\ela-platform\readme.htm
@echo ^<tr^>^<td rowspan=2^>ela^</td^>^<td^>Ela (ela.dll)^</td^>^<td^>.NET 4.0+/Mono 2.8+^</td^>^<td^>%ela_v% ^</td^>^</tr^> >> c:\ela-platform\readme.htm
@echo ^<tr^>^<td^>Ela Interactive Console (elac.exe)^</td^>^<td^>.NET 4.0+/Mono 2.8+^</td^>^<td^>%elac_v% ^</td^>^</tr^> >> c:\ela-platform\readme.htm
@echo ^<tr^>^<td^>elide^</td^>^<td^>Ela Integrated Development Environment (elide.exe)^</td^>^<td^>.NET 4.0+ (Windows only)^</td^>^<td^>%elide_v% ^</td^>^</tr^> >> c:\ela-platform\readme.htm
@echo ^<tr^>^<td^>lib^</td^>^<td^>Ela standard library^</td^>^<td^>.NET 4.0+/Mono 2.8+^</td^>^<td^>%lib_v% ^</td^>^</tr^> >> c:\ela-platform\readme.htm
@echo ^</table^>^<p^>This is a binary release. Source code is available at ^<a href="https://ela.codeplex.com/"^>https://ela.codeplex.com/^</a^>.^</p^>^<p^>Platform generated at %DATE%.^</p^>^</body^>^</html^> >> c:\ela-platform\readme.htm

@echo.
@echo Make completed

pause