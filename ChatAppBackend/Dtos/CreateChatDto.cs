namespace ChatAppBackend.Dtos
{
    public sealed record CreateChatDto(
        string ChatName,
        Guid CreatorId,
        Guid[] members,
        Boolean isPublic
        );
}
