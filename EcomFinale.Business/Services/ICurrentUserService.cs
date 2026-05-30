using EcomFinale.Business.Models;
using Microsoft.AspNetCore.Http;

namespace EcomFinale.Business.Services;

public interface ICurrentUserService
{
    JwtClaims? GetCurrentUserClaims();
}