namespace Api.Models.ExpositionModels.Requests;

public class ReadByNamesRequest
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}

public class UpdateProfilRequest
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Pseudonym { get; set; }
}
