# BPUA.DiagramModel

`BPUA.DiagramModel` is the UI-independent source model for a future BPUA diagram designer and code generator.

The first version intentionally contains only the core concepts needed for a use-case state diagram:

- State node
- Decision node
- Transition
- Diagram metadata
- JSON serialization
- Structural validation
- Factory helpers

The project does not reference WPF, Blazor, WinUI, Roslyn, or BPUA runtime projects. This keeps it usable from a designer, generator, tests, command-line tools, and documentation examples.

## Suggested usage

```csharp
BpuaDiagramFactory factory = new BpuaDiagramFactory();
BpuaDiagram diagram = factory.CreateDiagram("Account", "HR", "Account", "BPUA.Account");

BpuaDiagramNode initial = factory.CreateState("INITIAL", BpuaStateRole.Entry, 100, 100);
BpuaDiagramNode waiting = factory.CreateState("WAITING_FOR_LOGIN", BpuaStateRole.Regular, 400, 100);
BpuaDiagramTransition transition = factory.CreateTransition("SWITCHING_TO_LOGIN", initial.Id, waiting.Id, BpuaTransitionType.Navigation);

diagram.AddNode(initial);
diagram.AddNode(waiting);
diagram.AddTransition(transition);

BpuaDiagramValidator validator = new BpuaDiagramValidator();
BpuaDiagramValidationResult result = validator.Validate(diagram);

BpuaDiagramSerializer serializer = new BpuaDiagramSerializer();
string json = serializer.ToJsonString(diagram);
```

## Next likely projects

- `BPUA.CodeGeneration`
- `BPUA.Designer.Wpf`
- `BPUA.Designer.Blazor`
