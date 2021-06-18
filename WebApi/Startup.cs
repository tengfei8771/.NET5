using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PublicWebApi.Common.Validator;
using SqlSugarAndEntity;
using SqlSugarAndEntity.AutoMapperConfig;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TimedTask;
using Utils;
using Utils.SerilogConfig;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder
                    //.AllowAnyOrigin()//2.0д�� ����֮�����������new string[]ָ�����ε���+�˿�
                    .SetIsOriginAllowed(_ => true)//3.0��д��
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });//ע�����
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddConsul();
            //�����ڴ滺��(�ò�������AddSession()����ǰʹ��)
            services.AddDistributedMemoryCache();//����session֮ǰ����������ڴ�
            //services.AddSession();
            services.AddSession(options =>
            {
                options.Cookie.Name = "WebApi.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(2000);//����session�Ĺ���ʱ��
                                                                 //options.Cookie.HttpOnly = true;//���������������ͨ��js��ø�cookie��ֵ

            });
            services.AddSwaggerGen(options =>
            {
                // ����ĵ���Ϣ
                options.SwaggerDoc("base_api", new OpenApiInfo { Title = "�������API", Version = "v1" });
                options.SwaggerDoc("app_api", new OpenApiInfo { Title = "ҵ��API", Version = "v2" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // ��ȡxml�ļ�·��
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if(!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }
                    else
                    {
                        var version = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(t => t.GroupName).FirstOrDefault();
                        if(docName.ToLower()== "app_api" && version == null)
                        {
                            return true;
                        }
                        else
                        {
                            return version == docName.ToLower();
                        }
                        
                    }
                    

                });
                // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                options.IncludeXmlComments(xmlPath, true);
                options.AddSecurityDefinition("Bear", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken(����Ҫ����ǰ׺)",
                    Name = "Bear",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bear"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bear"}
                           },new string[] { }
                        }
                    });
            });
            services.AddControllers(options=>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            })
                .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // ��ʹ���շ�
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // ����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            services.AddAutoMapper();
            //services.AddQuartz(typeof(StandardsJob));

            //DiagnosticListener.AllListeners.Subscribe(new CommandListener());//EF����ִ��sql�Ͷ�д�����ע�뷽��
            //services.AddContext();//EFע��
            //services.AddDbContext<Entity.Models.AppDBContext>(options => options.UseSqlServer("server=localhost;user id=sa;pwd=sa;database=AppDB"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c => 
                {
                    c.SwaggerEndpoint("/swagger/app_api/swagger.json", "ҵ��api");
                    c.SwaggerEndpoint("/swagger/base_api/swagger.json", "�������api");
                });
            }
            //app.UseSerilogRequestLoggings();
            app.UseCors("any");
            //app.UseConsul();
            //QuartzServices.StartJobs<StandardsJob>();
            app.UseHttpsRedirection();
            
            app.UseRouting();

            //app.UseAuthorization();
            //app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Repository"))
                .Where(x => x.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(Assembly.Load("Services"))//ע���������еķ���������Ӧ�Ľӿ�
                .Where(x => x.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            builder.RegisterType<BaseMethod>().As<IBaseMethod>().AsImplementedInterfaces();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().AsImplementedInterfaces();
        }
    }
}
