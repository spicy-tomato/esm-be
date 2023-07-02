using ESM.Application.Common.Exceptions.Core;
using JetBrains.Annotations;

namespace ESM.Application.Common.Exceptions.Document;

[Serializable]
[UsedImplicitly]
public class EmptyFileException : BadRequestException
{
    public EmptyFileException() : base("File is empty!") { }
}