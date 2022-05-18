namespace Api.Utilitaries
{
    public static class DataValidator
    {
        public static bool IsMailAddressValid(string mailAddress)
        {
            return !string.IsNullOrWhiteSpace(mailAddress); // && un nugget de validation d'addresse mail
        }

        public static bool IsPasswordValid(string password)
        {
            return !string.IsNullOrEmpty(password); // && une convention à définir
        }
    }
}
