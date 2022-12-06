namespace SimpleWebApp.Core.Helpers;

public class CategoryNotFoundException : Exception
{
    public CategoryNotFoundException()
    {
    }

    public CategoryNotFoundException(string message)
        : base(message)
    {
    }

    public CategoryNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}

public class SupplierNotFoundException : Exception
{
    public SupplierNotFoundException()
    {
    }

    public SupplierNotFoundException(string message)
        : base(message)
    {
    }

    public SupplierNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
public class ProductNotFoundException : Exception
{
    public ProductNotFoundException()
    {
    }

    public ProductNotFoundException(string message)
        : base(message)
    {
    }

    public ProductNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}