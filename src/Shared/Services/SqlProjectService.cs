﻿namespace SSDTLifecycleExtension.Shared.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Contracts;
    using Contracts.DataAccess;
    using Contracts.Services;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class SqlProjectService : ISqlProjectService
    {
        private readonly IFileSystemAccess _fileSystemAccess;
        private readonly ILogger _logger;

        public SqlProjectService(IFileSystemAccess fileSystemAccess,
                                 ILogger logger)
        {
            _fileSystemAccess = fileSystemAccess ?? throw new ArgumentNullException(nameof(fileSystemAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private static void ReadProperties(XContainer root,
                                           out string name,
                                           out string outputPath,
                                           out string sqlTargetName)
        {
            name = null;
            outputPath = null;
            sqlTargetName = null;

            var propertyGroups = root.Elements().Where(m => m.Name.LocalName == "PropertyGroup").ToArray();
            foreach (var propertyGroup in propertyGroups)
            {
                // If the property group has a condition, check if that condition contains "Release", otherwise skip this group
                var conditionAttribute = propertyGroup.Attribute("Condition");
                if (conditionAttribute != null && !conditionAttribute.Value.Contains("Release"))
                    continue;

                var nameElement = propertyGroup.Elements().SingleOrDefault(m => m.Name.LocalName == "Name");
                if (nameElement != null)
                    name = nameElement.Value;

                var outputPathElement = propertyGroup.Elements().SingleOrDefault(m => m.Name.LocalName == "OutputPath");
                if (outputPathElement != null)
                    outputPath = outputPathElement.Value;

                var sqlTargetNameElement = propertyGroup.Elements().SingleOrDefault(m => m.Name.LocalName == "SqlTargetName");
                if (sqlTargetNameElement != null)
                    sqlTargetName = sqlTargetNameElement.Value;
            }
        }

        async Task<bool> ISqlProjectService.TryLoadSqlProjectPropertiesAsync(SqlProject project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            var projectDirectory = Path.GetDirectoryName(project.FullName);
            if (projectDirectory == null)
            {
                await _logger.LogAsync($"ERROR: Cannot get project directory for {project.FullName}");
                return false;
            }

            var content = await _fileSystemAccess.ReadFileAsync(project.FullName);
            var doc = XDocument.Parse(content);
            if (doc.Root == null)
            {
                await _logger.LogAsync($"ERROR: Cannot read contents of {project.FullName}");
                return false;
            }

            ReadProperties(doc.Root,
                          out var name,
                          out var outputPath,
                          out var sqlTargetName);

            if (name == null)
            {
                await _logger.LogAsync($"ERROR: Cannot read name of {project.FullName}");
                return false;
            }

            if (outputPath == null)
            {
                await _logger.LogAsync($"ERROR: Cannot read output path of {project.FullName}");
                return false;
            }

            // Set properties on the project object
            project.ProjectProperties.SqlTargetName = sqlTargetName ?? name;
            project.ProjectProperties.BinaryDirectory = Path.Combine(projectDirectory, outputPath);

            return true;
        }
    }
}