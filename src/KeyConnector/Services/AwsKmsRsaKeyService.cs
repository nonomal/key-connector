﻿using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;

namespace Bit.KeyConnector.Services
{
    public class AwsKmsRsaKeyService : IRsaKeyService
    {
        private readonly KeyConnectorSettings _settings;
        private readonly AmazonKeyManagementServiceClient _kmsClient;

        public AwsKmsRsaKeyService(
            KeyConnectorSettings settings)
        {
            _settings = settings;
            _kmsClient = new AmazonKeyManagementServiceClient(settings.RsaKey.AwsAccessKeyId,
                settings.RsaKey.AwsAccessKeySecret, RegionEndpoint.GetBySystemName(settings.RsaKey.AwsRegion));
        }

        public async Task<byte[]> EncryptAsync(byte[] data)
        {
            using var dataStream = new MemoryStream(data);
            var request = new EncryptRequest
            {
                KeyId = _settings.RsaKey.AwsKeyId,
                Plaintext = dataStream
            };
            var response = await _kmsClient.EncryptAsync(request);
            return response.CiphertextBlob.ToArray();
        }

        public async Task<byte[]> DecryptAsync(byte[] data)
        {
            using var dataStream = new MemoryStream(data);
            var request = new DecryptRequest
            {
                KeyId = _settings.RsaKey.AwsKeyId,
                CiphertextBlob = dataStream
            };
            var response = await _kmsClient.DecryptAsync(request);
            return response.Plaintext.ToArray();
        }

        public async Task<byte[]> SignAsync(byte[] data)
        {
            using var dataStream = new MemoryStream(data);
            var request = new SignRequest
            {
                KeyId = _settings.RsaKey.AwsKeyId,
                SigningAlgorithm = SigningAlgorithmSpec.RSASSA_PKCS1_V1_5_SHA_256,
                Message = dataStream,
                MessageType = MessageType.RAW
            };
            var response = await _kmsClient.SignAsync(request);
            return response.Signature.ToArray();
        }

        public async Task<bool> VerifyAsync(byte[] data, byte[] signature)
        {
            using var dataStream = new MemoryStream(data);
            using var signatureStream = new MemoryStream(data);
            var request = new VerifyRequest
            {
                KeyId = _settings.RsaKey.AwsKeyId,
                SigningAlgorithm = SigningAlgorithmSpec.RSASSA_PKCS1_V1_5_SHA_256,
                Message = dataStream,
                MessageType = MessageType.RAW,
                Signature = signatureStream
            };
            var response = await _kmsClient.VerifyAsync(request);
            return response.SignatureValid;
        }

        public async Task<byte[]> GetPublicKeyAsync()
        {
            var request = new GetPublicKeyRequest
            {
                KeyId = _settings.RsaKey.AwsKeyId
            };
            var response = await _kmsClient.GetPublicKeyAsync(request);
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(response.PublicKey.ToArray(), out _);
            return rsa.ExportSubjectPublicKeyInfo();
        }
    }
}
