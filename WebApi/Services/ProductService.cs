using System;
using System.Threading.Tasks;
using WebApi.DTO.Request;
using WebApi.DTO.Response;
using WebApi.Models;
using WebApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Services
{
    public class ProductService
    {
        private readonly WebApiDBContext _context;
        private readonly IConfiguration _configuration;

        private readonly ILogger<UserService> _logger;

        public ProductService(WebApiDBContext context, IConfiguration configuration, ILogger<UserService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<object> GetAllProducts(ProductFilterRequestDTO filterRequest)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                // Filter nama produk
                if (!string.IsNullOrEmpty(filterRequest.ProductName))
                {
                    query = query.Where(p => p.NameProduct.Contains(filterRequest.ProductName));
                }

                var totalCount = await query.CountAsync();
                var pageSize = filterRequest.PageSize ?? 10;
                var pageNumber = filterRequest.PageNumber ?? 1;
                var products = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                var response = new
                {
                    total = totalCount,
                    data = products.Select(p => new
                    {
                        productId = p.ProductId,
                        nameProduct = p.NameProduct,
                        price = p.Price,
                    }),
                    message = "Berhasil memuat produk",
                    statusCode = StatusCodes.Status200OK,
                    status = "OK"
                };
                _logger.LogInformation("Berhasil memuat produk...");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan server.");
                return new
                {
                    message = "Terjadi kesalahan server. Silakan coba kembali.",
                    statusCode = StatusCodes.Status500InternalServerError,
                    status = "Internal Server Error",
                    error = ex.Message
                };
            }
        }

        public async Task<object> AddProduct(ProductCreateRequestDTO request)
        {
            try
            {
                var newProduct = new Product
                {
                    NameProduct = request.NameProduct,
                    Price = request.Price,
                    CreatedTime = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                    ModifiedTime = DateTime.Now
                };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                var response = new
                {
                    productId = newProduct.ProductId,
                    nameProduct = newProduct.NameProduct,
                    price = newProduct.Price,
                    message = "Produk berhasil ditambahkan",
                    statusCode = StatusCodes.Status201Created,
                    status = "Created"
                };

                _logger.LogInformation("Produk berhasil ditambahkan: {ProductName}", newProduct.NameProduct);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat menambahkan produk.");
                return new
                {
                    message = "Terjadi kesalahan server. Silakan coba kembali.",
                    statusCode = StatusCodes.Status500InternalServerError,
                    status = "Internal Server Error",
                    error = ex.Message
                };
            }
        }


        public async Task<object> UpdateProduct(int productId, ProductUpdateRequestDTO request)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return new
                    {
                        message = "Produk tidak ditemukan",
                        statusCode = StatusCodes.Status404NotFound,
                        status = "Not Found"
                    };
                }

                product.NameProduct = request.NameProduct;
                product.Price = request.Price;
                product.ModifiedTime = DateTime.Now;
                product.ModifiedBy = "Admin";


                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                var response = new
                {
                    productId = product.ProductId,
                    nameProduct = product.NameProduct,
                    price = product.Price,
                    message = "Produk berhasil diperbarui",
                    statusCode = StatusCodes.Status200OK,
                    status = "OK"
                };

                _logger.LogInformation("Produk berhasil diperbarui: {ProductName}", product.NameProduct);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat memperbarui produk.");
                return new
                {
                    message = "Terjadi kesalahan server. Silakan coba kembali.",
                    statusCode = StatusCodes.Status500InternalServerError,
                    status = "Internal Server Error",
                    error = ex.Message
                };
            }
        }

        public async Task<object> DeleteProduct(int productId)
        {
            try
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    return new
                    {
                        message = "Produk tidak ditemukan",
                        statusCode = StatusCodes.Status404NotFound,
                        status = "Not Found"
                    };
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                var response = new
                {
                    message = "Produk berhasil dihapus",
                    statusCode = StatusCodes.Status200OK,
                    status = "OK"
                };

                _logger.LogInformation("Produk berhasil dihapus: {ProductName}", product.NameProduct);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat menghapus produk.");
                return new
                {
                    message = "Terjadi kesalahan server. Silakan coba kembali.",
                    statusCode = StatusCodes.Status500InternalServerError,
                    status = "Internal Server Error",
                    error = ex.Message
                };
            }
        }
    }


}

