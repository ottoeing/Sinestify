using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Sinesify;

public class Usuario
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(254)]
    public string Email { get; set; } = string.Empty;

    [JsonIgnore]
    [MaxLength(512)]
    public string SenhaHash { get; set; } = string.Empty;

    [NotMapped]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Required, MinLength(8), MaxLength(128)]
    public string Senha { get; set; } = string.Empty;
}
