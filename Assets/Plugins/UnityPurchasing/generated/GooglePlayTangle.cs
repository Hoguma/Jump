#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("tlxCwvw9ouwn/VaxPhOxdKTajcDYW1VaathbUFjYW1ta3R3ChrfKROJmVa7+v5TOFVdPlUqFtHicH+bV7fgYCkY5XhlZHLQeQqxGhpRlb0bzRiuHLU/HcFhb277a2gnT7NpmiaQ6zP3ZeOQXzsjzQxpGobhwZ3HIKTjhgMzjX93cmFGqNdgVKfJEt7ZYYhmTvAAQjVj+NUikGruzMFUFFPH4zjsOhSsoAjFaLjQmbkDP4kG7athbeGpXXFNw3BLcrVdbW1tfWlkdjL1+VnjwXzK01Hz0ZeSZ1KuPhYaCUCmc/4I8C2LyFaDwyAgR3HTFGWHdhhcI2jwGEYjjKUUoS+Odu03/qUM8+q3rYRTgYk5AdJ5SApjVIoZPLXSihOXy+1hZW1pb");
        private static int[] order = new int[] { 8,1,7,10,13,9,13,11,13,13,13,11,13,13,14 };
        private static int key = 90;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
