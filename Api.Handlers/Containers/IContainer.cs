namespace Api.Handlers.Containers;

public interface IContainer
{
    public Task<string> GetProfilPicUrl(byte[] profilPic, string mailAddress);
    public Task<string> GetGroupPicUrl(byte[] groupPic, Guid groupId);
    public Task<string> GetMusicUrl(byte[] sound, Guid postId, string filename);
    public Task<string> GetPipelineSoundUrl(byte[] sound, string filename);
    public Task<string> GetPostScriptUrl(string script, Guid postId);
    public Task<string> GetScriptContent(Guid scriptId);
    public Task<string> GetConvertedSoundUrl(byte[] sound, string filename);

    /// <summary>
    /// Unused atm, maybe usefull later
    /// </summary>
    public Task<string> GetPipelineImageUrl(byte[] image, string filename);

    /*
     * Used to clean AWS containers after testing
     */
    public Task DeletePostScript(Guid postId);
    public Task DeletePostSound(string file);
    public Task DeleteAccountPic(string mailAddress);
    public Task DeleteGroupPic(Guid groupId);
}
