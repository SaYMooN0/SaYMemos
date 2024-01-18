namespace SaYMemos.Models.data.entities.memos
{
    public record class Comment(
        Guid id, 
        Guid memoId,
        long authorId,
        Guid? parentComment,
        string text

        
        )
    {
    }
}
