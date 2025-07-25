using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace BookGen.Infrastructure.Tools;

internal static class Digest
{
    public async static Task<bool> VerifyDigest(string digest, Stream stream)
    {
        string[] parts = digest.Split(':');
        if (parts.Length != 2)
            throw new InvalidProgramException($"Invalid format digest: {digest}");

        using HashAlgorithm algorithm = CreateAlgorithm(parts[0]);

        byte[] result = await algorithm.ComputeHashAsync(stream);
        return Compare(result, Convert.FromHexString(parts[1]));
    }

    private static unsafe bool Compare(byte[] result, byte[] bytes)
    {
        if (result.Length != bytes.Length)
            return false;

        if (Avx2.IsSupported && Sse2.IsSupported)
        {
            fixed (byte* pA = result, pB = bytes)
            {
                int length = result.Length;

                if (length == 32) // 256-bit
                {
                    var va = Avx.LoadVector256(pA);
                    var vb = Avx.LoadVector256(pB);

                    var cmp = Avx2.CompareEqual(va, vb);
                    return Avx2.MoveMask(cmp) == -1;
                }
                else if (length == 48) // 384-bit
                {
                    var va1 = Avx.LoadVector256(pA);       // first 32 bytes
                    var vb1 = Avx.LoadVector256(pB);

                    var va2 = Sse2.LoadVector128(pA + 32);  // remaining 16 bytes
                    var vb2 = Sse2.LoadVector128(pB + 32);

                    var cmp1 = Avx2.CompareEqual(va1, vb1);
                    var cmp2 = Sse2.CompareEqual(va2, vb2);

                    return Avx2.MoveMask(cmp1) == -1 && Sse2.MoveMask(cmp2) == 0xFFFF;
                }
                else if (length == 64) // 512-bit
                {
                    var va1 = Avx.LoadVector256(pA);
                    var vb1 = Avx.LoadVector256(pB);

                    var va2 = Avx.LoadVector256(pA + 32);
                    var vb2 = Avx.LoadVector256(pB + 32);

                    var cmp1 = Avx2.CompareEqual(va1, vb1);
                    var cmp2 = Avx2.CompareEqual(va2, vb2);

                    return Avx2.MoveMask(cmp1) == -1 && Avx2.MoveMask(cmp2) == -1;
                }
            }
        }

        return result.SequenceEqual(bytes);
    }

    private static HashAlgorithm CreateAlgorithm(string algorithm)
    {
        return algorithm switch
        {
            "sha256" => SHA256.Create(),
            "sha384" => SHA384.Create(),
            "sha512" => SHA512.Create(),
            _ => throw new InvalidOperationException($"unknown digest: {algorithm}"),
        };
    }
}
