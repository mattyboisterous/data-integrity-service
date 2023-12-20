using DataIntegrityService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrityService.Console.Models
{
  internal class UserProfile : IUserProfile
  {
    public UserProfile() 
    {
      UserId = Guid.NewGuid().ToString();
      AccessToken = Guid.NewGuid().ToString();
      IdentityToken = Guid.NewGuid().ToString();
      RefreshToken = Guid.NewGuid().ToString();
    }

    public string UserId { get; set; }
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiry { get; set; }
    public string IdentityToken { get; set; }
    public DateTime IdentityTokenExpiry { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
  }
}
