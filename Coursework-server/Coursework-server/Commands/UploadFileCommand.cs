using MediatR;

namespace Coursework_server.Commands;

public class UploadFileCommand : IRequest<string>
{
    public IFormFile File { get; }

    public UploadFileCommand(IFormFile file)
    {
        File = file;
    }
}