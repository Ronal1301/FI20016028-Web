using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BinaryCalc.Models.Attributes;

namespace BinaryCalc.Models
{
    public class BinaryViewModel
    {
        [Display(Name = "a")]
        [BinaryString(ErrorMessage = "Valor inválido para a.")]
        public string A { get; set; } = string.Empty;

        [Display(Name = "b")]
        [BinaryString(ErrorMessage = "Valor inválido para b.")]
        public string B { get; set; } = string.Empty;

        public List<ResultRow> Results { get; set; } = new();

        public string A8 => BinaryCalculator.PadTo8(A);
        public string B8 => BinaryCalculator.PadTo8(B);

        public bool HasResults => Results?.Count > 0;
    }

    public class ResultRow
    {
        public string Label { get; set; } = string.Empty;
        public string Bin   { get; set; } = string.Empty;
        public string Oct   { get; set; } = string.Empty;
        public string Dec   { get; set; } = string.Empty;
        public string Hex   { get; set; } = string.Empty;
    }
}
