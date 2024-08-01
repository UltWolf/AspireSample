using FastEndpoints;
using PrometheusFastApiAspireSample.ApiService.Requests;
using PrometheusFastApiAspireSample.ApiService.Response;

namespace PrometheusFastApiAspireSample.ApiService.Mapper
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
    public class PersonMapper : Mapper<RequestPersonTest, ResponsePersonTest, Person>
    {
        public override Person ToEntity(RequestPersonTest r) => new()
        {
            Id = r.Id,
            DateOfBirth = DateOnly.Parse(r.BirthDay),
            FullName = $"{r.FirstName} {r.LastName}"
        };

        public override ResponsePersonTest FromEntity(Person e) => new()
        {
            Id = e.Id,
            FullName = e.FullName,
            UserName = $"USR{e.Id:0000000000}",
            Age = (DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - e.DateOfBirth.DayNumber) / 365,
        };
    }
}
