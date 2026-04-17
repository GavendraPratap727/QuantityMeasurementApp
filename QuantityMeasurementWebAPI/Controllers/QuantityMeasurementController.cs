using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModelLayer.DTO;
using QuantityMeasurementModelLayer.Exceptions;

namespace QuantityMeasurementWebAPI.Controllers
{
    [Authorize(Roles ="User,admin,user")]
    [ApiController]
    [Route("api/quantitymeasurement")]
    public class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        // Compare two quantities
       
        [AllowAnonymous]
        [HttpPost("compare")]
        public IActionResult Compare([FromBody] CompareRequestDTO request)
        {
            try
            {
                var result = _service.CompareQuantities(request.ThisQuantityDTO, request.ThatQuantityDTO);
                return Ok(new { result });
            }
            catch (UnsupportedOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid unit or measurement type", message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Compare operation error: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error occurred during compare operation", message = ex.Message });
            }
        }

        // Add two quantities
        [AllowAnonymous]
        [HttpPost("add")]
        public IActionResult Add([FromBody] OperationRequestDTO request)
        {
            try
            {
                var result = _service.AddQuantities(request.ThisQuantityDTO, request.ThatQuantityDTO);
                return Ok(result);
            }
            catch (UnsupportedOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid unit or measurement type", message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add operation error: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error occurred during add operation", message = ex.Message });
            }
        }

        // Subtract two quantities
        [AllowAnonymous]
        [HttpPost("subtract")]
        public IActionResult Subtract([FromBody] OperationRequestDTO request)
        {
            try
            {
                var result = _service.SubtractQuantities(request.ThisQuantityDTO, request.ThatQuantityDTO);
                return Ok(result);
            }
            catch (UnsupportedOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid unit or measurement type", message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Subtract operation error: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error occurred during subtract operation", message = ex.Message });
            }
        }

        // Divide two quantities
        [AllowAnonymous]
        [HttpPost("divide")]
        public IActionResult Divide([FromBody] OperationRequestDTO request)
        {
            try
            {
                var result = _service.DivideQuantities(request.ThisQuantityDTO, request.ThatQuantityDTO);
                return Ok(result);
            }
            catch (DivideByZeroException ex)
            {
                return StatusCode(500, new { error = "Divide by zero", message = ex.Message });
            }
            catch (UnsupportedOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid unit or measurement type", message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Divide operation error: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error occurred during divide operation", message = ex.Message });
            }
        }

        // Convert a quantity to target unit
        [AllowAnonymous]
        [HttpPost("convert")]
        public IActionResult Convert([FromBody] ConvertRequestDTO request)
        {
            try
            {
                Console.WriteLine($"Convert request: {request?.QuantityDTO?.Value} {request?.QuantityDTO?.Unit} to {request?.TargetUnit}");
                
                if (request == null || request.QuantityDTO == null)
                {
                    Console.WriteLine("Convert request is null or missing QuantityDTO");
                    return BadRequest("Request data is missing");
                }
                
                var result = _service.ConvertQuantity(request.QuantityDTO, request.TargetUnit);
                Console.WriteLine($"Convert result: {result}");
                return Ok(result);
            }
            catch (UnsupportedOperationException ex)
            {
                Console.WriteLine($"Convert error: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Invalid unit or measurement type", message = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Convert operation error: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error occurred during convert operation", message = ex.Message });
            }
        }

        // Get all operations with data source info
       
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var dataWithSource = _service.GetAllOperationsWithSource(); // Returns (List<QuantityMeasurementEntity>, string source)
            
            return Ok(dataWithSource.data);
        }

        [HttpGet("history/all")]
        public IActionResult GetAllOperations()
        {
            var dataWithSource = _service.GetAllOperationsWithSource(); // Returns (List<QuantityMeasurementEntity>, string source)
            
            return Ok(new 
            { 
                Source = dataWithSource.source, 
                Data = dataWithSource.data 
            });
        }

        // Get all errored operations
        [HttpGet("history/errored")]
        public IActionResult GetErroredHistory()
        {
            var result = _service.GetErroredOperations();
            return Ok(result);
        }

        // Get total count of operations by type
        [HttpGet("count/{operationType}")]
        public IActionResult GetOperationCount(string operationType)
        {
            var count = _service.GetOperationCount(operationType);
            return Ok(new { operationType, count });
        }
    }

    // Request DTO wrappers for clarity in Swagger UI
    public class OperationRequestDTO
    {
        public QuantityDTO ThisQuantityDTO { get; set; } = null!;
        public QuantityDTO ThatQuantityDTO { get; set; } = null!;
    }

    public class CompareRequestDTO : OperationRequestDTO { }

    public class ConvertRequestDTO
    {
        public QuantityDTO QuantityDTO { get; set; } = null!;
        public string TargetUnit { get; set; } = null!;
    }
}