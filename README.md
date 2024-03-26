# ShoelessJoe API (C# Edition)

## Prerequisites
### Download XAMPP
1. If you do not have it installed already, please install XAMPP. You can do so by going to by clicking this link https://www.apachefriends.org/
2. Follow the onscreen directions to install the program.
3. Once it is installed, run the XAMPP appliaction. (It is recommend that you are an adminstrator)

### Creating the ShoelessJoe database
1. Click the "Start" button beside "Appache" and "MySQL".
2. Click the "Admin" button beside "MySQL", it will take you to the phpMyAdmin website.
3. Click the "New" link  on the left sidebar
4. Type "shoelessjoecsharp" (not case-sensitive) in the "Database name" textbox.
5. Click "Create" button

### Download the source code
1. Either download or clone the git repository to local machine.

### Additional files
1. You will need to create a file called "SecretConfig.cs" (case-sensitive) in the ShoelessJoeAPI.DataAccess root directory.
2. Copy the following code and place in the newly created file.
#### SecretConfig.cs
```c#
namespace ShoelessJoeAPI.DataAccess.DataModels
{
    public class SecretConfig
    {
        public static string ConnectionString { get; } = "server={your server};user={your username};password={your password};database=ShoelessJoeCSharp";
        public static Version Version { get; } = new Version(8, 0, 31);
        public static string ErrorPath { get; } = "{Your path to error log}";
        public static string LocalAppUrl { get; } = "{Your URL for LOCAL development}";
    }
}

```
### Updating the database
1. Inside Visual Studio, in the "Package Manager Console"
2. Make sure the Default Project is set to ShoelessJoe.DataAccess
```bash
Update-Database
```

## Usage
1. Press ctrl + F5