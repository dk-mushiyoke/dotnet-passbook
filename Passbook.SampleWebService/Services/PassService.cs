﻿using Passbook.Generator;
using Passbook.Generator.Fields;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Passbook.SampleWebService.Services
{
    public class PassService : IPassService
    {
        private readonly PassGeneratorConfiguration _generatorConfiguration;

        public PassService(PassGeneratorConfiguration generatorConfiguration)
        {
            _generatorConfiguration = generatorConfiguration;
        }

        public Task<byte[]> GeneratePassAsync(string serialNumber, string value, string secret)
        {
            var generatorRequest = new PassGeneratorRequest();

            generatorRequest.AppleWWDRCACertificate = GetCertificateContents(_generatorConfiguration.AppleWWDRCACertificatePath);
            generatorRequest.Certificate = GetCertificateContents(_generatorConfiguration.CertificatePath);
            generatorRequest.CertificatePassword = _generatorConfiguration.CertificatePassword;

            generatorRequest.AddPrimaryField(new StandardField("primary-value-text", "Main Value", value));

            generatorRequest.AuthenticationToken = secret;
            generatorRequest.WebServiceUrl = _generatorConfiguration.WebServiceUrl;

            generatorRequest.PassTypeIdentifier = _generatorConfiguration.PassTypeIdentifier;
            generatorRequest.TeamIdentifier = _generatorConfiguration.TeamIdentifier;
            generatorRequest.Description = "Pass";
            generatorRequest.OrganizationName = "Organisation";
            generatorRequest.LogoText = "Pass";

            generatorRequest.SerialNumber = serialNumber;

            var generator = new PassGenerator();
            var rawPass = generator.Generate(generatorRequest);

            return rawPass;
        }

        private byte[] GetCertificateContents(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}