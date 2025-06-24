﻿namespace Web.Api.Features.Common;

public record CustomerDto(string Id,
        string FirstName,
        string LastName,
        string Email,
        string? IdentificationNumber,
        DateOnly? BirthDate,
        string PhoneNumber,
        string Status);
