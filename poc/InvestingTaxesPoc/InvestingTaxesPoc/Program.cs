var filePath = args.Any() ? args[0] : "example.csv";
var parser = new CsvStatementParser();
var statement = parser.Parse(filePath);
Console.WriteLine(statement);
Console.ReadLine();
