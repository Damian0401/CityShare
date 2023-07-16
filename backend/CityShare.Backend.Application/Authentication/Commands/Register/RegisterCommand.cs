﻿using CityShare.Backend.Application.Core.Contracts.Authentication.Register;
using CityShare.Backend.Domain.Shared;
using MediatR;

namespace CityShare.Backend.Application.Authentication.Commands.Register;

public record RegisterCommand(RegisterRequest Request) 
    : IRequest<Result<RegisterResponse>>;
