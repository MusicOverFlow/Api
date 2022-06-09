using System.Dynamic;
using System.Text.Json;

namespace Api.Utilitaries;

public class ExceptionHandler
{
    public Exception InvalidMail { get => this.GetException(nameof(this.InvalidMail)); }
    public Exception InvalidPassword { get => this.GetException(nameof(this.InvalidPassword)); }
    public Exception MailAlreadyInUse { get => this.GetException(nameof(this.MailAlreadyInUse)); }
    public Exception AccountNotFound { get => this.GetException(nameof(this.AccountNotFound)); }
    public Exception SelfFollow { get => this.GetException(nameof(this.SelfFollow)); }
    public Exception PostOrCommentaryNotFound { get => this.GetException(nameof(this.PostOrCommentaryNotFound)); }
    public Exception InvalidName { get => this.GetException(nameof(this.InvalidName)); }
    public Exception InvalidRole { get => this.GetException(nameof(this.InvalidRole)); }
    public Exception WrongCredentials { get => this.GetException(nameof(this.WrongCredentials)); }
    public Exception GroupeMissingName { get => this.GetException(nameof(this.GroupeMissingName)); }
    public Exception GroupNotFound { get => this.GetException(nameof(this.GroupNotFound)); }
    public Exception AccountAlreadyInGroup { get => this.GetException(nameof(this.AccountAlreadyInGroup)); }
    public Exception LeaveWhileOwner { get => this.GetException(nameof(this.LeaveWhileOwner)); }
    public Exception NotOwnerOfGroup { get => this.GetException(nameof(this.NotOwnerOfGroup)); }
    public Exception AccountNotInGroup { get => this.GetException(nameof(this.AccountNotInGroup)); }
    public Exception PostTitleOrContentEmpty { get => this.GetException(nameof(this.PostTitleOrContentEmpty)); }
    public Exception PostNotFound { get => this.GetException(nameof(this.PostNotFound)); }

    public dynamic Get { get; private set; }

    private Dictionary<string, Exception> exceptions;

    public ExceptionHandler(string exceptionsFilepath)
    {
        this.exceptions = this.ExceptionsRuntimeInitialization(exceptionsFilepath);
    }

    /// <summary>
    /// Qu'est ce qu'il se passe dans cette méthode ? <para/>
    /// La propriété 'Get' est un objet dynamique, dont les propriétés sont crées au runtime en fonction des exceptions contenues
    /// dans le fichier JSON. <br/>
    /// Ce qui veut dire que cet objet dynamique contiendra un dictionnaire avec comme clé le nom de chaque exception, et comme valeur le contenu de l'exception. <br/>
    /// Le problème, c'est que ces propriétés ne sont pas connues de l'IDE avant le runtime, et donc l'autocomplétion de l'IDE ne propose pas ces propriétés, <br/>
    /// ce qui le rend plus compliqué à utiliser que le paté de propriétés de l'objet pré-défini plus haut. <para/>
    /// C'est pourquoi cette méthode instancie l'objet dynamique 'Get' en même temps que le dictionnaire afin de permettre les deux façons d'appeler les exceptions du fichier JSON. <br/>
    /// Par exemple: <br/>
    ///  - Appel par propriétés: this.exceptionHandler.InvalidMail -> compile, autocomplétion donc pas d'erreur possible, ne compile pas si faute dans le nom<br/>
    ///  - Appel par objet dynamique: this.exceptionHandler.Get.InvalidMl -> compile même avec une faute, pas d'autocomplétion donc erreur possible <br/>
    /// L'intéret de l'objet dynamique aurait été de se débarasser du paté de propriétés, mais pose un gros problème de facilité de développement par la suite.
    /// </summary>
    /// <returns>
    /// Le dictionnaire qui servira à chercher les exceptions du fichier JSON
    /// </returns>
    private Dictionary<string, Exception> ExceptionsRuntimeInitialization(string exceptionsFilepath)
    {
        this.Get = new ExpandoObject();

        Dictionary<string, Exception> exceptions = JsonSerializer.Deserialize<Dictionary<string, Exception>>(File.ReadAllText(exceptionsFilepath), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        });

        foreach (KeyValuePair<string, Exception> entry in exceptions)
        {
            ((IDictionary<string, object>)this.Get)[entry.Key] = entry.Value;
        }

        return exceptions;
    }

    private Exception GetException(string errorName)
    {
        return this.exceptions.TryGetValue(errorName, out Exception exception) ? exception : new Exception();
    }
}

public class Exception
{
    public string Error { get; init; } = "Execution error";
    public string Message { get; init; } = "An error occured";
    public string Example { get; init; } = string.Empty;
}