using MediatR;
using Microsoft.AspNetCore.Http;

namespace Coursework.Core.Commands;

public class UploadFileCommand : IRequest<string>
{
    public IFormFile File { get; }

    public UploadFileCommand(IFormFile file)
    {
        File = file;
    }
}