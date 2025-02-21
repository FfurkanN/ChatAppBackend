namespace ChatAppBackend.Dtos
{
    public sealed record CreateChatDto(
        string ChatName,
        List<Guid> members,
        Boolean isPublic
        );
}
