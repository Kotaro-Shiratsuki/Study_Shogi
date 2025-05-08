@echo off
REM -----------------------------------------------------------------
REM Script of delete unnecessary files and folders on unity project.
REM -----------------------------------------------------------------

echo.
echo Delete unnecessary files on unity project...
echo.

REM --- Delete folders ---
for %%D in (
    "Library"
    "Logs"
    "obj"
    "Temp"
    ".vs"
    ".vscode"
) do (
    if exist %%D (
        echo Deleting：%%D
        rd /s /q %%D
    )
)

REM --- Delete files (include subdirectory) ---
echo.
echo Start deleting files related to Visual Studio...
echo.

REM Delete *.vsconfig
for /R %%F in (*.vsconfig) do (
    echo Deleting：%%F
    del /F /Q "%%F"
)
REM Delete *.csproj
for /R %%F in (*.csproj) do (
    echo Deleting：%%F
    del /F /Q "%%F"
)
REM Delete *.sln
for /R %%F in (*.sln) do (
    echo Deleting：%%F
    del /F /Q "%%F"
)
REM Delete *.suo
for /R %%F in (*.suo) do (
    echo Deleting：%%F
    del /F /Q "%%F"
)
REM Delete *.pdb
for /R %%F in (*.pdb) do (
    echo Deleting：%%F
    del /F /Q "%%F"
)
REM Delete *.user
for /R %%F in (*.user) do (
    echo Deleting：%%F
    del /F /Q "%%F"
)

echo.
echo Delete is completed.
pause
