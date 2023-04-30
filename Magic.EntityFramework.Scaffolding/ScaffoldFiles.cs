using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Magic.EntityFramework.Scaffolding
{
    public class ScaffoldFiles
    {
        public static string ScaffoldEnvironment(string dbContextPath, string dbModelsPath, out string dbModelsContent, string dbContextContentOverride = null)
        {
            // Read the contents of SayouDbContext.cs file
            string dbContextContent = null;
            if (dbContextContentOverride == null)
            {
                if (dbContextPath.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    dbContextContent = TemplateContextHelper.GetTemplate();
                }
                else
                {
                    dbContextContent = File.ReadAllText(dbContextPath);
                }
            }
            else
                dbContextContent = new string(dbContextContentOverride);

            // Remove the ScaffoldingCode region
            dbContextContent = RemoveScaffoldingCode(dbContextContent);

            // Remove the model building region
            dbContextContent = RemoveBuildEntityCode(dbContextContent);


            dbContextContent = RemoveEnvironmentEnumCode(dbContextContent);

            // Read the contents of SayouDevelopmentContext.cs file
            dbModelsContent = File.ReadAllText(dbModelsPath);

            return dbContextContent;
        }

        public static string AddOrUpdateContentInRegion(string content, string regionName, string newContent)
        {
            Regex regionPattern = new Regex($@"\#region\s+{regionName}.*?\#endregion", RegexOptions.Singleline);
            Match regionMatch = regionPattern.Match(content);
            if (regionMatch.Success)
            {
                string regionContent = regionMatch.Value;
                string newRegionContent = $"#region {regionName}\n{newContent}\n#endregion";
                content = content.Replace(regionContent, newRegionContent);
            }
            else
            {
                string newRegionContent = $"#region {regionName}\n{newContent}\n#endregion";
                content += $"\n\n{newRegionContent}";
            }
            return content;
        }

        public static string CreateEnumFromPropertyNames(IEnumerable<string> propertyNames, string enumName)
        {
            Settings _settings = SettingsHelper.GetSettings();
            string DevEnvironmentString = $"DbEnvironment.{propertyNames.Where(x => x.Equals(_settings.NameOfDevEnvDb, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()};";

            StringBuilder enumBuilder = new StringBuilder();
            enumBuilder.AppendLine($"public enum {enumName}");
            enumBuilder.AppendLine("{");
            foreach (string propertyName in propertyNames)
            {
                enumBuilder.AppendLine($"    {propertyName},");
            }
            enumBuilder.AppendLine("}");
            enumBuilder.AppendLine("private DbEnvironment _environment { get; set; }  = " + DevEnvironmentString);


            enumBuilder.AppendLine("private DbEnvironment _devEnvironment { get; set; } = " + DevEnvironmentString);


            return enumBuilder.ToString();
        }

        public static string BuildModelBuildingSwitch(IEnumerable<ScaffoldFile> scaffoldFiles)
        {
            string switchStatement = @"switch (_environment)" + Environment.NewLine + "{" + Environment.NewLine;
            foreach (var file in scaffoldFiles)
            {
                string environmentName = new CliMenu().GetFileNameWithoutExtension(file.FilePath);
                if (!String.IsNullOrWhiteSpace(environmentName))
                {
                    switchStatement += string.Format("case DbEnvironment.{0}:\n{1}\nbreak;\n\n", environmentName, file.ModelBuildingContent);
                }
                else
                {
                    switchStatement += string.Format("// Error: Failed to parse environment name from: ", file.FilePath);
                }
            }

            return switchStatement + Environment.NewLine + "}";
        }

        private string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        private enum DbEnvironment
        {
            Sayou_Development,
            Sayou_Production,
        }




        public static string GetModelBuildingContent(string fileContent)
        {
            string startTag = "OnModelCreating(ModelBuilder modelBuilder)\r\n    {\r\n";
            string endTag = "\r\n\r\n        OnModelCreatingPartial(modelBuilder);\r\n    }";
            int startIndex = fileContent.IndexOf(startTag) + startTag.Length;
            int endIndex = fileContent.IndexOf(endTag, startIndex);
            return fileContent.Substring(startIndex, endIndex - startIndex);
        }

        static string RemoveScaffoldingCode(string content)
        {
            Regex regionPattern = new Regex(@"\#region\s+ScaffoldingCode.*?\#endregion", RegexOptions.Singleline);
            return regionPattern.Replace(content, "#region ScaffoldingCode\n#endregion");
        }

        static string RemoveBuildEntityCode(string content)
        {
            Regex regionPattern = new Regex(@"\#region\s+ModelBuilding.*?\#endregion", RegexOptions.Singleline);
            return regionPattern.Replace(content, "#region ModelBuilding\n#endregion");
        }

        static string RemoveEnvironmentEnumCode(string content)
        {
            Regex regionPattern = new Regex(@"\#region\s+EnvironmentEnum.*?\#endregion", RegexOptions.Singleline);
            return regionPattern.Replace(content, "#region EnvironmentEnum\n#endregion");
        }

        public static string ExtractDbSetRegion(string content)
        {
            Regex dbSetPattern = new Regex(@"public virtual\s+DbSet<.*?>\s+[A-Za-z0-9_]+\s*\{.*?\}", RegexOptions.Singleline);
            MatchCollection matches = dbSetPattern.Matches(content);
            if (matches.Count == 0)
            {
                throw new Exception("No DbSet found in the given content.");
            }
            int firstIndex = matches[0].Index;
            int lastIndex = matches[matches.Count - 1].Index + matches[matches.Count - 1].Length;
            return content.Substring(firstIndex, lastIndex - firstIndex);
        }

        public static string InsertDbSetRegion(string dbSetRegion, string dbContextContent)
        {
            Regex regionPattern = new Regex(@"\#region\s+ScaffoldingCode.*?\#endregion", RegexOptions.Singleline);
            return regionPattern.Replace(dbContextContent, $"#region ScaffoldingCode\n{dbSetRegion}\n#endregion");
        }

        public async Task<string> RunCommandInPmcAsync(string command, string workingDirectory)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"ef {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            if (workingDirectory != null)
            {
                psi.WorkingDirectory = workingDirectory;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Scaffolding starting");
            Console.ForegroundColor = ConsoleColor.White;

            var process = Process.Start(psi);
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
            {
                throw new Exception($"Command failed with exit code {process.ExitCode}. Error output:{Environment.NewLine}{error}");
            }
            return output;
        }

        public string GetFolderName(string directoryPath)
        {
            return new DirectoryInfo(directoryPath).Name;
        }

        public string RunCommandInPmc(string command, string workingDirectory)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"ef {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            if (workingDirectory != null)
            {
                psi.WorkingDirectory = workingDirectory;
            }
            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return error + " " + output;
        }

        public string RunCommandInPmc(string command)
        {
            var _settings = SettingsHelper.GetSettings();
            string namespaceWord = null;
            if (_settings.WorkingDirectory.Equals("default", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Generating default template");
                Console.ForegroundColor = ConsoleColor.White;
                namespaceWord = "TemplateNamespace";
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Using existing template");
                Console.ForegroundColor = ConsoleColor.White;
                namespaceWord = _settings.WorkingDirectory;
            }
            //string namespaceWord = new CliMenu().GetSecondWord(_settings.WorkingDirectory);
            // Create a temporary directory

            string tempDirectory1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tempProjDir");
            string tempDirectory = Path.Combine(tempDirectory1, namespaceWord);
            string tempDirectoryProjFile = Path.Combine(tempDirectory, namespaceWord + ".csproj");

            //string tempDirectory = Path.Combine(Path.GetTempPath(), namespaceWord);
            Directory.CreateDirectory(tempDirectory);

            if (!File.Exists(tempDirectoryProjFile))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Creating temporary project");
                Console.ForegroundColor = ConsoleColor.White;
                // Create a temporary .NET Core project in the temporary directory
                var createProjectProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "new console",
                        WorkingDirectory = tempDirectory,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                createProjectProcess.Start();
                createProjectProcess.WaitForExit();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Installing required packages");
                Console.ForegroundColor = ConsoleColor.White;

                // Add necessary NuGet packages to the temporary project
                string[] requiredPackages = new[]
                {
        "Microsoft.EntityFrameworkCore.Design",
        "Microsoft.EntityFrameworkCore.SqlServer"
    };

                foreach (var package in requiredPackages)
                {
                    var addPackageProcess = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "dotnet",
                            Arguments = $"add package {package}",
                            WorkingDirectory = tempDirectory,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };
                    addPackageProcess.Start();
                    addPackageProcess.WaitForExit();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Temp project already exists, skipping temp propject creation");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Scaffolding starting");
            Console.ForegroundColor = ConsoleColor.White;

            // Run the scaffold command in the temporary project
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"ef {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = tempDirectory
            };

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Running scaffold package manager now");
            Console.ForegroundColor = ConsoleColor.White;
            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Package manager scaffolding complete");
            Console.ForegroundColor = ConsoleColor.White;
            

            return error + " " + output;
        }
    }
}
