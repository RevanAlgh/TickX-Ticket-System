using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;
using Ticket.Data.Models.Enums;
using Ticket.Data.Models;
using Ticket.Data.Models.Data;
using Ticket.Service.Models;
using Microsoft.Extensions.Logging;

namespace Ticket.Service.Repositories 
{ 

    public class ProductService : IProductService
    { 
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;
        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductAsync(CreateProductDTO dto)
        {
            _logger.LogInformation($"AddProductAsync: Request Body {dto}");
            if (await _context.Products.AnyAsync(p => p.Name == dto.Name))
            {
                _logger.LogError("A product with the same name already exists.");
                throw new InvalidOperationException("A product with the same name already exists.");
            }

            var product = new Product
            {
                Name = dto.Name
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ProductDTO> GetAllProductsAsync()
        {
            _logger.LogInformation("GetAllProductsAsync");
            if (!_context.Products.Any())
            {
                _logger.LogError("There is no products.");
                throw new InvalidOperationException("There is no products.");
            }

            return _context.Products
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    Name = p.Name
                }).ToList();
            
        }

    }
}
