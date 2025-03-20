using FinanceManagerApi.Models.Response;
using FinanceManagerApi.Util;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FinanceManagerApi.Services.FieldValidationService
{
    public class FieldValidationService
    {
        public static FieldValidator<TRequest> Create<TRequest>(TRequest request) where TRequest : class
        {
            return new FieldValidator<TRequest>(request);
        }
    }

    public class FieldValidator<TSource> where TSource : class
    {
        private readonly TSource _request;

        public FieldValidator(TSource request)
        {
            _request = request;
        }

        private readonly List<string> _validationErrors = new();

        public bool Any() => _validationErrors.Any();

        public BadRequestObjectResult BadRequest()
        {
            return new BadRequestObjectResult(new BadRequestDto { Message = "Missing parameters", Errors = _validationErrors });
        }


        public FieldValidator<TSource> FieldIsRequired<TValue>(Expression<Func<TSource, TValue?>> propertySelector)
        {
            var value = propertySelector.Compile().Invoke(_request);
            var propertyInfo = Helpers.GetPropertyInfo(propertySelector);

            if (propertyInfo.PropertyType == typeof(string))
            {
                if (value == null || string.IsNullOrWhiteSpace(value as string))
                {
                    if (!_validationErrors.Contains($"Field {propertyInfo.Name} is required"))
                    {
                        AddErrorFieldIsRequired(propertyInfo);
                    }
                }
            }
            else
            {
                if (value == null)
                {
                    AddErrorFieldIsRequired(propertyInfo);
                }
            }

            return this;
        }

        public FieldValidator<TSource> FieldHasMaxLength(Expression<Func<TSource, string?>> propertySelector, int maxLength)
        {
            var value = propertySelector.Compile().Invoke(_request);
            var propertyInfo = Helpers.GetPropertyInfo(propertySelector);

            if (value == null || string.IsNullOrWhiteSpace(value))
            {
                if (!_validationErrors.Contains($"Field {propertyInfo.Name} is required"))
                {
                    AddErrorFieldIsRequired(propertyInfo);
                }
            }
            else if (value!.Length > maxLength)
            {
                AddErrorFieldHasMaxLength(propertyInfo, maxLength);
            }

            return this;
        }

        public FieldValidator<TSource> FieldHasValidEmailFormat(Expression<Func<TSource, string?>> propertySelector)
        {
            var value = propertySelector.Compile().Invoke(_request);
            var propertyInfo = Helpers.GetPropertyInfo(propertySelector);
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            try
            {
                if (value == null || string.IsNullOrWhiteSpace(value))
                {
                    if (!_validationErrors.Contains($"Field {propertyInfo.Name} is required"))
                    {
                        AddErrorFieldIsRequired(propertyInfo);
                    }
                }
                else if (!Regex.IsMatch(value!, pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                {
                    AddErrorFieldHasInvalidEmailFormat(propertyInfo);
                }

            }
            catch (RegexMatchTimeoutException)
            {
                AddErrorFieldHasInvalidEmailFormat(propertyInfo);
            }

            return this;
        }

        private void AddErrorFieldIsRequired(PropertyInfo propertyInfo)
        {
            _validationErrors.Add($"Field {propertyInfo.Name} is required");
        }

        private void AddErrorFieldHasMaxLength(PropertyInfo propertyInfo, int maxLength)
        {
            _validationErrors.Add($"Field {propertyInfo.Name} has max length {maxLength} characters");
        }

        private void AddErrorFieldHasInvalidEmailFormat(PropertyInfo propertyInfo)
        {
            _validationErrors.Add($"Field {propertyInfo.Name} requires correct email format");
        }
    }
}
