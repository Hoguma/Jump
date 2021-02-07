#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("BmiO329lVyB2GDcuK301CywwGD6dEoXcJyYouiOZCT4GXP0UJfNKPnGPLSFUP2h+OTZc+5+jCxNvi/1HCEdOCFxATQhcQE1GCElYWERBS0k3rautM7EVbx/agbNopgT8mbg68ERNCGFGSwYZDhgMLit9LCM7NWlYmRhwxHIsGqRAm6c19k1b1092TZQlLiECrmCu3yUpKS0tKCuqKSkodBiqLJMYqiuLiCsqKSoqKSoYJS4hDhgMLit9LCM7NWlYWERNCGtNWlxGTAhLR0ZMQVxBR0ZbCEdOCF1bTR0aGRwYGx5yPyUbHRgaGBEaGRwYGx5yGEoZIxghLit9LC47Kn17GTsECEtNWlxBTkFLSVxNCFhHREFLUUwdCz1jPXE1m7zf3rS253iS6XB4ltxbs8b6TCfjUWcc8IoW0VDXQ+BSGKopXhgmLit9NScpKdcsLCsqKS0oK6opJygYqikiKqopKSjMuYEhDMrD+Z9Y9ydtyQ/i2UVQxc+dPz8VDk8IohtC3yWq5/bDiwfRe0JzTOExWt11Jv1Xd7PaDSuSfadldSXZQU5BS0lcQUdGCGldXEBHWkFcURnxHlfpr33xj7GRGmrT8P1ZtlaJeje58zZveMMtxXZRrAXDHop/ZH3EPhg8Lit9LCs7JWlYWERNCHpHR1yA9FYKHeIN/fEn/kP8igwLOd+JhFEISVtbXUVNWwhJS0tNWFxJRktNL8RVEaujewj7EOyZl7JnIkPXA9QHGKnrLiADLiktLS8qKhipnjKpmye1FdsDYQAy4NbmnZEm8XY0/uMVYfBetxs8TYlfvOEFKispKCmLqilcQEdaQVxRGT4YPC4rfSwrOyVpWHpNREFJRktNCEdGCFxAQVsIS01aqDwD+EFvvF4h1txDpQZojt9vZVefM5W7agw6Au8nNZ5ltHZL4GOoP+hLG1/fEi8EfsPyJwkm8pJbMWedLC47Kn17GTsYOS4rfSwiOyJpWFgCrmCu3yUpKS0tKBhKGSMYIS4rfQhraRiqKQoYJS4hAq5grt8lKSkpCElGTAhLTVpcQU5BS0lcQUdGCFhaSUtcQUtNCFtcSVxNRU1GXFsGGC4rfTUmLD4sPAP4QW+8XiHW3EOlp1upSO4zcyEHuprQbGDYSBC2Pd1tVjdkQ3i+aaHsXEojOKtprxuiqVhETQh6R0dcCGtpGDY/JRgeGBwaT6cgnAjf44QECEdYnhcpGKSfa+cgAy4pLS0vKik+NkBcXFhbEgcHX722UiSMb6Nz/D4fG+PsJ2XmPEH5IHYYqik5Lit9NQgsqikgGKopLBhYRE0Ia01aXEFOQUtJXEFHRghpXV9fBklYWERNBktHRQdJWFhETUtJV2mAsNH54k60DEM5+IuTzDMC6zdKRE0IW1xJRkxJWkwIXE1aRVsISVxBTkFLSVxNCEpRCElGUQhYSVpcozGh9tFjRN0vgwoYKsAwFtB4IfuqKSguIQKuYK7fS0wtKRip2hgCLh6xZAVQn8Wks/TbX7PaXvpfGGfpg4tZum97femHB2mb0NPLWOXOi2QuGCcuK301Oykp1ywtGCspKdcYNRg5Lit9LCI7ImlYWERNCGFGSwYZeIKi/fLM1PghLx+YXV0J");
        private static int[] order = new int[] { 31,23,52,41,49,41,26,47,31,10,43,47,31,44,20,50,28,20,53,56,56,47,58,50,31,54,40,51,37,58,51,41,38,38,59,57,52,50,44,45,46,51,55,54,46,47,57,53,57,49,52,56,55,53,55,59,57,57,58,59,60 };
        private static int key = 40;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
