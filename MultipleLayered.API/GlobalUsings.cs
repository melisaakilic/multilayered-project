// Data Access
global using Multiple_Layered_DataAccess.Library.Data;
global using Multiple_Layered_DataAccess.Library.IdentityConfigurations;
global using Multiple_Layered_DataAccess.Library.Seeds;
global using Multiple_Layered_DataAccess.Library.Repositories;
global using Multiple_Layered_DataAccess.Library.UnitOfWork;
global using Multiple_Layered_DataAccess.Library.Models;


// API
global using Multiple_Layered.API;
global using System.Text.Json;
global using Microsoft.EntityFrameworkCore;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Microsoft.OpenApi.Models;
global using System.Security.Claims;


// Extensions
global using Microsoft.Extensions.Options;
global using Multiple_Layered.API.Extensions.Middlewares;
global using Multiple_Layered.API.Extensions.Exceptions;



// Services
global using Multiple_Layered_Service.Library.Dtos.AuthDtos;
global using Multiple_Layered_Service.Library.Services.AuthRepo;
global using Multiple_Layered_Service.Library.Services.AuthServices;
global using Multiple_Layered_Service.Library.Services.UserServices;
global using Multiple_Layered_Service.Library.Dtos.OrderDtos;
global using Multiple_Layered_Service.Library.Paginations;
global using Multiple_Layered_Service.Library.Services.OrderServices;
global using Multiple_Layered_Service.Library.Dtos.OrderProduct;
global using Multiple_Layered_Service.Library.Services.OrderProductServices;
global using Multiple_Layered_Service.Library.Dtos.ProductDtos;
global using Multiple_Layered_Service.Library.Services.ProductServices;
global using Multiple_Layered_Service.Library.Dtos.UserDtos;
global using Multiple_Layered_Service.Library.Services.JwtServices;
global using Multiple_Layered_Service.Library.Services.RoleServices;
global using Multiple_Layered_Service.Library.Dtos.RoleDtos;