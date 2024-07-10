namespace BookStore;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }

    public ApiResponse(T data)
    {
        this.Data = data;
        this.Success = true;
        this.Message = string.Empty;
    }
    
    public ApiResponse(string error)
    {
        this.Success = false;
        this.Message = error;
    }
}