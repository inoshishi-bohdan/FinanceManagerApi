﻿using FinanceManagerApi.Entities;
using FinanceManagerApi.Models.ProfileImage;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagerApi.Extensions
{
    public static class ProfileImageExtension
    {
        public static async Task<List<ProfileImageDto>> ToProfileImageDtoListAsync(this IQueryable<ProfileImage> profileImages)
        {
            return await profileImages.Select(profileImage => new ProfileImageDto
            {
                Id = profileImage.Id,
                Path = profileImage.Path,
                Caption = profileImage.Caption
            }).ToListAsync();
        }
    }
}
