﻿namespace SSDTLifecycleExtension.DataAccess
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.SqlServer.Dac;
    using Shared.Contracts.DataAccess;

    [UsedImplicitly]
    public class DacAccess : IDacAccess
    {
        public async Task<(string DeployScriptContent, string DeployReportContent, string[] Errors)> CreateDeployFilesAsync(string previousVersionDacpacPath,
                                                 string newVersionDacpacPath,
                                                 string publishProfilePath,
                                                 bool createDeployScript,
                                                 bool createDeployReport)
        {
            if (previousVersionDacpacPath == null)
                throw new ArgumentNullException(nameof(previousVersionDacpacPath));
            if (newVersionDacpacPath == null)
                throw new ArgumentNullException(nameof(newVersionDacpacPath));
            if (publishProfilePath == null)
                throw new ArgumentNullException(nameof(publishProfilePath));

            if (!createDeployScript && !createDeployReport)
                throw new InvalidOperationException($"Either {nameof(createDeployScript)} or {nameof(createDeployReport)} must be true.");

            var (publishResult, errors) = await Task.Run<(PublishResult Result, string[] Errors)>(() =>
            {
                var previousDacpac = DacPackage.Load(previousVersionDacpacPath, DacSchemaModelStorageType.Memory);
                var newDacpac = DacPackage.Load(newVersionDacpacPath, DacSchemaModelStorageType.Memory);
                var deployOptions = DacProfile.Load(publishProfilePath).DeployOptions;
                PublishResult result;
                try
                {
                    result = DacServices.Script(newDacpac,
                                                previousDacpac,
                                                "PRODUCTION",
                                                new PublishOptions
                                                {
                                                    GenerateDeploymentScript = createDeployScript,
                                                    GenerateDeploymentReport = createDeployReport,
                                                    DeployOptions = deployOptions
                                                });
                }
                catch (DacServicesException e)
                {
                    if (e.Messages.Any())
                    {
                        return (null, e.Messages
                                       .Select(m => m.ToString())
                                       .ToArray());
                    }

                    return (null, new[] { e.Message });
                }

                return (result, null);
            });

            return (publishResult?.DatabaseScript, publishResult?.DeploymentReport, errors);
        }
    }
}