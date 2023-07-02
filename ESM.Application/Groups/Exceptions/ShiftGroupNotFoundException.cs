﻿using ESM.Application.Common.Exceptions.Core;
using ESM.Domain.Entities;

namespace ESM.Application.Groups.Exceptions;

public class ShiftGroupNotFoundException : NotFoundException
{
    public ShiftGroupNotFoundException(string id) : base(nameof(ShiftGroup), id) { }

    public ShiftGroupNotFoundException(Guid id) : base(nameof(ShiftGroup), id) { }
}