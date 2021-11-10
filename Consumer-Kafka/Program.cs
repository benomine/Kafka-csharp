using Confluent.Kafka;
using Newtonsoft.Json;


var consumerConfig = new ConsumerConfig
{
    GroupId = "test-consumer-group",
    BootstrapServers = "localhost:9092"
};

using (var cons = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
{
    cons.Subscribe("test");
    var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        cts.Cancel();
    };

    try
    {
        while (true)
            try
            {
                var cr = cons.Consume(cts.Token);
                var person = JsonConvert.DeserializeObject<Person>(cr.Message.Value);
                Console.WriteLine(person.ToString());
                foreach(var note in person.Notes)
                {
                    Console.WriteLine($"{note.Text} {note.Created}");
                }
                Console.WriteLine(person.GetType());
                Console.WriteLine("---");
            }
            catch (ConsumeException e)
            {
                Console.WriteLine(e);
            }
    }
    catch (Exception)
    {
        cons.Close();
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

