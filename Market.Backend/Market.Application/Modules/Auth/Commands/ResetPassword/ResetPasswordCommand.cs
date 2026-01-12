using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace Market.Application.Modules.Auth.Commands.ResetPassword;

public sealed class ResetPasswordCommand : IRequest
{
    public string Token { get; init; } = null!;
    public string NewPassword { get; init; } = null!;
}

