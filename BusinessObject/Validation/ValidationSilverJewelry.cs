using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessObject.Validation
{
    public class ValidationSilverJewelry : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string artTitle = value.ToString();
                string[] words = artTitle.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    if (!char.IsUpper(word[0]))
                    {
                        return new ValidationResult("Each word in ArtTitle must begin with a capital letter.");
                    }
                    if (word.Any(char.IsDigit))
                    {
                        return new ValidationResult("ArtTitle cannot contain numbers (0-9).");
                    }
                    // Allow specific special characters
                    if (!Regex.IsMatch(word, @"^[A-Za-z0-9]+$"))
                    {
                        return new ValidationResult("SilverJewelryName can only contain letters, numbers, and spaces.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
