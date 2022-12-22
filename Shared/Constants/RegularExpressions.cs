namespace BlazorAuth.Shared;

public static class RegularExpressions
{
    public const string PasswordRegex = @"^(?=.*\d)(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z]).{12,}$";
    public const string NameRegex = @"^[a-zA-ZãáàéêíóõúçÃÁÀÉÊÍÓÕÚÇ\s]+$";
}