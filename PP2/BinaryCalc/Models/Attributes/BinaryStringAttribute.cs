using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BinaryCalc.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BinaryStringAttribute : ValidationAttribute
    {
        public int MaxLength { get; set; } = 8;

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var str = value as string ?? string.Empty;

            // 1) Longitud > 0
            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult("El valor no puede estar vacío.");

            // 2) Solo 0 y 1
            if (str.Any(c => c != '0' && c != '1'))
                return new ValidationResult("Solo se permiten caracteres 0 y 1.");

            // 3) Longitud <= 8
            if (str.Length > MaxLength)
                return new ValidationResult($"La longitud no puede exceder {MaxLength} caracteres.");

            // 4) Longitud múltiplo de 2
            if (str.Length % 2 != 0)
                return new ValidationResult("La longitud debe ser múltiplo de 2 (2, 4, 6 u 8).");

            return ValidationResult.Success!;
        }
    }
}
