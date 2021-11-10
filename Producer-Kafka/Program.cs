using Confluent.Kafka;
using Newtonsoft.Json;

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

var person = new Person
{
    Name = "Bob",
    Age = 69,
    LastName = "Marley",
    Notes = new List<Note> { new Note
    {
        Text = "Lalalalal",
        Created = DateTime.Now,
        Updated = DateTime.Now
    },
    new Note
    {
        Text = "Tsoin Tsoin",
        Created = DateTime.Now.AddSeconds(3600),
        Updated = DateTime.Now.AddSeconds(5000)
    }
    }
};

using (var p = new ProducerBuilder<string, string>(producerConfig).Build())
{
    while(true) {
        var message = new Message<string, string>()
        {
            Key = null,
            Value = JsonConvert.SerializeObject(person)
        };
        p.ProduceAsync("test", message);
        Thread.Sleep(5000);
    }
}


public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string LastName { get; set; }
    public IEnumerable<Note>? Notes { get; set; }

    public override string ToString()
    {
        return $"{Name} {LastName} is {Age} years old";
    }
}

public class Note
{
    public string? Text { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}

