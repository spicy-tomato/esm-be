using ESM.Application.Common.Exceptions.Core;

namespace ESM.Application.Common.Exceptions.Document;

[Serializable]
public class EmptyFileException : BadRequestException
{
    public EmptyFileException() : base("File is empty!") { }
}