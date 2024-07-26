using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Models.Entidades;
using System.Text;

namespace Api.Extensiones
{
    public static class ServicioIdentidadExtension
    {
        public static IServiceCollection AgregarServicioIdentidad(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentityCore<UsuarioAplicacion>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            }).AddRoles<RolAplicacion>().AddRoleManager<RoleManager<RolAplicacion>>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(config["TokenKey"])),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminRol", policy => policy.RequireRole("Admin"));
                opt.AddPolicy("AdminAgendadorRol", policy => policy.RequireRole("Admin", "Agendador"));
                opt.AddPolicy("AdminMedicoRol", policy => policy.RequireRole("Admin", "Medico"));
            });

            return services;
        }
    }
}
