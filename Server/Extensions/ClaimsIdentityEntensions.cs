﻿using Duende.IdentityServer.EntityFramework.Entities;
using Google.Apis.PeopleService.v1.Data;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlazorAuth.Server.Extensions;

public static class ClaimsIdentityEntensions
{
    public static void AddClaim(this ClaimsIdentity claimsIdentity, string type, string value)
    {
        if (claimsIdentity.HasClaim(type))
        {
            claimsIdentity.RemoveClaim(claimsIdentity.GetClaim(type));
        }

        claimsIdentity.AddClaim(new Claim(type, value));
    }

    public static void TryAddClaim(this ClaimsIdentity claimsIdentity, string type, string value)
    {
        if (!string.IsNullOrEmpty(type) && !string.IsNullOrWhiteSpace(value))
        {
            claimsIdentity.AddClaim(type, value);
        }
    }

    public static void TryAddClaim(this ClaimsIdentity claimsIdentity, string type, Date date)
    {
        if (date?.Year.HasValue == true && date.Month.HasValue && date.Day.HasValue)
        {
            var dateStr = $"{date?.Year}-{(date.Month.ToString().Length == 1 ? $"0{date.Month}" : date.Month)}-{(date.Day.ToString().Length == 1 ? $"0{date.Day}" : date.Day)}";
            if (dateStr.TryDateParse(out DateTime? value) && value.AgeIsValid(18))
            {
                claimsIdentity.AddClaim(type, dateStr);
            }
        }
    }

    public static Claim GetClaim(this ClaimsIdentity claimsIdentity, string type)
    {
        return claimsIdentity.FindFirst(x => x.Type == type);
    }

    public static bool HasClaim(this ClaimsIdentity claimsIdentity, string type)
    {
        return !string.IsNullOrWhiteSpace(type) && claimsIdentity?.HasClaim(x => x.Type == type) == true;
    }

    public static void AddGoogleConfigClaims(this ClaimsIdentity claimsIdentity, Person person)
    {
        var gender = person.Genders?.FirstOrDefault()?.FormattedValue;
        var birthDate = person.Birthdays?.FirstOrDefault(x => x.Date.Year.HasValue && x.Date.Month.HasValue && x.Date.Day.HasValue)?.Date;

        claimsIdentity.TryAddClaim(JwtClaimTypes.Gender, gender);
        claimsIdentity.TryAddClaim(JwtClaimTypes.BirthDate, birthDate);

        var index = 0;
        foreach (var phone in person.PhoneNumbers ?? new List<PhoneNumber>())
        {
            claimsIdentity.AddClaim(index == 0 ? JwtClaimTypes.PhoneNumber : $"{JwtClaimTypes.PhoneNumber}{index}", phone.Value);
            index++;
        }
    }

    public static string GetClaim(this ExternalLoginInfo info, string type)
    {
        if (info.Principal.HasClaim(c => c.Type == type))
        {
            return info.Principal.FindFirstValue(type);

        }
        return string.Empty;
    }
}
