#include <iostream>
#include <filesystem>
#include <cstdlib>

#include <Windows.h>
#include <psapi.h>

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, PWSTR pCmdLine, int nCmdShow)
{
    BOOL ret;
    STARTUPINFOW startupInfo = {0};
    PROCESS_INFORMATION processInfo = {0};
    DWORD processCreationFlags = 0;
    std::string errorMessage;

    std::wstring commandLine = L"SporeAppOriginal.exe";
    commandLine += L" ";
    commandLine += pCmdLine;

    // if the arguments contain '-suspend',
    // launch Spore in a suspended state
    if (commandLine.find(L"-suspend") != std::string::npos)
    {
        processCreationFlags = CREATE_SUSPENDED;
    }

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
        errorMessage = "Failed to create Spore process: ";
        errorMessage += std::to_string(GetLastError());

        MessageBoxA(nullptr, errorMessage.c_str(), "CustomSporeApp", MB_OK);
        return 1;
    }

    WaitForSingleObject(processInfo.hProcess, INFINITE);

    CloseHandle(processInfo.hProcess);
    CloseHandle(processInfo.hThread);
    return 0;
}
