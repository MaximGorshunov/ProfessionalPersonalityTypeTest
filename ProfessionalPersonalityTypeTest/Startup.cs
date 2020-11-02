using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DBRepository;
using DBRepository.IRepositories;
using DBRepository.Repositories;
using Service.IServices;
using Service.Services;
using Microsoft.Extensions.Configuration;
using ProfessionalPersonalityTypeTest.Helpers;

namespace ProfessionalPersonalityTypeTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserResultRepository, UserResultRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IProfessionRepository, ProfessionRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserResultService, UserResultService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IProfessionService, ProfessionService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<JwtMiddleware>();
            app.UseRouting();
            app.UseMvc();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
