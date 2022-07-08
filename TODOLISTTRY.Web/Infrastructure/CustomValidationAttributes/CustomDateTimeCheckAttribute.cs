using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using TODOLISTTRY.Web.Services;

namespace TODOLISTTRY.Web.Infrastructure.CustomValidationAttributes
{
    /// <summary>
    /// Проверка запланированной даты
    /// </summary>
    public class CustomDateTimeCheckAttribute : ValidationAttribute
    {
        private DateTime _minDateTime;
        private DateTime _maxDateTime;

        public CustomDateTimeCheckAttribute(string minDateTime,string maxDateTime)
        {
            _minDateTime = Convert.ToDateTime(minDateTime);
            _maxDateTime = Convert.ToDateTime(maxDateTime);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime propValue = Convert.ToDateTime(value);

            if (propValue.Date > _minDateTime && propValue.Date < _maxDateTime)
            {
                return ValidationResult.Success;
            }

            var errorString = GetErrorMessage(validationContext);

            errorString = errorString.Replace("{1}",$"{_minDateTime.ToShortDateString()}");
            errorString = errorString.Replace("{2}",$"{_maxDateTime.ToShortDateString()}");

            return new ValidationResult(errorString);
        }

        /// <summary>
        /// Получение локализованной ошибки через общие ресурсы
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        private string GetErrorMessage(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                return "Invalid SharedResources string";
            }

            ErrorMessageTranslationService errorTranslation = validationContext.GetService(typeof(ErrorMessageTranslationService)) as ErrorMessageTranslationService;

            return errorTranslation.GetLocalizedError(ErrorMessage);
        }
    }
}
