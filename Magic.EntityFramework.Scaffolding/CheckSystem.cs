using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.EntityFramework.Scaffolding
{
    public static class CheckSystem
    {


        public static bool PerformChecks()
        {
            Console.WriteLine("Checking if .NET Core SDK is installed...");
            var dotnetExe = GetDotnetExePath();
            if (dotnetExe == null)
            {
                Console.WriteLine("Error: .NET Core SDK is not installed or not in the system PATH.");
                Console.WriteLine("Please download and install the latest version of .NET Core SDK before continuing.");
                return false;
            }
            Console.WriteLine("OK: .NET Core SDK is installed.");

            Console.WriteLine("Checking if 'dotnet' tool commands are available...");
            var dotnetResult = RunCommand(dotnetExe, "--version");
            if (dotnetResult.ExitCode != 0)
            {
                Console.WriteLine("Error: 'dotnet' tool commands are not available.");
                return false;
            }
            Console.WriteLine($"OK: 'dotnet' tool commands are available (version {dotnetResult.Output.Trim()}).");

            Console.WriteLine("Checking if 'dotnet-ef' tool commands are available...");
            var dotnetEfResult = RunCommand(dotnetExe, "tool list --global");
            if (dotnetEfResult.ExitCode != 0)
            {
                Console.WriteLine("Error: failed to run 'dotnet tool list --global'.");
                return false;
            }
            if (!dotnetEfResult.Output.Contains("dotnet-ef"))
            {
                Console.WriteLine("Installing 'dotnet-ef' tool...");
                var dotnetEfInstallResult = RunCommand(dotnetExe, "tool install --global dotnet-ef");
                if (dotnetEfInstallResult.ExitCode != 0)
                {
                    Console.WriteLine("Error: failed to install 'dotnet-ef' tool.");
                    return false;
                }
                Console.WriteLine("OK: 'dotnet-ef' tool installed.");
            }
            else
            {
                Console.WriteLine("OK: 'dotnet-ef' tool is already installed.");
            }

            Console.WriteLine("Verifying 'dotnet-ef' tool commands are available...");

            var dotnetEfResultVerify = RunCommand(dotnetExe, "tool list --global");
            if (dotnetEfResultVerify.ExitCode == 0 && dotnetEfResult.Output.Contains("dotnet-ef"))
            {
                Console.WriteLine($"OK: 'dotnet-ef' tool commands are available (version {dotnetEfResultVerify.Output.Trim()}).");
                return true;
            }
            else
            {
                Console.WriteLine("Error: failed to recognize the install of dotnet-ef.");
                return false;
            }
        }

        private static string GetDotnetExePath()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "--version",
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };
            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                return null;
            }
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                return null;
            }
            return processStartInfo.FileName;
        }

        private static (int ExitCode, string Output) RunCommand(string fileName, string arguments)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            };
            using var process = Process.Start(processStartInfo);
            if (process == null)
            {
                return (-1, null);
            }
            process.WaitForExit();
            return (process.ExitCode, process.StandardOutput.ReadToEnd());
        }

    }
}
