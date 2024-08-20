using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticket.Data.Models;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Service.Repositories;

namespace Ticket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductDTO model)
        {
            _logger.LogInformation("Start AddProduct API");
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Status = false,
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            try
            {
                await _productService.AddProductAsync(model);
                return Ok(new ApiResponse<string> { Message = "Product created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = false,
                    Message = "A product with the same name already exists.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            _logger.LogInformation("Start GetAllProducts API");
            try
            {
                var product = _productService.GetAllProductsAsync();
                return Ok(new ApiResponse<IEnumerable<ProductDTO>> { Data = product });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<string>
                {
                    Status = false,
                    Message = "There is no products.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }
}
