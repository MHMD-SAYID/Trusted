
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Trusted.Core.Abstraction;

public class Result<T>
{


    public bool IsSuccess { get; set; }


    public string ArRes { get; set; }


    public string EnRes { get; set; }
    public T Data { get; set; }

    public int StatusCode { get; set; }

    public List<string> Errors { get; set; } = new List<string>();
}
