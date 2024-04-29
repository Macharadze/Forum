using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Models
{
    public class ErrorViewModel
    {
        public string Path { get; set; }
        public ProblemDetails ProblemDetails { get; set; }
        public List<string> Errors { get;set; }

    }
}