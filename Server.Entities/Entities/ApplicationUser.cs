﻿using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace Server.Entities.Entities;
public class ApplicationUser : IdentityUser
{
    public UserDetail Detail { get; set; } 
}
