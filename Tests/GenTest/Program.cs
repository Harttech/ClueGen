using ClueGen.Framework.Generator;
using static System.Console;
using Environment = ClueGen.Framework.Models.Environment.Environment;

var gen = new XorShiftGenerator(978564312);

var environment = new Environment("Test Environment");
var kitchen = environment.CreateChild("Kitchen");
var livingRoom = environment.CreateChild("Living Room");
var bedroom = environment.CreateChild("Bedroom");
var bathroom = environment.CreateChild("Bathroom");
var basement = environment.CreateChild("Basement");
var entrance = environment.CreateChild("Entrance");

entrance.ConnectTo(livingRoom, true);
entrance.ConnectTo(basement, true);
entrance.ConnectTo(bathroom, true);
livingRoom.ConnectTo(kitchen, true);
livingRoom.ConnectTo(bedroom, true);
basement.ConnectTo(kitchen, false);

var options = new CaseOptions
{
    PresetEnvironment = environment,
    PeopleToGenerate = 5,
    NumberGenerator = gen
};

var generatedCase = CaseGenerator.GenerateNewCase(options);

ReadKey();