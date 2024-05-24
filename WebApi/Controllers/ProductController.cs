using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.Request;
using WebApi.Models;
using WebApi.Data;
using System;
using System.Threading.Tasks;
using WebApi.DTO;
using Microsoft.AspNetCore.Http;
using WebApi.Services;

namespace WebApi.Controllers;

[Route("api/products/")]
[ApiController]
public class ProductController : ControllerBase
{

    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }
    // GET: products/data
    [HttpGet("data")]
    public async Task<IActionResult> AllFoods([FromQuery] ProductFilterRequestDTO filterRequest)
    {
        try
        {
            var response = await _productService.GetAllProducts(filterRequest);

            return StatusCode(StatusCodes.Status200OK, response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    // POST: products/add
    [HttpPost("add")]
    public async Task<IActionResult> AddProduct([FromBody] ProductCreateRequestDTO request)
    {
        try
        {
            var response = await _productService.AddProduct(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }


    // PUT: products/edit/{productId}
    [HttpPut("edit/{productId}")]
    public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductUpdateRequestDTO request)
    {
        try
        {
            var response = await _productService.UpdateProduct(productId, request);
            return StatusCode(StatusCodes.Status200OK, response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }


    [HttpDelete("delete/{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        try
        {
            var response = await _productService.DeleteProduct(productId);
            return StatusCode(StatusCodes.Status200OK, response);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }



}