using System;
using System.Security.Cryptography;
using System.Text;

namespace Slickflow.AI.Configuration
{
    /// <summary>
    /// Cross-platform symmetric crypto helper for API keys.
    /// - AES-256-GCM with HKDF-SHA256 per-record derived key
    /// - Master key is provided via DI-loaded options (appsettings) (Base64, 32 bytes)
    /// - Cipher text format: Base64(IV(12) || TAG(16) || CIPHERTEXT)
    /// - AAD binds recordId to the ciphertext to mitigate misuse
    /// </summary>
    public static class ApiKeyCryptoHelper
    {
        private const int MasterKeyLength = 32; // 256-bit
        private const int IvLength = 12;        // AES-GCM nonce
        private const int TagLength = 16;       // AES-GCM tag
        private static AiAppConfigProviderOptions? _aiAppConfigOptions;

        /// <summary>
        /// Expose current options (for consumers needing provider-specific configs).
        /// </summary>
        public static AiAppConfigProviderOptions AiAppConfigOptions =>
            _aiAppConfigOptions ?? throw new InvalidOperationException("ApiKeyCryptoHelper is not configured. Call Configure(AIAppConfigProviderOptions) during startup.");

        /// <summary>
        /// Configure helper with DI-provided options.
        /// Call once at startup (e.g., in Program.cs).
        /// </summary>
        public static void Configure(AiAppConfigProviderOptions options)
        {
            _aiAppConfigOptions = options ?? throw new ArgumentNullException(nameof(options));
        }

        public static string Encrypt(string plainText, string recordId)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));

            var masterKey = GetMasterKey();
            var aad = Encoding.UTF8.GetBytes(recordId ?? string.Empty);
            var derivedKey = DeriveKey(masterKey, aad);

            var iv = RandomNumberGenerator.GetBytes(IvLength);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = new byte[plainBytes.Length];
            var tagBytes = new byte[TagLength];

            using (var aes = new AesGcm(derivedKey))
            {
                aes.Encrypt(iv, plainBytes, cipherBytes, tagBytes, aad);
            }

            // Combine: IV || TAG || CIPHER
            var payload = new byte[IvLength + TagLength + cipherBytes.Length];
            Buffer.BlockCopy(iv, 0, payload, 0, IvLength);
            Buffer.BlockCopy(tagBytes, 0, payload, IvLength, TagLength);
            Buffer.BlockCopy(cipherBytes, 0, payload, IvLength + TagLength, cipherBytes.Length);

            return Convert.ToBase64String(payload);
        }

        public static string Decrypt(string cipherText, string recordId)
        {
            if (string.IsNullOrWhiteSpace(cipherText)) throw new ArgumentNullException(nameof(cipherText));

            var payload = Convert.FromBase64String(cipherText);
            if (payload.Length < IvLength + TagLength)
            {
                throw new CryptographicException("Invalid cipher payload.");
            }

            var iv = new byte[IvLength];
            var tag = new byte[TagLength];
            var cipher = new byte[payload.Length - IvLength - TagLength];

            Buffer.BlockCopy(payload, 0, iv, 0, IvLength);
            Buffer.BlockCopy(payload, IvLength, tag, 0, TagLength);
            Buffer.BlockCopy(payload, IvLength + TagLength, cipher, 0, cipher.Length);

            var masterKey = GetMasterKey();
            var aad = Encoding.UTF8.GetBytes(recordId ?? string.Empty);
            var derivedKey = DeriveKey(masterKey, aad);

            var plainBytes = new byte[cipher.Length];
            using (var aes = new AesGcm(derivedKey))
            {
                aes.Decrypt(iv, cipher, tag, plainBytes, aad);
            }

            return Encoding.UTF8.GetString(plainBytes);
        }

        /// <summary>
        /// Masks a secret for logs / UI (keep tail visible).
        /// </summary>
        public static string Mask(string? secret, int visibleTail = 4)
        {
            if (string.IsNullOrEmpty(secret)) return string.Empty;
            var tail = secret.Length <= visibleTail ? secret : secret[^visibleTail..];
            return $"***{tail}";
        }

        private static byte[] GetMasterKey()
        {
            if (_aiAppConfigOptions == null)
            {
                throw new InvalidOperationException("ApiKeyCryptoHelper is not configured. Call Configure(AIModelProviderOptions) during startup.");
            }

            var base64Key = _aiAppConfigOptions.MasterKeyDb;
            if (string.IsNullOrWhiteSpace(base64Key))
            {
                throw new InvalidOperationException("Master key is not configured in AIModelProvider options.");
            }

            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(base64Key);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("Master key is not valid Base64.");
            }

            if (keyBytes.Length < MasterKeyLength)
            {
                throw new InvalidOperationException($"Master key must be {MasterKeyLength} bytes (Base64-encoded).");
            }

            if (keyBytes.Length > MasterKeyLength)
            {
                var trimmed = new byte[MasterKeyLength];
                Buffer.BlockCopy(keyBytes, 0, trimmed, 0, MasterKeyLength);
                return trimmed;
            }

            return keyBytes;
        }

        // HKDF-SHA256: derive a 32-byte sub-key bound to recordId (AAD)
        private static byte[] DeriveKey(byte[] masterKey, byte[] info)
        {
            var salt = Array.Empty<byte>();

            // Extract
            byte[] prk;
            using (var hmac = new HMACSHA256(salt.Length == 0 ? new byte[32] : salt))
            {
                prk = hmac.ComputeHash(masterKey);
            }

            // Expand
            var okm = new byte[MasterKeyLength];
            var previousBlock = Array.Empty<byte>();
            var blockIndex = 1;
            var generated = 0;

            while (generated < MasterKeyLength)
            {
                byte[] input = new byte[previousBlock.Length + info.Length + 1];
                Buffer.BlockCopy(previousBlock, 0, input, 0, previousBlock.Length);
                Buffer.BlockCopy(info, 0, input, previousBlock.Length, info.Length);
                input[^1] = (byte)blockIndex;

                using (var hmac = new HMACSHA256(prk))
                {
                    previousBlock = hmac.ComputeHash(input);
                }

                var toCopy = Math.Min(previousBlock.Length, MasterKeyLength - generated);
                Buffer.BlockCopy(previousBlock, 0, okm, generated, toCopy);
                generated += toCopy;
                blockIndex++;
            }

            return okm;
        }
    }
}

