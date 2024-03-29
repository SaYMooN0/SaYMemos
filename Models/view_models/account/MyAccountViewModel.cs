﻿using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.view_models.memos;

namespace SaYMemos.Models.view_models.account
{
    public record MyAccountViewModel(
       string nickname,
       string profilePicturePath,
       Dictionary<string, string> links,
       string fullName,
       MemoPreviewViewModel[] memos
       )
    {
        public static MyAccountViewModel FromUser(User user)
        {
            var likedMemoIds = user.Likes.Select(l => l.MemoId).ToHashSet();

            return new MyAccountViewModel(
                user.Nickname,
                user.ProfilePicturePath,
                user.UserLinks.ParseToNonEmptyDictionary(),
                user.FullName,
                user.PostedMemos
                    .Select(memo => MemoPreviewViewModel.FromMemo(memo, user))
                    .OrderByDescending(memo => memo.creationDate)
                    .ToArray()
            );
        }


    }

}
