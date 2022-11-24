using MemberClaim.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Claim = System.Security.Claims.Claim;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CaseStudyContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("dbConn")));
builder.Services.AddSingleton<ITokenService>(new TokenService());
builder.Services.AddCors((setup) =>
{
setup.AddPolicy("default", (options) =>
{
options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
});
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
app.UseSwagger();
app.UseSwaggerUI();
}
app.MapPost("/validate", object (UserValidationRequestModel request, HttpContext http, ITokenService tokenService) =>
{
var member = request.IsValidate(request.Name, request.Password);
if (member != null)
{
var token = tokenService.BuildToken(builder.Configuration["Jwt:Key"],
                                        builder.Configuration["Jwt:Issuer"],
                                        new[]
                                        {
                                                        builder.Configuration["Jwt:Aud1"]

                                                        },
                                        request.Name);
return new
{
Token = token,
Member = member,
IsAuthenticated = true,
};
}
return new
{
Token = string.Empty,
Member = member,
IsAuthenticated = false
};
})
.WithName("Validate");
app.UseCors("default");
await app.RunAsync();
internal interface ITokenService
{
    string BuildToken(string key, string issuer, IEnumerable<string> audience, string userName);
}
internal class TokenService : ITokenService
{
    private TimeSpan ExpiryDuration = new TimeSpan(20, 30, 0);
    public string BuildToken(string key, string issuer, IEnumerable<string> audience, string userName)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
        };

        claims.AddRange(audience.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
            expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
