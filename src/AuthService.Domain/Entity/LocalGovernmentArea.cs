
// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
// ReSharper disable ClassNeverInstantiated.Global
namespace AuthService.Domain.Entity
{
    public class LocalGovernmentArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}
