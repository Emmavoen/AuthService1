
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable ClassNeverInstantiated.Global
namespace AuthService.Domain.Entity
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
    }
}
