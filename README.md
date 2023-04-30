# Magic EntityFramework Scaffolding

Magic EntityFramework Scaffolding is a console application designed to streamline the setup and management of your C# Entity Framework projects. It automates the creation of model classes, extension classes, metadata classes, repository classes, environment switching logic classes, helper classes, concrete classes, and interface classes for both database-first and code-first projects. By intelligently managing your code, it significantly reduces the amount of manual work required when adding new models that match your database. The application also provides an easy-to-use repository for common CRUD operations.

Magic EntityFramework Scaffolding takes care of environment configurations by allowing you to input connection strings for different environments. It includes smart logic to avoid overwriting your changes to extensions, metadata, and more, while updating the main models as needed.

## Setup Process

Follow these steps to set up and use Magic EntityFramework Scaffolding:

### 1. Install .NET 7 Core SDK

Ensure that you have the .NET 7 Core SDK installed on your computer to run the application.

### 2. Update Encryption Keys

Open the `SecurityUtilities.cs` file and change the values for `PasswordHash`, `SaltKey`, `VIKey`, and `SaltKeyUnknown`. This is crucial for security, as the default keys provided in the example should not be used in a production environment. Replace the example keys with your own unique, secure keys.

Example:
```csharp
public static class SecurityUtilities
{
    static readonly string PasswordHash = "YOUR_UNIQUE_PASSWORD_HASH";
    static readonly string SaltKey = "YOUR_UNIQUE_SALT_KEY";
    static readonly string VIKey = "YOUR_UNIQUE_VI_KEY";
    static readonly string SaltKeyUnknown = "YOUR_UNIQUE_SALT_KEY_UNKNOWN";
}
```

### 3. Launch the App

Upon launching the app, it will perform an initial system check to ensure the required dotnet commands are available. If not, it will attempt to install the necessary dotnet tool package. If the installation fails, follow the provided error message instructions.

### 4. Configure the Application

Once the initial check is complete, you will be presented with the Main Menu. Follow these steps to configure your environment and settings:

1. Add and manage connection strings for your environments.
2. Set up your dbContext file and namespaces.
3. Configure scaffold paths and dbContext naming conventions.

Refer to the provided instructions in the original question for a detailed walkthrough of the configuration process.

With these settings in place, Magic EntityFramework Scaffolding will use smart logic to handle namespaces, file generation, environment management, and more. Once configured, you can take advantage of this setup to easily manage your Entity Framework database projects.

### 3. Launch the Application and Run Initial System Check

When you first launch the application, it will run an initial system check to verify that the required dotnet commands are available. If they are not, the application will attempt to install the necessary dotnet tool package. If the installation fails, an error message will be displayed, informing you of the required action.

### 4. Main Menu

After passing the initial system check, you will be presented with the Main Menu:

```
Main Menu
Type the number associated with what you would like to do:
1.) View/Edit Connection Strings/Environments
2.) Scaffold Environments
3.) Settings
```

Begin by selecting `1.) View/Edit Connection Strings/Environments`.

### 5. Connection Strings/Environments

In this menu, you will manage your database connection strings. If no connection files exist, you will see the following prompt:

```
No Connection Files Exist
Type '1' to go back to the Main Menu or '2' to go back to the Connections Menu or '3' to to add a connection string
```

Type `3` to add a new connection string. After adding a connection string, you will be taken back to the Database Connection Strings menu with additional options:

```
Database Connection Strings

Database1.LanCon
Database2.LanCon

Type the number associated with what you would like to do
1.) Go back to the Main Menu
2.) Add a new connection string
3.) Delete a file
4.) Set connection string as Development (Required)
```

Add all your connection strings, and then type `4` to specify which one is the development connection string. This will mark the selected connection string as the development environment:

```
Database Connection Strings

Database1.LanCon - Dev
Database2.LanCon

Type the number associated with what you would like to do
1.) Go back to the Main Menu
2.) Add a new connection string
3.) Delete a file
4.) Set connection string as Development (Required)
```

Press `1` to return to the Main Menu, and then type `3` to access the Settings menu.

### 6. Settings

The Settings menu presents several options for configuring the application:

```
Settings
Type the number associated with what you would like to do:
1.) Go back to main menu
2.) Show setting values
3.) Set file path of dbContext file or type 'default' to use the template
4.) Set namespace of your working directory or type 'default' to generate a generic template
5.) Designate final scaffold path for all models
6.) Set path to create folders for Extension classes and MetaData classes
7.) Set dbContext name
```

You must set option `6` in the Settings menu to specify the folder where many auto-generated files will be created. The other settings may not require input, especially if you have a code-first application. However, if you have a database-first project, you will want to configure the other settings for proper project management:

- Option `3`: Set the file location of your DbContext file. If you do not have a DbContext file, the application will auto-generate one in the specified location.
- Option `4`: Set the namespace of your working directory, typically your DataAccess library project.
- Option `5`: Choose the folder path where you want the models to be generated, usually your project's model folder.
- Option `6`: Specify where you want multiple folders and files to be generated for Extension and MetaData classes.
- Option `7`: Set a custom name for your dbContext if you do not want to use the default "dbContext" name.

The application uses smart logic to handle namespaces, file generation, environment handling, and more behind the scenes. Once these settings are configured, you do not need to set them again, and you can use the new setup to manage your Entity Framework database with ease.

### 7. Scaffolding Environments

With your settings and connection strings configured, you can now scaffold your environments. Return to the Main Menu and select `2.) Scaffold Environments`. The application will use your configurations to generate model classes, extension classes, metadata classes, repository classes, environment switching logic classes, helper classes, concrete classes, and interface classes for your project.

Magic EntityFramework Scaffolding will intelligently manage your code to significantly reduce the amount of manual work required when adding new models that match your database. The application also provides an easy-to-use repository for common CRUD operations.

### 8. Updating and Managing Your Project

As you make changes to your database schema, you can continue to use Magic EntityFramework Scaffolding to update your project. Remember that the application includes smart logic to avoid overwriting your changes to extensions, metadata, and more, while updating the main models as needed. The dbContext.cs file does overwrite that file in specified locations, but large comments and regions are written in that file to inform you as to where you can and cannot write in that file.

## Conclusion

Magic EntityFramework Scaffolding streamlines the setup and management of your C# Entity Framework projects by automating the creation of various classes and managing environment configurations. By following the steps outlined in this documentation, you can quickly set up and manage your Entity Framework projects with ease, saving time and effort on manual tasks.
