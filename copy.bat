@cd "C:\Users\lomzt\Documents\GitHub\RimWorld-Minimods"
@set name=%1

@echo Copying mod %name% to RimWorld mods

@set dest=%2%
@set dest=%dest:"=%

@echo %name%
@echo %dest%

@robocopy "./%1" "%dest%/%name%" /xd "obj" "bin" "Source" /xf "*.csproj" /mir

@exit /B