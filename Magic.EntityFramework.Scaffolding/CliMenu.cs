using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security.Principal;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Magic.EntityFramework.Scaffolding
{
    public class CliMenu
    {
        public void MainMenu()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Main Menu");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Type the number associated with what you would like to do:");
            Console.WriteLine("1.) View/Edit Connection Strings/Environments");
            Console.WriteLine("2.) Scaffold Environments");
            Console.WriteLine("3.) Settings");// Set the dotnet package location
            Console.WriteLine(Environment.NewLine);
            string UserResponse = Console.ReadLine();

            if (Int32.TryParse(UserResponse, out int _userIntResponse))
            {
                if (_userIntResponse == 1)
                {
                    ConnectionsMenu();
                }
                else if (_userIntResponse == 2)
                {
                    RunScaffolding();
                }
                else if (_userIntResponse == 3)
                {
                    Settings();
                }
                else
                {
                    Console.WriteLine("You must type a numerical value within the option range");
                }

            }
            else
            {
                Console.WriteLine("You must type a numerical value");
            }
        }

        public void Settings()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Settings");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Type the number associated with what you would like to do:");
            Console.WriteLine("1.) Go back to main menu");
            Console.WriteLine("2.) Show setting values");
            Console.WriteLine("3.) Set file path of dbContext file or type 'default' to use the template");
            Console.WriteLine("4.) Set namespace of your working directory or type 'default' to generate a generic template");
            Console.WriteLine("5.) Designate final scaffold path for all models");
            Console.WriteLine("6.) Set path to create folders for Extension classes and MetaData classes");
            Console.WriteLine("7.) Set dbContext name");
            //Console.WriteLine("5.) Designate final scaffold path for ");
            Console.WriteLine(Environment.NewLine);
            string UserResponse = Console.ReadLine();

            if (Int32.TryParse(UserResponse, out int _userIntResponse))
            {
                if (_userIntResponse == 1)
                {
                    MainMenu();
                }
                else if (_userIntResponse == 2)
                {
                    Console.WriteLine(Environment.NewLine);
                    // Serialize the object to JSON
                    var _settings = SettingsHelper.GetSettings();
                    if (_settings != null)
                    {
                        string json = JsonSerializer.Serialize(_settings);

                        // Parse the JSON string into a JsonDocument
                        JsonDocument doc = JsonDocument.Parse(json);

                        // Iterate over the object's properties and print them to the console
                        foreach (JsonProperty property in doc.RootElement.EnumerateObject())
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(property.Name);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine(": " + property.Value.ToString());

                        }
                    }
                    else
                    {
                        Console.WriteLine("There are no settings set");
                    }
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Type anything to go back to the settings menu:");
                    Console.ReadLine();
                    Settings();
                }
                else if (_userIntResponse == 3)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(@"'default' will use a template to combine your environments safely into a context cs file for you. Type 'cancel' to cancel. Or you can type a target file related to your project after setting up the required regions so that it'll build into your existing project for you.");
                    Console.WriteLine(Environment.NewLine);
                    string dbContextPathResponse = Console.ReadLine();

                    if (dbContextPathResponse.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                    {
                        Settings();
                    }
                    else if (String.IsNullOrEmpty(dbContextPathResponse))
                    {
                        Console.WriteLine("Invalid path");
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                    if (dbContextPathResponse.Trim().Equals("default", StringComparison.OrdinalIgnoreCase) && !File.Exists(dbContextPathResponse))
                    {
                        var _settings = SettingsHelper.GetSettings();
                        _settings.MainDbContextPath = dbContextPathResponse.Trim();
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                    else
                    {
                        var _settings = SettingsHelper.GetSettings();
                        _settings.MainDbContextPath = dbContextPathResponse.Replace(" ", "");
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                }
                else if (_userIntResponse == 4)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(@"Write the namespace of the working directory, write 'default' followed by the namespace you would like to choose,  or type 'cancel'.");
                    Console.WriteLine(Environment.NewLine);
                    string dbContextPathResponse = Console.ReadLine();
                    if (@String.IsNullOrEmpty(dbContextPathResponse) &&
                        (!dbContextPathResponse.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                        )
                    {
                        Settings();
                    }
                    else if (String.IsNullOrWhiteSpace(dbContextPathResponse))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Invalid input");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                    else if (dbContextPathResponse.Trim().StartsWith("default"))
                    {
                        string namespaceWord = "default";

                        var _settings = SettingsHelper.GetSettings();
                        _settings.WorkingDirectory = "default" + " " + namespaceWord;
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                    else
                    {
                        var _settings = SettingsHelper.GetSettings();
                        _settings.WorkingDirectory = dbContextPathResponse.Trim();
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                }
                else if (_userIntResponse == 5)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(@"Write the folder path of where you would like to place all the models created. Note this will delete all items in the folder in the end except for the newly generated files! Or you can type, 'default' and the models will be placed in this projects folder.");
                    Console.WriteLine(Environment.NewLine);
                    string dbContextPathResponse = Console.ReadLine();
                    if (@String.IsNullOrEmpty(dbContextPathResponse) &&
                        (!dbContextPathResponse.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                        )
                    {
                        Settings();
                    }
                    else if (dbContextPathResponse.Trim().StartsWith("default"))
                    {
                        var _settings = SettingsHelper.GetSettings();
                        _settings.ScaffoldModelsPath = "default";
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                    else
                    {
                        var _settings = SettingsHelper.GetSettings();
                        _settings.ScaffoldModelsPath = dbContextPathResponse;
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                }
                else if (_userIntResponse == 6)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(@"Type in the path where 2 folders will be created to put all the extensions and metadata classes that connect to the db models created during the scaffold process");
                    Console.WriteLine(Environment.NewLine);
                    string dbContextPathResponse = Console.ReadLine();
                    if (@String.IsNullOrEmpty(dbContextPathResponse) &&
                        (!dbContextPathResponse.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                        )
                    {
                        Settings();
                    }
                    else
                    {
                        var _settings = SettingsHelper.GetSettings();
                        _settings.PathToPlaceExtensionsAndMetaData = dbContextPathResponse;
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                }
                else if (_userIntResponse == 7)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(@"Type the name you would like to call your dbContext or type 'cancel'");
                    Console.WriteLine(Environment.NewLine);
                    string dbContextPathResponse = Console.ReadLine();
                    if (@String.IsNullOrEmpty(dbContextPathResponse) &&
                        (!dbContextPathResponse.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                        )
                    {
                        Settings();
                    }
                    else
                    {
                        var _settings = SettingsHelper.GetSettings();
                        _settings.DbContextName = dbContextPathResponse;
                        SettingsHelper.SaveSettings(_settings);


                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Saved");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(Environment.NewLine);
                        Settings();
                    }
                }
                else
                {
                    Console.WriteLine("You must type a numerical value within the option range");
                }

            }
            else
            {
                Console.WriteLine("You must type a numerical value");
            }
            Settings();
        }

        public string GetSecondWord(string input)
        {
            // Split the input string into words
            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // If there are at least two words, return the second word
            if (words.Length >= 2)
            {
                return words[1];
            }

            // Otherwise, return "TemplateNamespace"
            return "TemplateNamespace";
        }

        public void CreateNewConnectionMenu()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionFiles");
            // prompt user to enter connection string
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(@"Enter the connection string or type ""cancel"":");
            //Console.WriteLine(@"If you need to use a non standard connection string that does not include the database name, username, and password, you must use the following commands:");
            //GiveExplanationOfConnectionStringCommands();


            string connectionString = Console.ReadLine();

            if (connectionString.Equals("cancel", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Canceled creating connection string");
                ConnectionsMenu();
            }
           
            // generate unique filename and save connection string to file
            string fileName = $"connection_{DateTime.Now:yyyyMMddHHmmss}.txt";


            string ConnectionFileName = GetConnectionNameDatabase(connectionString);

            if (String.IsNullOrWhiteSpace(ConnectionFileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Connection String");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            string filePath = Path.Combine(folderPath, ConnectionFileName + ".LanCon");
            File.WriteAllText(filePath, connectionString.Encrypt());

            // inform user that connection was added and go back to ConnectionsMenu
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Connection added.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
        }
        public string RemoveSpaces(string input)
        {
            return new string(input.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        public static string RemoveSpecial_characters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

        private string GetConnectionNameDatabase(string connectionString)
        {
            string databaseName = "";

            // check if there are commands in the connection string
            if (connectionString.Contains("-"))
            {
                var dbName = GetCommandValue("databaseName", connectionString);
                databaseName = dbName;
            }
            else
            {
                // if there are no commands, extract the database name as before
                int databaseStartIndex = connectionString.IndexOf("Database=") + 9;
                if (databaseStartIndex >= 0)
                {
                    int databaseEndIndex = connectionString.IndexOf(";", databaseStartIndex);
                    if (databaseEndIndex >= 0)
                    {
                        databaseName = connectionString.Substring(databaseStartIndex, databaseEndIndex - databaseStartIndex);
                    }
                }
            }

            return RemoveSpecial_characters(RemoveSpaces(databaseName));
        }

        private string GetCommandValue(string command, string connectionString)
        {
            string value = null;

            // check if there are commands in the connection string
            if (connectionString.Contains("-"))
            {
                // split the connection string into segments based on the "-" command indicators
                string[] segments = connectionString.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                // loop through each segment to find the command and its value
                foreach (string segment in segments)
                {
                    // get the current command by splitting the segment based on space
                    string[] commandSegments = segment.Trim().Split(' ');

                    // check if the current segment contains a command
                    if (commandSegments.Length > 0)
                    {
                        // get the current command by removing the leading "-"
                        string currentCommand = commandSegments[0];

                        // check if the current command matches the requested command
                        if (currentCommand.Equals(command, StringComparison.OrdinalIgnoreCase))
                        {
                            // get the value associated with the command
                            value = commandSegments.Length > 1 ? commandSegments[1] : null;

                            // remove any surrounding quotes from the value
                            if (!string.IsNullOrEmpty(value) && value.StartsWith("\"") && value.EndsWith("\""))
                            {
                                value = value.Substring(1, value.Length - 2);
                            }

                            break;
                        }
                    }
                }
            }

            return value;
        }

        public static void GiveExplanationOfConnectionStringCommands()
        {
            Console.WriteLine("Here are the available commands to use with connection strings:");
            Console.WriteLine("-connectionString [connectionString]: The connection string to use for the database.");
            Console.WriteLine("-databaseName [databaseName]: The name of the database to target.");
            Console.WriteLine("-ConfigFile [configFilePath]: The file path of the config file associated with the project.");
            Console.WriteLine("-ConnectionStringIsPath [true/false]: Signifies whether the connection string is the full connection string or just a path to the connection string.");

            Console.WriteLine("\nExample usage:");
            Console.WriteLine("-connectionString \"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDatabase;Integrated Security=True;\" -databaseName MyDatabase -ConfigFile \"C:\\Projects\\MyProject\\App.config\" -ConnectionStringIsPath false");
        }

        public void RunScaffolding()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            Settings _settings = SettingsHelper.GetSettings();
            List<string> SettingsError = new List<string>();
            if (String.IsNullOrWhiteSpace(_settings.NameOfDevEnvDb))
            {
                SettingsError.Add("No dev environment designated! Go edit connection strings and set a dev environment.");
            }
            if (String.IsNullOrWhiteSpace(_settings.MainDbContextPath))
            {
                SettingsError.Add("No db context path set. Go to settings and set a context path.");
            }
            if (String.IsNullOrWhiteSpace(_settings.WorkingDirectory))
            {
                SettingsError.Add("No working directory path set. Go to settings and set a context path.");
            }
            if (String.IsNullOrWhiteSpace(_settings.PathToPlaceExtensionsAndMetaData))
            {
                SettingsError.Add("No path was designated where the program is supposed to create the extensions and metadata classes. This is option # 6 that you have to set in the settings.");
            }

            if (SettingsError.Count > 0)
            {
                Console.WriteLine(Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (string s in SettingsError)
                {
                    Console.WriteLine(s);
                }
                Console.ForegroundColor = ConsoleColor.White;
                MainMenu();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All required settings set");
            Console.ForegroundColor = ConsoleColor.White;


            string MainContextPath = _settings.MainDbContextPath; //@"C:\Source\Sayou\DataAccess\SayouDbContext.cs";
            string DevelopmentEnvironment = _settings.NameOfDevEnvDb;

            try
            {

                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionFiles");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // get all files in the ConnectionFiles folder
                string[] filePaths = Directory.GetFiles(folderPath);

                if (filePaths == null || filePaths.Count() == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No connection strings found! You must add connection strings to scaffold database");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }


                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("StartingScaffolding");
                Console.ForegroundColor = ConsoleColor.White;

                List<string> specificScaffoldFolders = new List<string>();

                bool WorkingDirectoryNotFound = false;


                foreach (string connectionFilePath in filePaths)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Starting Scaffold for: {GetFileNameFromPath(connectionFilePath)}");
                    Console.ForegroundColor = ConsoleColor.White;

                    //Console.WriteLine(Environment.NewLine);
                    string PathToPurgeScaffolding = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PurgeScaffolding");
                    if (!Directory.Exists(PathToPurgeScaffolding))
                    {
                        Directory.CreateDirectory(PathToPurgeScaffolding);
                    }



                    string PathToPurgeScaffoldingSpecificDb = Path.Combine(PathToPurgeScaffolding, GetFileNameFromPath(connectionFilePath));
                    Directory.CreateDirectory(PathToPurgeScaffoldingSpecificDb);
                    specificScaffoldFolders.Add(PathToPurgeScaffoldingSpecificDb);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Creating Temporary directory: {PathToPurgeScaffoldingSpecificDb}");
                    Console.ForegroundColor = ConsoleColor.White;

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Deleting old files in scaffolding temp file if they exist");
                    Console.ForegroundColor = ConsoleColor.White;
                    string[] filePathss = Directory.GetFiles(PathToPurgeScaffoldingSpecificDb);
                    if (filePathss.Length > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Found non deleted files in scaffolding folder. This should have been deleted. An error must of occurred during the last processed run");
                        Console.ForegroundColor = ConsoleColor.White;
                        DeleteFilesInDirectory(PathToPurgeScaffoldingSpecificDb);
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Reading and decrypting connection file");
                    Console.ForegroundColor = ConsoleColor.White;
                    string connectionString = File.ReadAllText(connectionFilePath).Decrypt();

                    string command = null;
                    if (!connectionString.Contains("-"))
                    {
                        command = $@"dbcontext scaffold ""{connectionString}"" Microsoft.EntityFrameworkCore.SqlServer -o ""{PathToPurgeScaffoldingSpecificDb}"" --force --data-annotations";
                    }
                    else
                    {
                        
                        var configFileFound = GetCommandValue("configFile", connectionString);
                        string AddConfigFile = null;
                        if (!String.IsNullOrEmpty(configFileFound))
                            AddConfigFile = $@"--config-file=""{configFileFound}""";

                        var ConnectStringFound = GetCommandValue("connectionString", connectionString);

                        command = $@"dbcontext scaffold {ConnectStringFound} Microsoft.EntityFrameworkCore.SqlServer -o ""{PathToPurgeScaffoldingSpecificDb}"" --force --data-annotations";
                    }
                    string pmcResponse = new ScaffoldFiles().RunCommandInPmc(command);

                    if (pmcResponse.Contains("A connection was successfully established with the server, but then an error occurred during the login process. (provider: SSL Provider, error: 0 - The certificate chain was issued by an authority that is not trusted.)", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Scaffold failed, but error recognized. Attempting to fix now...");
                        Console.ForegroundColor = ConsoleColor.White;

                        command = $@"dbcontext scaffold ""{AddTrustServerCertificate(connectionString)}"" Microsoft.EntityFrameworkCore.SqlServer -o ""{PathToPurgeScaffoldingSpecificDb}"" --force --data-annotations";
                        pmcResponse = new ScaffoldFiles().RunCommandInPmc(command);
                    }

                    if (pmcResponse.Contains("No project was found. Change the current working directory", StringComparison.OrdinalIgnoreCase))
                    {
                        WorkingDirectoryNotFound = true;
                        break;
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(pmcResponse);
                    Console.ForegroundColor = ConsoleColor.White;

                }

                if (WorkingDirectoryNotFound)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The working directory you chose was not found for one or all of the projects");
                    Console.ForegroundColor = ConsoleColor.White;
                    MainMenu();
                    return;
                }

                string ContextOverride = null;
                List<ScaffoldFile> contexts = new List<ScaffoldFile>();


                string dbSetRegion = null;
                string MainDevModelsPath = null;
                string MainContextToNotCopy = null;

                foreach (string scaffoldFolderPath in specificScaffoldFolders)
                {
                    IEnumerable<string> FoundContextFiles = GetContextFiles(scaffoldFolderPath);

                    if (FoundContextFiles.Count() == 1)
                    {

                        ContextOverride = ScaffoldFiles.ScaffoldEnvironment(MainContextPath, FoundContextFiles.FirstOrDefault(), out string ScaffoldContext, ContextOverride);

                        ScaffoldFile sff = new ScaffoldFile() { FilePath = scaffoldFolderPath, TextContent = ScaffoldContext, ModelBuildingContent = ScaffoldFiles.GetModelBuildingContent(ScaffoldContext) };
                        contexts.Add(sff);
                        if (GetFileNameWithoutExtension(scaffoldFolderPath).Equals(DevelopmentEnvironment, StringComparison.OrdinalIgnoreCase))
                        {
                            dbSetRegion = ScaffoldFiles.ExtractDbSetRegion(ScaffoldContext);
                            MainContextToNotCopy = new string(FoundContextFiles.FirstOrDefault());
                            MainDevModelsPath = new string(scaffoldFolderPath);
                        }
                    }
                    else if (FoundContextFiles.Count() == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("CRITICAL: Something went really wrong!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine("Multiple context files found:");
                        int i = 1;
                        foreach (string file in FoundContextFiles)
                        {
                            Console.WriteLine("{0}. {1}", i, GetFileNameFromPath(file));
                            i++;
                        }

                        bool validChoice = false;
                        int choice = 0;
                        while (!validChoice)
                        {
                            Console.Write("Enter the number associated with the file containing the DbContext: ");
                            string choiceStr = Console.ReadLine();
                            if (Int32.TryParse(choiceStr, out choice) && choice >= 1 && choice <= FoundContextFiles.Count())
                            {
                                validChoice = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice. Please choose a number between 1 and {0}.", FoundContextFiles.Count());
                            }
                        }

                        string chosenFilePath = FoundContextFiles.ElementAt(choice - 1);

                        ContextOverride = ScaffoldFiles.ScaffoldEnvironment(MainContextPath, chosenFilePath, out string ScaffoldContext, ContextOverride);
                        ScaffoldFile sff = new ScaffoldFile() { FilePath = scaffoldFolderPath, TextContent = ScaffoldContext, ModelBuildingContent = ScaffoldFiles.GetModelBuildingContent(ScaffoldContext) };
                        contexts.Add(sff);
                        if (GetFileNameWithoutExtension(scaffoldFolderPath).Equals(DevelopmentEnvironment, StringComparison.OrdinalIgnoreCase))
                        {
                            dbSetRegion = ScaffoldFiles.ExtractDbSetRegion(ScaffoldContext);
                            MainContextToNotCopy = new string(chosenFilePath);
                            MainDevModelsPath = new string(scaffoldFolderPath);
                        }

                    }
                }

                if (ContextOverride != null)
                {
                    ContextOverride = ScaffoldFiles.InsertDbSetRegion(dbSetRegion, ContextOverride);

                    List<string> enumNames = new List<string>();
                    foreach (string scaffoldFolderPath in specificScaffoldFolders)
                    {
                        enumNames.Add(GetFileNameWithoutExtension(scaffoldFolderPath));
                    }

                    if (enumNames.Count == 0 || enumNames.Where(x => x.Equals(_settings.NameOfDevEnvDb, StringComparison.OrdinalIgnoreCase)).Count() == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The selected development connection string doesn't exist after the scaffold process. Please make sure you have selected a dev connection string.");
                        Console.ForegroundColor = ConsoleColor.White;

                        MainMenu();
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Development connection string selected and scaffold associated found");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("creating environment variables");
                    Console.ForegroundColor = ConsoleColor.White;
                    string enumName = "DbEnvironment";
                    string EnumString = ScaffoldFiles.CreateEnumFromPropertyNames(enumNames, enumName);
                    ContextOverride = ScaffoldFiles.AddOrUpdateContentInRegion(ContextOverride, "EnvironmentEnum", EnumString);


                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Creating switch case entity builder");
                    Console.ForegroundColor = ConsoleColor.White;
                    string ModelEntityBuilder = ScaffoldFiles.BuildModelBuildingSwitch(contexts);
                    ContextOverride = ScaffoldFiles.AddOrUpdateContentInRegion(ContextOverride, "ModelBuilding", ModelEntityBuilder);


                    if (_settings.MainDbContextPath.Equals("default", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Saving Template");
                        Console.ForegroundColor = ConsoleColor.White;
                        string dbContextPath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TemplateDbContext");
                        string dbContextPath = Path.Combine(dbContextPath1, "DbContext.cs");
                        Directory.CreateDirectory(dbContextPath1);
                        TemplateContextHelper.SaveTemplate(ContextOverride, dbContextPath);
                        OpenFolder(dbContextPath1);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Saving template changes");
                        Console.ForegroundColor = ConsoleColor.White;
                        TemplateContextHelper.SaveTemplate(ContextOverride, _settings.MainDbContextPath);
                    }

                }

                List<string> csModelFiles = GetCsFiles(MainDevModelsPath);
                csModelFiles.RemoveAll(x => x == MainContextToNotCopy);



                GenExtra.GenerateMetadataAndExtensionFiles(csModelFiles.ToArray(), _settings.PathToPlaceExtensionsAndMetaData);
                GenExtra.GenerateDbCache(_settings.PathToPlaceExtensionsAndMetaData);
                GenExtra.GenerateDbHelper(_settings.PathToPlaceExtensionsAndMetaData);
                GenExtra.GenerateEntityHelper(_settings.PathToPlaceExtensionsAndMetaData);
                GenExtra.GenerateIReadOnlyRepository(_settings.PathToPlaceExtensionsAndMetaData);
                GenExtra.GenerateIRepository(_settings.PathToPlaceExtensionsAndMetaData);
                GenExtra.GenerateReadOnlyRepositoryBase(_settings.PathToPlaceExtensionsAndMetaData);
                GenExtra.GenerateRepositoryBase(_settings.PathToPlaceExtensionsAndMetaData);



                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Proccessing files and moving them to designated location");
                Console.ForegroundColor = ConsoleColor.White;

                string folderpath = _settings.ScaffoldModelsPath;
                if (_settings.ScaffoldModelsPath.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    folderpath = Path.Combine(_settings.PathToPlaceExtensionsAndMetaData, "DbModels");
                    Directory.CreateDirectory(folderpath);
                }
                ProcessFiles(csModelFiles, folderpath);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Opening Scaffold path");
                Console.ForegroundColor = ConsoleColor.White;
                //OpenFolder(_settings.ScaffoldModelsPath);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Deleting temp scaffold paths to remove any connection strings in plain text");
                Console.ForegroundColor = ConsoleColor.White;
                foreach (string tempScaffoldPath in specificScaffoldFolders)
                {
                    string[] filePathss = Directory.GetFiles(tempScaffoldPath);
                    if (filePathss.Length > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Deleting all files in: {tempScaffoldPath}");
                        Console.ForegroundColor = ConsoleColor.White;
                        DeleteFilesInDirectory(tempScaffoldPath);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{filePathss.Length} files deleted");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Done deleting temp scaffold paths");
                Console.ForegroundColor = ConsoleColor.White;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Scaffolding Complete");
                Console.ForegroundColor = ConsoleColor.White;
                MainMenu();
            }
            catch (Exception ex)
            {

                Console.WriteLine(Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CRITICAL: Something went really wrong!");
                Console.WriteLine("Go through the following folder and delete all the contents inside:");
                Console.WriteLine(ex?.Message);
                Console.WriteLine(ex?.InnerException?.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PurgeScaffolding"));
                Console.WriteLine(Environment.NewLine);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("If you do not delete everything inside the folder specified, then you will have plain text connection strings sitting in a text file. This is extremely dangerous. You must manually delete the files due to an unexpected error that has caused the program to be unable to verify if the folders were properly deleted or not.");
                MainMenu();
            }

        }

        public string AddTrustServerCertificate(string input)
        {
            if (!input.EndsWith(";"))
            {
                input += ";";
            }
            return input + "TrustServerCertificate=True;";
        }

        public void OpenFolder(string folderPath)
        {
            try
            {
                Process.Start("explorer.exe", folderPath);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                // Check if the error was due to lack of admin privileges
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);

                if (!isAdmin)
                {
                    // Ask for admin privileges
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.Verb = "runas";
                    startInfo.FileName = "explorer.exe";
                    startInfo.Arguments = folderPath;

                    try
                    {
                        Process.Start(startInfo);
                    }
                    catch (System.ComponentModel.Win32Exception ex2)
                    {
                        Console.WriteLine("Failed to open folder: " + ex2.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Failed to open folder: " + ex.Message);
                }
            }
        }

        public bool CheckFileNameExists(string folderPath, string fileName)
        {
            if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
            {
                return false;
            }

            string[] fileNames = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToArray();

            var test = fileNames.Contains(fileName);
            return test;
        }
        public List<string> GetCsFiles(string folderPath)
        {
            return Directory.GetFiles(folderPath, "*.cs")
                            .Select(Path.GetFullPath).ToList();
        }

        public void ProcessFiles(List<string> filePaths, string folderPath)
        {
            // Get the list of file names in the folder path
            string[] fileNames = Directory.GetFiles(folderPath).Select(Path.GetFileName).ToArray();

            // Delete any files in the folder path that do not have matching names in the List
            foreach (string fileName in fileNames)
            {
                if (!filePaths.Any(filePath => Path.GetFileName(filePath) == fileName))
                {
                    File.Delete(Path.Combine(folderPath, fileName));
                }
            }

            // Process each file in the List
            foreach (string filePath in filePaths)
            {
                // Read the contents of the file
                string content;
                using (FileStream stream = new FileStream(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        content = reader.ReadToEnd();
                    }
                }

                // Modify the content if the "Index" attribute is found
                string indexAttributePattern = @"\[(Index\()";
                string fullPathIndexAttribute = "[Microsoft.EntityFrameworkCore.Index(";
                content = Regex.Replace(content, indexAttributePattern, fullPathIndexAttribute);

                // Write the updated contents to a file in the folder path with the same name as the original file
                string fileName = Path.GetFileName(filePath);
                string outputPath = Path.Combine(folderPath, fileName);
                using (FileStream stream = new FileStream(outputPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(content);
                    }
                }
            }
        }

        public static IEnumerable<string> GetContextFiles(string folderLocation)
        {
            // Get all files within the specified folder location
            var allFiles = Directory.GetFiles(folderLocation);

            // Filter the files to only include those that have "Context.cs" within the file name
            var contextFiles = allFiles.Where(file => file.Contains("Context.cs"));

            return contextFiles;
        }

        public void DeleteFilesInDirectory(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath);
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
        }

        public string GetFileNameFromPath(string filePath)
        {
            //return GetFileNameWithoutExtension(filePath);;
            return Path.GetFileName(filePath);
        }

        public string GetFileNameWithoutExtension(string filePath)
        {
            string fileNameWithExtension = Path.GetFileName(filePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithExtension);
            return fileNameWithoutExtension.Replace(".", "");
        }
        public void ConnectionsMenu()
        {
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Database Connection Strings");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Environment.NewLine);
            // check if ConnectionFiles folder exists and create it if it doesn't
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConnectionFiles");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // get all files in the ConnectionFiles folder
            string[] filePaths = Directory.GetFiles(folderPath);

            // check if any files exist
            if (filePaths.Length == 0)
            {
                Console.WriteLine("No Connection Files Exist");

                // give option to go back to MainMenu or ConnectionsMenu
                Console.WriteLine("Type '1' to go back to the Main Menu or '2' to go back to the Connections Menu or '3' to to add a connection string");
                string response = Console.ReadLine();
                if (response == "1")
                {
                    MainMenu();
                }
                else if (response == "2")
                {
                    ConnectionsMenu();
                }
                else if (response == "3")
                {
                    CreateNewConnectionMenu();
                    ConnectionsMenu();
                }
                else
                {
                    Console.WriteLine("Invalid input. Going back to the Main Menu.");
                    MainMenu();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                // display contents of each file
                Settings _settings = SettingsHelper.GetSettings();
                foreach (string filePath in filePaths)
                {
                    if (_settings != null && !String.IsNullOrWhiteSpace(_settings.NameOfDevEnvDb)
                        && GetFileNameWithoutExtension(filePath) == _settings.NameOfDevEnvDb)
                    {

                        Console.Write(GetFileNameFromPath(filePath));
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" - Dev" + Environment.NewLine);
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else
                    {
                        //Console.WriteLine(File.ReadAllText(filePath));
                        Console.WriteLine(GetFileNameFromPath(filePath));
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Environment.NewLine);
                // give options to go back to MainMenu, add a new connection, or delete a file
                Console.WriteLine("Type the number associated with what you would like to do");
                Console.WriteLine("1.) Go back to the Main Menu");
                Console.WriteLine("2.) Add a new connection string");
                Console.WriteLine("3.) Delete a file");
                Console.Write("4.) Set connection string as Development");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" (Required)");
                Console.WriteLine(Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.White;
                string response = Console.ReadLine();

                if (response == "1")
                {
                    MainMenu();
                }
                else if (response == "2")
                {
                    CreateNewConnectionMenu();
                    ConnectionsMenu();
                }
                else if (response == "3")
                {
                    // prompt user to enter file name
                    Console.WriteLine("Enter the file name or type 'cancel':");
                    string fileName = Console.ReadLine();

                    if (!String.IsNullOrWhiteSpace(fileName) && !fileName.Trim().Equals("cancel", StringComparison.OrdinalIgnoreCase))
                    {
                        // check if file exists and delete it if it does
                        string filePath = Path.Combine(folderPath, fileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            Console.WriteLine("File deleted.");
                        }
                        else
                        {
                            Console.WriteLine("File not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("canceled");
                    }
                    // go back to ConnectionsMenu
                    ConnectionsMenu();
                }
                else if (response == "4")
                {
                    // prompt user to enter file name
                    Console.WriteLine("Enter the file name to set as your development or type 'cancel':");
                    string fileName = Console.ReadLine();

                    if (!String.IsNullOrWhiteSpace(fileName) && !fileName.Trim().Equals("cancel", StringComparison.OrdinalIgnoreCase))
                    {
                        // check if file exists and delete it if it does
                        string filePath = Path.Combine(folderPath, fileName);
                        if (File.Exists(filePath))
                        {
                            Settings settings = SettingsHelper.GetSettings();
                            settings.NameOfDevEnvDb = GetFileNameWithoutExtension(filePath);
                            SettingsHelper.SaveSettings(settings);
                            Console.WriteLine("Dev environment set");
                        }
                        else
                        {
                            Console.WriteLine("File not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("canceled");
                    }
                    // go back to ConnectionsMenu
                    ConnectionsMenu();
                }
                else
                {
                    Console.WriteLine("Invalid input. Going back to the Main Menu.");
                    MainMenu();
                }
            }
        }



    }
}
