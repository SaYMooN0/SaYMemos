﻿using SaYMemos.Models.data.entities.memos;

namespace SaYMemos.Models.view_models
{
    public record OneMemoViewModel(
        long authorId,
        string authorProfilePicture,
        string authorNickName,
        string authorComment,
        string? imagePath,
        string creationDate,
        string[] tags
        //string likesCount,
        //guid commentsId
        )
    {
        public static OneMemoViewModel FromMemo(Memo memo) => new(
            memo.Author.Id,
            memo.Author.ProfilePicturePath,
            memo.Author.Nickname,
            memo.authorComment,
            memo.imagePath,
            memo.creationTime.ToString("f"),
            memo.Tags.Select(i => i.Value).ToArray());
    }
}
