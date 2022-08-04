﻿using System.ComponentModel.DataAnnotations;

namespace Api.ExpositionModels;

/**
 * Resource classes
 */
public class AccountResource
{
    public string MailAddress { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
    public string PicUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AccountResource_WithPosts : AccountResource
{
    public ICollection<PostResource> OwnedPosts { get; set; }
    public ICollection<PostResource> LikedPosts { get; set; }
    public ICollection<CommentaryResource> LikedCommentaries { get; set; }
    public ICollection<CommentaryResource> OwnedCommentaries { get; set; }
}

public class AccountResource_WithPosts_AndGroups : AccountResource_WithPosts
{
    public ICollection<GroupResource> Groups { get; set; }
}

public class AccountResource_WithPosts_AndGroups_AndFollows : AccountResource_WithPosts_AndGroups
{
    public ICollection<AccountResource> Follows { get; set; }
}

/**
 * Request classes
 */
public class CreateAccountRequest
{
    [Required] public string MailAddress { get; set; }
    [Required] public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
    public byte[] ProfilPic { get; set; }
}

public class ReadByNames
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class Authentication
{
    [Required] public string MailAddress { get; set; }
    [Required] public string Password { get; set; }
}

public class UpdateProfil
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
}

public class UpdateProfilPic
{
    public byte[] ProfilPic { get; set; }
}