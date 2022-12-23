namespace BlazorAuth.Shared;

public static class RegularExpressions
{
    public const string PasswordRegex = @"^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{6,}$";
    public const string NameRegex = @"^[a-zA-ZãáàéêíóõúçÃÁÀÉÊÍÓÕÚÇ\s]+$";
}