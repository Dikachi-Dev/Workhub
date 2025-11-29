namespace Workhub.Domain.Exceptions;

/// <summary>
/// Base exception for all domain-specific exceptions
/// </summary>
public abstract class WorkhubException : Exception
{
    protected WorkhubException(string message) : base(message) { }
    
    protected WorkhubException(string message, Exception innerException) 
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a requested resource is not found
/// </summary>
public class NotFoundException : WorkhubException
{
    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} with identifier '{key}' was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when a validation error occurs
/// </summary>
public class ValidationException : WorkhubException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string field, string error)
        : base($"Validation failed for {field}")
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { error } }
        };
    }
}

/// <summary>
/// Exception thrown when a business rule is violated
/// </summary>
public class BusinessRuleViolationException : WorkhubException
{
    public BusinessRuleViolationException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when a duplicate resource is detected
/// </summary>
public class DuplicateException : WorkhubException
{
    public DuplicateException(string resourceName, object key)
        : base($"{resourceName} with identifier '{key}' already exists.")
    {
    }

    public DuplicateException(string message) : base(message)
    {
    }
}
