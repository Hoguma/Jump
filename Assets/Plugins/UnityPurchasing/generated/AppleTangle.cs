#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("R0BDRkJBRChlf0FHQkBCS0BDRkJ1ng9L8fkhUqFKtsPN6D14GY1ZjsdI34Z9fHLgecNTZFwGp05/qRBkAh4XUiAdHQZSMTNCbGV/QkRCRkDZ0QPgNSEns91dM8GKiZECv5TRPgATEQYbERdSAQYTBhcfFxwGAVxCBQVcEwICHhdcER0fXRMCAh4XEROyEUEFhUh1XiSZqH1TfKjIAWs9xxwWUhEdHBYbBhsdHAFSHRRSBwEX2q4MUEe4V6erfaQZptBWUWOF0953cnHwc31yQvBzeHDwc3NyluPbe3Z0YXAnIUNhQmN0cSd2eGF4MwICEB4XUgEGExwWEwAWUgYXAB8BUhMeF1I7HBFcQ1RCVnRxJ3Z5YW8zAvBzcnR7WPQ69IURFndzQvOAQlh0GxQbERMGGx0cUjMHBhodABsGC0NUQlZ0cSd2eWFvMwICHhdSMRcABn90e1j0OvSFf3Nzd3dycfBzc3IuK9V3ew5lMiRjbAahxflRSTXRpx1WkJmjxQKtfTeTVbiDHwqflcdlZULwdslC8HHR0nFwc3Bwc3BCf3R7T1QVUvhBGIV/8L2smdFdiyEYKRZBRChCEEN5Qnt0cSd2dGFwJyFDYVIxM0Lwc1BCf3R7WPQ69IV/c3Nz+Wv7rIs5Hod12VBCcJpqTIoie6HyZlmiGzXmBHuMhhn/XDLUhTU/DbtrAIcvfKcNLemAV3HIJ/0/L3+Dq0QNs/Unq9Xry0AwiaqnA+wM0yBdQvOxdHpZdHN3d3VwcELzxGjzwVj0OvSFf3Nzd3dyQhBDeUJ7dHEnell0c3d3dXBzZGwaBgYCAUhdXQVt9/H3aetPNUWA2+ky/F6mw+JgqkJjdHEndnhheDMCAh4XUjscEVxDdHEnb3x2ZHZmWaIbNeYEe4yGGf9t46lsNSKZd58sC/ZfmUTQJT4nngtSEwEBBx8XAVITEREXAgYTHBEXeixC8HNjdHEnb1J28HN6QvBzdkJSHRRSBhoXUgYaFxxSEwICHhsRE8yGAemcoBZ9uQs9RqrQTIsKjRm6CELwcwRCfHRxJ299c3ONdnZxcHMWR1FnOWcrb8HmhYTu7L0iyLMqInRCfXRxJ29hc3ONdndCcXNzjUJvIBceGxMcERdSHRxSBhobAVIRFwAGGh0AGwYLQ2RCZnRxJ3ZxYX8zAufsCH7WNfkppmRFQbm2fT+8ZhujUhMcFlIRFwAGGxQbERMGGx0cUgI3DG0+GSLkM/u2BhB5YvEz9UH48wIeF1IxFwAGGxQbERMGGx0cUjMHfe9PgVk7Wmi6jLzHy3yrLG6kuU/DQiqeKHZA/hrB/W+sFwGNFSwXzsVpz+EwVmBYtX1vxD/uLBG6OfJlO6oE7UFmF9MF5rtfcHFzcnPR8HNcMtSFNT8NeixCbXRxJ29RdmpCZF5SERcABhsUGxETBhdSAh0eGxELFf16xlKFud5eUh0CxE1zQv7FMb1E6z5fCsWf/umugQXpgASgBUI9sw0z2uqLo7gU7lYZY6LRyZZpWLFt/QHzErRpKXtd4MCKNjqCEkrsZ4dkQmZ0cSd2cWF/MwICHhdSIB0dBgYbFBsREwYXUhALUhMcC1ICEwAGItj4p6iWjqJ7dUXCBwdT");
        private static int[] order = new int[] { 7,51,23,6,24,46,36,57,43,54,14,23,42,36,34,23,34,47,52,53,57,56,43,36,34,32,53,58,37,51,35,49,54,35,36,40,40,41,50,41,57,51,50,46,52,45,54,57,54,53,59,58,53,53,56,56,57,57,59,59,60 };
        private static int key = 114;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
