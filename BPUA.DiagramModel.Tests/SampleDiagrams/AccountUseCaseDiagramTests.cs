using BPUA.DiagramModel.Enums;
using BPUA.DiagramModel.Extensions;
using BPUA.DiagramModel.Factories;
using BPUA.DiagramModel.Model;
using BPUA.DiagramModel.Serialization;
using BPUA.DiagramModel.Validation;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace BPUA.DiagramModel.Tests.SampleDiagrams
{
    /// <summary>
    /// Demonstrates the first practical use of BPUA.DiagramModel:
    /// create a use case diagram model in code, validate it, save it to JSON,
    /// load it back, and validate the loaded model again.
    /// </summary>
    public sealed class AccountUseCaseDiagramTests
    {
        #region Fields

        private readonly ITestOutputHelper _output;

        #endregion

        #region Constructors

        public AccountUseCaseDiagramTests(ITestOutputHelper output)
        {
            _output = output;
        }

        #endregion

        #region Tests

        [Fact]
        public void AccountUseCaseDiagram_CanBeCreatedValidatedSavedAndLoaded()
        {
            BpuaDiagram diagram = CreateAccountUseCaseDiagram();

            BpuaDiagramValidator validator = new BpuaDiagramValidator();
            BpuaDiagramValidationResult validationResult = validator.Validate(diagram);

            WriteValidationMessages(validationResult);
            Assert.True(validationResult.IsValid);

            string outputFolderPath = Path.Combine(AppContextBaseDirectory(), "GeneratedDiagrams");
            Directory.CreateDirectory(outputFolderPath);

            string jsonFilePath = Path.Combine(outputFolderPath, "AccountUseCaseDiagram.json");

            BpuaDiagramSerializer serializer = new BpuaDiagramSerializer();
            serializer.SaveToFile(diagram, jsonFilePath);

            Assert.True(File.Exists(jsonFilePath));

            string json = File.ReadAllText(jsonFilePath);
            Assert.False(string.IsNullOrWhiteSpace(json));
            Assert.Contains("Account Use Case", json);
            Assert.Contains("WAITING_FOR_LOGIN", json);
            Assert.Contains("LOGGING_IN", json);

            BpuaDiagram loadedDiagram = serializer.LoadFromFile(jsonFilePath);
            Assert.NotNull(loadedDiagram);
            Assert.NotNull(loadedDiagram.Metadata);
            Assert.NotNull(loadedDiagram.Nodes);
            Assert.NotNull(loadedDiagram.Transitions);

            Assert.Equal("HR", loadedDiagram.Metadata.DomainName);
            Assert.Equal("Account", loadedDiagram.Metadata.UseCaseName);
            Assert.Equal(4, loadedDiagram.Nodes.Count);
            Assert.Equal(3, loadedDiagram.Transitions.Count);

            BpuaDiagramValidationResult loadedValidationResult = validator.Validate(loadedDiagram);
            WriteValidationMessages(loadedValidationResult);
            Assert.True(loadedValidationResult.IsValid);

            _output.WriteLine("Diagram JSON file was created at:");
            _output.WriteLine(jsonFilePath);
        }

        #endregion

        #region Private Methods

        private BpuaDiagram CreateAccountUseCaseDiagram()
        {
            BpuaDiagramFactory factory = new BpuaDiagramFactory();

            BpuaDiagram diagram = factory.CreateDiagram(
                "Account Use Case",
                "HR",
                "Account",
                "BPUA.Account");

            diagram.Metadata.DefaultApplicationLayerName = "SL";
            diagram.Metadata.Description = "Sample diagram used to prove BPUA.DiagramModel creation and JSON serialization.";

            BpuaDiagramNode initialState = factory.CreateState("INITIAL", BpuaStateRole.Entry, 80, 120);
            initialState.ApplicationLayerName = "SL";
            initialState.HandlerClassName = "InitialStateHandler";

            BpuaDiagramNode waitingForLoginState = factory.CreateState("WAITING_FOR_LOGIN", BpuaStateRole.Regular, 360, 120);
            waitingForLoginState.ApplicationLayerName = "SL";
            waitingForLoginState.HandlerClassName = "WaitingForLoginStateHandler";

            BpuaDiagramNode loginDecision = factory.CreateDecision("ARE_CREDENTIALS_VALID", 650, 110);
            loginDecision.ApplicationLayerName = "BL";
            loginDecision.Description = "Decision point representing credential validation outcome.";

            BpuaDiagramNode loggedInExitState = factory.CreateState("LOGGED_IN", BpuaStateRole.Exit, 930, 120);
            loggedInExitState.ApplicationLayerName = "SL";
            loggedInExitState.HandlerClassName = "LoggedInStateHandler";

            diagram.AddNode(initialState);
            diagram.AddNode(waitingForLoginState);
            diagram.AddNode(loginDecision);
            diagram.AddNode(loggedInExitState);

            BpuaDiagramTransition switchingToLoginTransition = factory.CreateTransition(
                "SWITCHING_TO_LOGIN",
                initialState.Id,
                waitingForLoginState.Id,
                BpuaTransitionType.Navigation);
            switchingToLoginTransition.ApplicationLayerName = "SL";
            switchingToLoginTransition.TransitionClassName = "SwitchingToLoginTransition";
            switchingToLoginTransition.HandlerClassName = "SwitchingToLoginTransitionHandler";

            BpuaDiagramTransition loggingInTransition = factory.CreateTransition(
                "LOGGING_IN",
                waitingForLoginState.Id,
                loginDecision.Id,
                BpuaTransitionType.Business);
            loggingInTransition.ApplicationLayerName = "BL";
            loggingInTransition.TransitionClassName = "LoggingInTransition";
            loggingInTransition.HandlerClassName = "LoggingInTransitionHandler";

            BpuaDiagramTransition loginSucceededTransition = factory.CreateTransition(
                "LOGIN_SUCCEEDED",
                loginDecision.Id,
                loggedInExitState.Id,
                BpuaTransitionType.Completion);
            loginSucceededTransition.ApplicationLayerName = "SL";
            loginSucceededTransition.TransitionClassName = "LoginSucceededTransition";
            loginSucceededTransition.HandlerClassName = "LoginSucceededTransitionHandler";
            loginSucceededTransition.GuardName = "CredentialsAreValid";

            diagram.AddTransition(switchingToLoginTransition);
            diagram.AddTransition(loggingInTransition);
            diagram.AddTransition(loginSucceededTransition);

            return diagram;
        }

        private void WriteValidationMessages(BpuaDiagramValidationResult validationResult)
        {
            if (validationResult == null)
            {
                _output.WriteLine("Validation result is null.");
                return;
            }

            int index = 0;
            while (index < validationResult.Messages.Count)
            {
                BpuaDiagramValidationMessage message = validationResult.Messages[index];
                if (message != null)
                {
                    _output.WriteLine(message.Severity + " " + message.Code + " " + message.Message);
                }

                index = index + 1;
            }
        }

        private string AppContextBaseDirectory()
        {
            return System.AppContext.BaseDirectory;
        }

        #endregion
    }
}
