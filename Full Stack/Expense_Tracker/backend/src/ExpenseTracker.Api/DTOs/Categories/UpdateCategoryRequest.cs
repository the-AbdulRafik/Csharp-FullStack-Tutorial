using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Api.Enums;

namespace ExpenseTracker.Api.DTOs.Categories;

public class UpdateCategoryRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [EnumDataType(typeof(TransactionType))]
    public TransactionType Type { get; set; }
}
