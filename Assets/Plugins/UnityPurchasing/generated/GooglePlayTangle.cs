#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("558jeOn2JML473Yd17vWtR1jRbMcmKtQAEFqMOupsWu0e0qGYuEYKw241XnTsTmOpqUlQCQk9y0SJJh3lCalhpSpoq2OIuwiU6mlpaWhpKdIorw8AsNcEtkDqE/A7U+KWiRzPhMG5vS4x6Dnp+JK4LxSuHhqm5G4DwYwxfB71db8z6TQytiQvjEcv0UBV73CBFMVn+oenLC+imCs/GYr3NfGH34yHaEjImavVMsm69cMuklIJqWrpJQmpa6mJqWlpCPjPHhJNLqmnOdtQv7uc6YAy7Za5EVNzqv76lrEMgMnhhrpMDYNveS4X0aOmY82eHyu12IBfML1nAzrXg429u8iijvjckOAqIYOocxKKoIKmxpnKlVxe3ix04pcehsMBaanpaSl");
        private static int[] order = new int[] { 3,7,13,3,8,10,6,12,8,12,11,12,13,13,14 };
        private static int key = 164;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
