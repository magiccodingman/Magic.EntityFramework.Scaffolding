using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Magic.EntityFramework.Scaffolding
{
    public static class GenExtra
    {
        public static void GenerateMetadataAndExtensionFiles(string[] csFiles, string outputFolderPath)
        {
            var extensionsFolderPath = Path.Combine(outputFolderPath, "Extensions");
            var metaDataClassesFolderPath = Path.Combine(outputFolderPath, "MetaDataClasses");

            Directory.CreateDirectory(extensionsFolderPath);
            Directory.CreateDirectory(metaDataClassesFolderPath);

            foreach (var csFile in csFiles)
            {
                var fileContent = ReadFileContent(csFile);
                var className = GetClassNameFromContent(fileContent);

                if (string.IsNullOrEmpty(className))
                {
                    continue;
                }

                

                var metadataClassName = className + "MetaData";
                var metadataFilePath = Path.Combine(metaDataClassesFolderPath, metadataClassName + ".cs");
                var extensionFilePath = Path.Combine(extensionsFolderPath, className + "Extension.cs");

                if (!File.Exists(metadataFilePath))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Creating Extension for: {new CliMenu().GetFileNameWithoutExtension(csFile)}");
                    Console.ForegroundColor = ConsoleColor.White;
                    var metadataClassContent = GenerateMetadataClassContent(className, metadataClassName);
                    File.WriteAllText(metadataFilePath, metadataClassContent);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Already Exists: {new CliMenu().GetFileNameWithoutExtension(csFile)}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (!File.Exists(extensionFilePath))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Creating Metadata for: {new CliMenu().GetFileNameWithoutExtension(csFile)}");
                    Console.ForegroundColor = ConsoleColor.White;
                    var extensionClassContent = GenerateExtensionClassContent(className, metadataClassName);
                    File.WriteAllText(extensionFilePath, extensionClassContent);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Already Exists: {new CliMenu().GetFileNameWithoutExtension(csFile)}");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                var keys = GetPrimaryKeyProperties(fileContent);
                GenExtra.GenerateConcreteFiles(className, keys, outputFolderPath);
                GenerateInterfaceFiles(className, outputFolderPath);
            }
        }

        private static string ReadFileContent(string filepath)
        {
            using (FileStream stream = new FileStream(filepath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    return content;
                }
            }
        }

        private static string GetClassNameFromContent(string content)
        {
            var match = Regex.Match(content, @"public partial class (\w+)");
            return match.Groups[1].Value;
        }

        private static string GenerateMetadataClassContent(string className, string metadataClassName)
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public class {metadataClassName}
    {{
    }}
}}
";
        }

        private static string GenerateExtensionClassContent(string className, string metadataClassName)
        {
            return $@"using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    [MetadataType(typeof({metadataClassName}))]
    public partial class {className}
    {{
    }}
}}
";
        }

       


        private static void GenerateInterfaceFiles(string className, string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "Interfaces");
            Directory.CreateDirectory(interfacesFolderPath);

            var interfaceFileName = $"I{className}Repository.cs";
            var interfaceFilePath = Path.Combine(interfacesFolderPath, interfaceFileName);

            if (!File.Exists(interfaceFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating Interface for: {className}");
                Console.ForegroundColor = ConsoleColor.White;
                var interfaceContent = GenerateInterfaceContent(className);
                File.WriteAllText(interfaceFilePath, interfaceContent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {interfaceFileName}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        public static void GenerateDbCache(string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "DbHelpers");
            Directory.CreateDirectory(interfacesFolderPath);

            var filename = $"DbCache.cs";
            var filepath = Path.Combine(interfacesFolderPath, filename);

            if (!File.Exists(filepath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating {filename}");
                Console.ForegroundColor = ConsoleColor.White;
                var filecontent = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public static class DbCache
    {{
        public static {SettingsHelper.GetSettings().DbContextName}.DbEnvironment dbEnvironment {{ get; set; }}
        public static string DecryptedConnectionString {{ get; set; }}
    }}
}}
";
                File.WriteAllText(filepath, filecontent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {filename}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void GenerateDbHelper(string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "DbHelpers");
            Directory.CreateDirectory(interfacesFolderPath);

            var filename = $"DbHelper.cs";
            var filepath = Path.Combine(interfacesFolderPath, filename);

            if (!File.Exists(filepath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating {filename}");
                Console.ForegroundColor = ConsoleColor.White;
                var filecontent = $@"
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public class DbHelper
    {{
        public {SettingsHelper.GetSettings().DbContextName} Get{SettingsHelper.GetSettings().DbContextName}()
        {{
            return new {SettingsHelper.GetSettings().DbContextName}(new DbContextOptionsBuilder<{SettingsHelper.GetSettings().DbContextName}>().UseSqlServer(GetConnectionString()).Options, DbCache.dbEnvironment);
        }}

        public string GetConnectionString()
        {{
            return DbCache.DecryptedConnectionString;
        }}
    }}
}}
";
                File.WriteAllText(filepath, filecontent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {filename}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void GenerateEntityHelper(string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "DbHelpers");
            Directory.CreateDirectory(interfacesFolderPath);

            var filename = $"EntityHelper.cs";
            var filepath = Path.Combine(interfacesFolderPath, filename);

            if (!File.Exists(filepath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating {filename}");
                Console.ForegroundColor = ConsoleColor.White;
                var filecontent = $@"
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public static class EntityHelper
    {{
        public static PropertyInfo[] GetKeyProperties(Type EntityType) 
        {{
            var properties = EntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties.Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToArray();
        }}

        public static PropertyInfo[] GetKeyProperties<T>() where T : class
        {{
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties.Where(p => p.GetCustomAttribute<KeyAttribute>() != null).ToArray();
        }}
    }}
}}

";
                File.WriteAllText(filepath, filecontent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {filename}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void GenerateIReadOnlyRepository(string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "DbHelpers");
            Directory.CreateDirectory(interfacesFolderPath);

            var filename = $"IReadOnlyRepository.cs";
            var filepath = Path.Combine(interfacesFolderPath, filename);

            if (!File.Exists(filepath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating {filename}");
                Console.ForegroundColor = ConsoleColor.White;
                var filecontent = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public interface IReadOnlyRepository<TEntity>
    {{
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
    }}
}}

";
                File.WriteAllText(filepath, filecontent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {filename}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void GenerateIRepository(string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "DbHelpers");
            Directory.CreateDirectory(interfacesFolderPath);

            var filename = $"IRepository.cs";
            var filepath = Path.Combine(interfacesFolderPath, filename);

            if (!File.Exists(filepath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating {filename}");
                Console.ForegroundColor = ConsoleColor.White;
                var filecontent = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {{
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entitys);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entitys);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entitys);
    }}
}}

";
                File.WriteAllText(filepath, filecontent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {filename}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void GenerateReadOnlyRepositoryBase(string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "DbHelpers");
            Directory.CreateDirectory(interfacesFolderPath);

            var filename = $"ReadOnlyRepositoryBase.cs";
            var filepath = Path.Combine(interfacesFolderPath, filename);

            if (!File.Exists(filepath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating {filename}");
                Console.ForegroundColor = ConsoleColor.White;
                var filecontent = $@"
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public class ReadOnlyRepositoryBase<TEntity> where TEntity : class
    {{
        protected SayouDbContext _dbContext {{ get; set; }}
        protected DbSet<TEntity> _dbSet;

        public ReadOnlyRepositoryBase()
        {{
            _dbContext = new DbHelper().Get{SettingsHelper.GetSettings().DbContextName}();
            _dbSet = _dbContext.Set<TEntity>();
        }}
        public virtual IEnumerable<TEntity> GetAll()
        {{
            return _dbSet;
        }}
    }}
}}

";
                File.WriteAllText(filepath, filecontent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {filename}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void GenerateRepositoryBase(string outputFolderPath)
        {
            var interfacesFolderPath = Path.Combine(outputFolderPath, "DbHelpers");
            Directory.CreateDirectory(interfacesFolderPath);

            var filename = $"RepositoryBase.cs";
            var filepath = Path.Combine(interfacesFolderPath, filename);

            if (!File.Exists(filepath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating {filename}");
                Console.ForegroundColor = ConsoleColor.White;
                var filecontent = $@"
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace {SettingsHelper.GetSettings().WorkingDirectory}
{{
    public abstract class RepositoryBase<TEntity> : ReadOnlyRepositoryBase<TEntity> where TEntity : class
    {{
        public virtual void Add(TEntity entity)
        {{
            try
            {{
                _dbContext.Set<TEntity>().Add(entity);
                _dbContext.SaveChanges();
            }}
            catch (DbEntityValidationException e)
            {{
                LogDbErrors(e);
                throw;
            }}
        }}

        public virtual void AddRange(IEnumerable<TEntity> entitys)
        {{
            try
            {{
                _dbContext.Set<TEntity>().AddRange(entitys);
                _dbContext.SaveChanges();
            }}
            catch (DbEntityValidationException e)
            {{
                LogDbErrors(e);
                throw;
            }}
        }}

        public virtual void Update(TEntity entity)
        {{
            try
            {{
                if (_dbContext.Entry(entity).State == EntityState.Detached)
                {{
                    _dbContext.Set<TEntity>().Attach(entity);
                }}
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }}
            catch (DbEntityValidationException e)
            {{
                LogDbErrors(e);
                throw;
            }}
        }}

        public virtual void UpdateRange(IEnumerable<TEntity> entitys)
        {{
            try
            {{
                foreach (TEntity entity in entitys)
                {{
                    if (_dbContext.Entry(entity).State == EntityState.Detached)
                    {{
                        _dbContext.Set<TEntity>().Attach(entity);
                    }}
                    _dbContext.Entry(entity).State = EntityState.Modified;
                }}
                _dbContext.SaveChanges();
            }}
            catch (DbEntityValidationException e)
            {{
                LogDbErrors(e);
                throw;
            }}
        }}

        public virtual void Delete(TEntity entity)
        {{
            try
            {{
                _dbContext.Set<TEntity>().Remove(entity);
                _dbContext.SaveChanges();
            }}
            catch (DbEntityValidationException e)
            {{
                LogDbErrors(e);
                throw;
            }}
        }}

        public virtual void DeleteRange(IEnumerable<TEntity> entitys)
        {{
            try
            {{
                _dbContext.Set<TEntity>().RemoveRange(entitys);
                _dbContext.SaveChanges();
            }}
            catch (DbEntityValidationException e)
            {{
                LogDbErrors(e);
                throw;
            }}
        }}

        private void LogDbErrors(DbEntityValidationException e)
        {{
            foreach (var eve in e.EntityValidationErrors)
            {{
                Console.WriteLine(""Entity of type \""{{0}}\"" in state \""{{1}}\"" has the following validation errors:"",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {{
                    Console.WriteLine(""- Property: \""{{0}}\"", Error: \""{{1}}\"""",
                        ve.PropertyName, ve.ErrorMessage);
                }}
            }}
        }}
    }}
}}

";
                File.WriteAllText(filepath, filecontent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {filename}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        private static void GenerateConcreteFiles(string className, Dictionary<string, string> keys, string outputFolderPath)
        {
            var concreteFolderPath = Path.Combine(outputFolderPath, "Concrete");
            Directory.CreateDirectory(concreteFolderPath);

            var concreteFileName = $"{className}Repository.cs";
            var concreteFilePath = Path.Combine(concreteFolderPath, concreteFileName);

            if (!File.Exists(concreteFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Creating Concrete Implementation for: {className}");
                Console.ForegroundColor = ConsoleColor.White;
                var concreteContent = GenerateConcreteContent(className, keys);
                File.WriteAllText(concreteFilePath, concreteContent);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Already Exists: {concreteFileName}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static string GenerateInterfaceContent(string className)
        {
            return $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}.Interfaces
{{
    public interface I{className}Repository : IRepository<{className}>
    {{
    }}
}}
";
        }

        private static string GenerateConcreteContent(string className, Dictionary<string, string> keys)
        {
            string overloadedGetById = GenerateOverloadedGetById(className, keys);
            string defaultGetById = GenerateDefaultGetById(className, keys);

            return $@"
using {SettingsHelper.GetSettings().WorkingDirectory}.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace {SettingsHelper.GetSettings().WorkingDirectory}.Concrete
{{
    public class {className}Repository : RepositoryBase<{className}>, I{className}Repository
    {{
        {overloadedGetById}
        {defaultGetById}
    }}
}}
";
        }

        private static string GenerateOverloadedGetById(string className, Dictionary<string, string> keys)
        {
            var overloadedParameters = string.Join(", ", keys.Select(key => $"{key.Value} {key.Key}"));

            var dictionaryInitialization = string.Join(", ", keys.Select(key => $"{{ \"{key.Key}\", {key.Key} }}"));

            return $@"// Overloaded method that takes multiple parameters
        public {className} GetById({overloadedParameters})
        {{
            // Call the default implementation with the parameters as a dictionary
            return GetById((object)new Dictionary<string, object> {{ {dictionaryInitialization} }});
        }}";
        }
        private static string GenerateDefaultGetById(string className, Dictionary<string, string> keys)
        {
            var conditions = string.Join(" && ", keys.Select(key => $"c.{key.Key} == ({key.Value})idDict[\"{key.Key}\"]"));
            var typeChecksAndConversions = string.Join(Environment.NewLine, keys.Select(key => GetIdTypeCheckAndConversionCode(key.Key)));

            return $@"public {className} GetById(object id)
    {{
        var idDict = id as IDictionary<string, object>;
        if (idDict == null)
        {{
            throw new ArgumentException(""id must be a dictionary with string keys"");
        }}
        {typeChecksAndConversions}

        // Default implementation that takes an object parameter
        return _dbSet.Where(c => {conditions}).FirstOrDefault();
    }}";
        }

        private static string GetKeyType(string key)
        {
            // Add logic to determine the type of the key based on the class property.
            // For now, we are assuming "long" as the default type.
            return "long";
        }

        private static string GetIdTypeCheckAndConversionCode(string key)
        {
            string keyType = GetKeyType(key);
            return $@"object temp{key};
            if (!idDict.TryGetValue(""{key}"", out temp{key}))
            {{
                throw new ArgumentException(""id must contain key '{key}'"");
            }}
            {keyType} {key}Value;
            if (!{keyType}.TryParse(temp{key}.ToString(), out {key}Value))
            {{
                throw new ArgumentException(""id['{key}'] must be a valid {keyType} integer"");
            }}";
        }

        private static Dictionary<string, string> GetPrimaryKeyProperties(string content)
        {
            var matches = Regex.Matches(content, @"\[Key\]\s*public\s+(\w+)\s+(\w+)");
            return matches.Cast<Match>().ToDictionary(m => m.Groups[2].Value, m => m.Groups[1].Value);
            //var matches = Regex.Matches(content, @"\[Key\]\s*public\s+\w+\s+(\w+)");
            //return matches.Cast<Match>().Select(m => m.Groups[1].Value).ToArray();
        }

    }
}
