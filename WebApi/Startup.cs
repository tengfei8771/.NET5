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
                    //.AllowAnyOrigin()//2.0写法 升级之后必须加入参数new string[]指定信任的域+端口
                    .SetIsOriginAllowed(_ => true)//3.0新写法
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                });
            });//注册跨域
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddConsul();
            //启用内存缓存(该步骤需在AddSession()调用前使用)
            services.AddDistributedMemoryCache();//启用session之前必须先添加内存
            //services.AddSession();
            services.AddSession(options =>
            {
                options.Cookie.Name = "WebApi.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(2000);//设置session的过期时间
                                                                 //options.Cookie.HttpOnly = true;//设置在浏览器不能通过js获得该cookie的值

            });
            services.AddSwaggerGen(options =>
            {
                // 添加文档信息
                options.SwaggerDoc("base_api", new OpenApiInfo { Title = "基础框架API", Version = "v1" });
                options.SwaggerDoc("app_api", new OpenApiInfo { Title = "业务API", Version = "v2" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // 获取xml文件路径
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
                // 添加控制器层注释，true表示显示控制器注释
                options.IncludeXmlComments(xmlPath, true);
                options.AddSecurityDefinition("Bear", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token(不需要输入前缀)",
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
                // 不使用驼峰
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            services.AddAutoMapper();
            //services.AddQuartz(typeof(StandardsJob));

            //DiagnosticListener.AllListeners.Subscribe(new CommandListener());//EF监听执行sql和读写分离的注入方法
            //services.AddContext();//EF注入
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
                    c.SwaggerEndpoint("/swagger/app_api/swagger.json", "业务api");
                    c.SwaggerEndpoint("/swagger/base_api/swagger.json", "基础框架api");
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
            builder.RegisterAssemblyTypes(Assembly.Load("Services"))//注册服务层所有的服务类和其对应的接口
                .Where(x => x.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase)).AsImplementedInterfaces();
            builder.RegisterType<BaseMethod>().As<IBaseMethod>().AsImplementedInterfaces();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().AsImplementedInterfaces();
        }
    }
}
