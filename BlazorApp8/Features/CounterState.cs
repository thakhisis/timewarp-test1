using System.Text.Json.Serialization;
using TimeWarp.Features.Persistence;
using TimeWarp.State;
using TimeWarp.State.Plus.State;

namespace BlazorApp8.Features
{
    [PersistentState(PersistentStateMethod.SessionStorage)]
    public partial class CounterState : State<CounterState>
    {
        public int Count { get; private set; }
        public override void Initialize() => Count = 1;

        public CounterState() { }

        [JsonConstructor]
        public CounterState(Guid guid, int count) : this()
        {
            this.Guid = guid;
            this.Count = count;
        }
    }

    public partial class CounterState 
    {
        public static class IncrementBy
        {
            public record Action(int Amount) : IAction;

            public class Handler : ActionHandler<Action>
            {
                private readonly ILogger<Handler> logger;

                public Handler(ILogger<Handler> logger, IStore aStore) : base(aStore)
                {
                    this.logger = logger;
                }

                CounterState CurrentCensorkorpsState => Store.GetState<CounterState>();

                public override async Task Handle(Action aAction, CancellationToken aCancellationToken)
                {
                    // set state
                    CurrentCensorkorpsState.Count += aAction.Amount;
                    logger.LogInformation("Incremented by {Amount}", aAction.Amount);
                    await Task.CompletedTask;
                }
            }
        }

        public static class Reset
        {
            public record Action() : IAction;

            public class Handler : ActionHandler<Action>
            {
                private readonly ILogger<Handler> logger;

                public Handler(ILogger<Handler> logger, IStore aStore) : base(aStore)
                {
                    this.logger = logger;
                }

                CounterState CounterState => Store.GetState<CounterState>();

                public override async Task Handle(Action aAction, CancellationToken aCancellationToken)
                {
                    // set state
                    CounterState.Count = 0;
                    logger.LogInformation("Reset");
                    await Task.CompletedTask;
                }
            }
        }
    }
}
