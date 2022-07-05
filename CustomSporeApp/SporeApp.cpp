#include <iostream>
#include <filesystem>
#include <cstdlib>

#include <Windows.h>
#include <psapi.h>

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, PWSTR pCmdLine, int nCmdShow)
{
    std::wstring currentDirectory = std::filesystem::current_path().wstring();
    BOOL ret;
    STARTUPINFOW startupInfo = {0};
    PROCESS_INFORMATION processInfo = {0};
    DWORD processCreationFlags = 0;

    std::wstring commandLine = L"SporeAppOriginal.exe";
    commandLine += L" ";
    commandLine += pCmdLine;

    // if the arguments contain '-suspend',
    // launch Spore in a suspended state
    if (commandLine.find(L"-suspend") != std::string::npos)
    {
        processCreationFlags = CREATE_SUSPENDED;
    }

    SetDllDirectoryA("C:\\Windows\\WinSxS\\x86_microsoft.vc90.crt_1fc8b3b9a1e18e3b_9.0.30729.9625_none_508ef7e4bcbbe589");

    ret = CreateProcessW(nullptr, 
                const_cast<LPWSTR>(commandLine.c_str()),
                nullptr,
                nullptr,
                false,
                processCreationFlags,
                nullptr,
                nullptr,
                &startupInfo,
                &processInfo);

    if (ret != TRUE)
    {
        return 1;
    }

    WaitForSingleObject(processInfo.hProcess, INFINITE);

    CloseHandle(processInfo.hProcess);
    CloseHandle(processInfo.hThread);
    return 0;
}
